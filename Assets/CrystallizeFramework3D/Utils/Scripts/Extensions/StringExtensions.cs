using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public static class StringExtensions {

    public static string Truncate(this string s, int length) {
        if (s == null) {
            return s;
        }
        
        if (s.Length <= length) {
            return s;
        }
        return s.Substring(0, length);
    }

    public static bool IsEmptyOrNull(this string s) {
        if (s == null) {
            return true;
        }
        if (s == "") {
            return true;
        }
        return false;
    }

    public static IEnumerable<string> GetCountSet(string baseString, int min, int max, int digits) {
        var strings = new List<string>(max - min);
        for (int i = min; i <= max; i++) {
            strings.Add(string.Format("{0}{1:D" + digits + "}", baseString, i));
        }
        return strings;
    }

    static byte[] ToByteArray(this string str) {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

    public static string GetUnicodeValue(char c) {
        var s = string.Format(" {0,-5}", CharUnicodeInfo.GetNumericValue(c));
        Debug.Log(s);
        return s;
    }

    public static bool IsVowel(this char c) {
        return "aeiouAEIOU".Contains(c.ToString());
    }

    public static string GetArticle(this string word) {
        if (word[0].IsVowel()) {
            return "an";
        } else {
            return "a";
        }
    }

    public static string GetPossessivePronoun(bool isMale) {
        if (isMale) {
            return "his";
        } else {
            return "her";
        }
    }

    public static string GetDOPronoun(bool isMale) {
        if (isMale) {
            return "him";
        } else {
            return "her";
        }
    }

}