using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

namespace Util.Serialization
{
    public class Serializer {

        static Type[] extraTypes;

        static Serializer() {
            extraTypes = (from t in Assembly.GetAssembly(typeof(Serializer)).GetTypes()
                          where t.HasAttribute<XmlExtraTypeAttribute>()
                          select t).ToArray();
        }

        public static void SaveWithBackups(string path, string data) {
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }

            using (var writer = new StreamWriter(path + ".tmp")) {
                writer.Write(data);
            }

            if (File.Exists(path + ".bak2")) {
                File.Delete(path + ".bak2");
            }

            if (File.Exists(path + ".bak1")) {
                File.Move(path + ".bak1", path + ".bak2");
            }

            if (File.Exists(path)) {
                File.Move(path, path + ".bak1");
            }

            File.Move(path + ".tmp", path);
        }

        //public static string GetOrCreateDirectory(string path) {
        //    if (!Directory.Exists(path)) {
        //        Directory.CreateDirectory(path);
        //    }
        //    return path;
        //}

        public static void Serialize(string filename, IDictionary dictionary) {
            using (StreamWriter writer = new StreamWriter(filename, false)) {
                foreach (var key in dictionary.Keys) writer.WriteLine(key + "\t" + dictionary[key]);
            }
        }
        public static void Serialize(string filename, IEnumerable list) {
            using (StreamWriter writer = new StreamWriter(filename, false)) {
                foreach (var c in list) writer.WriteLine(c);
            }
        }

        public static Dictionary<string, float> DeserializeDictionary(string filename) {
            Dictionary<string, float> dict = new Dictionary<string, float>();

            if (!File.Exists(filename)) {
                Console.WriteLine(filename + " not found.");
                return dict;
            }

            using (StreamReader reader = new StreamReader(filename)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    dict[line.Split('\t')[0]] = float.Parse(line.Split('\t')[1]);
                }
            }
            return dict;
        }

        public static IEnumerable<char> DeserializeSet(string filename) {
            List<char> set = new List<char>();

            if (!File.Exists(filename)) {
                Console.WriteLine(filename + " not found.");
                return set;
            }

            using (StreamReader reader = new StreamReader(filename)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    set.Add(line[0]);
                }
            }
            return set;
        }

        public static void SaveToXml<T>(string filePath, T data, bool printMessage = true) where T : class {
            var dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }

            using (var writer = new StreamWriter(filePath)) {
                var serializer = new XmlSerializer(typeof(T), extraTypes);
                serializer.Serialize(writer, data);
                if (printMessage) {
                    Debug.Log("Saved to " + filePath);
                }
            }
        }

        public static string SaveToXmlString<T>(T data) where T : class {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(T), extraTypes);
            serializer.Serialize(stringwriter, data);
            return stringwriter.ToString();
        }

        public static T LoadFromXml<T>(string filePath, bool printMessage = true) where T : class {
            if (!File.Exists(filePath)) {
                //Debug.Log(filePath + " does not exist");
                return null;
            }

            //Debug.Log("Loading from " + filePath);
            using (var reader = new StreamReader(filePath)) {
                var serializer = new XmlSerializer(typeof(T), extraTypes);
                var data = (T)serializer.Deserialize(reader);
                if (printMessage) {
                    Debug.Log("Loaded from " + filePath);
                }
                return data;
            }
        }

        public static T LoadFromXmlString<T>(string text) where T : class {
            using (var reader = new StringReader(text)) {
                var serializer = new XmlSerializer(typeof(T), extraTypes);
                var data = (T)serializer.Deserialize(reader);
                //Debug.Log("Loaded from text.");
                return data;
            }
        }

        public static void SaveDictionaryToXml<K, V>(string filePath, Dictionary<K, V> dictionary) {
            var keyValueList = new List<KeyValuePair<K, V>>();
            foreach (var kv in dictionary) {
                keyValueList.Add(kv);
            }

            SaveToXml<List<KeyValuePair<K, V>>>(filePath, keyValueList);
        }

        public static Dictionary<K, V> LoadDictionaryFromXml<K, V>(string filePath) {
            var dictionary = new Dictionary<K, V>();
            var list = LoadFromXml<List<KeyValuePair<K, V>>>(filePath);
            if (list == null) {
                return new Dictionary<K, V>();
            }

            foreach (var kv in list) {
                dictionary.Add(kv.Key, kv.Value);
            }
            return dictionary;
        }

        public static void SaveToBinary<T>(string filePath, T obj) {
            var dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }

            var file = new FileStream(filePath, FileMode.Create);
            var bf = new BinaryFormatter();
            bf.Serialize(file, obj);
            file.Close();
            Debug.Log(string.Format("[{0}] Saved to {1}", DateTime.Now, filePath));
        }

        public static void SaveToFile(string filePath, string content) {
            using (var writer = new StreamWriter(filePath, false)) { 
                writer.Write(content); 
            }
        }

        public static string LoadFromFile(string filePath) {
            string s;
            using (var reader = new StreamReader(filePath)) {
                s = reader.ReadToEnd();
            }
            return s;
        }

        public static string GetBytesForObject<T>(T obj) where T : class {
            var o = new MemoryStream();
            var bf = new BinaryFormatter();
            bf.Serialize(o, obj);
            return Convert.ToBase64String(o.GetBuffer());
        }

        public static T GetObjectForBytes<T>(string bytes) {
            var i = new MemoryStream(Convert.FromBase64String(bytes));
            var bf = new BinaryFormatter();
            return (T)bf.Deserialize(i);
        }

        public static T LoadFromBinary<T>(string filePath) {
            if (!File.Exists(filePath)) {
                Debug.Log(filePath + " not found.");
                return default(T);
            }

            var file = new FileStream(filePath, FileMode.Open);
            var bf = new BinaryFormatter();
            var obj = (T)bf.Deserialize(file);
            file.Close();
            Debug.Log("Loaded from " + filePath);

            return obj;
        }

        public static void SaveToPNG(string filePath, Texture2D texture) {
            var bytes = texture.EncodeToPNG();
            var file = File.Open(filePath, FileMode.Create);
            var binary = new BinaryWriter(file);
            binary.Write(bytes);
            file.Close();
            binary.Close();
            Debug.Log("Saved image to " + filePath);
        }

        public static byte[] LoadFromPNG(string filePath) {
            if (File.Exists(filePath)) {
                var file = File.Open(filePath, FileMode.Open);
                var bin = new BinaryReader(file);
                return bin.ReadBytes(int.MaxValue);
            } else {
                return null;
            }
        }

    }
}
