using UnityEngine;
using UnityEditor;
using System.Collections;

//[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : EditorWindow { 

	GameObject terrain;
	string[] layers;

	[MenuItem("Utils/Generator")]
	public static void Open(){
		GetWindow<MapGeneratorEditor> ();
	}

	void OnGUI(){
		terrain = 
			(GameObject)EditorGUI.ObjectField (new Rect (3, 40, position.width - 6, 20), "terrain", terrain, typeof(GameObject), true);

		if (GUILayout.Button ("Generate level")) {
			MapGenerator generator = new MapGenerator(terrain);
			generator.LayerNames = layers;
			generator.generateMap();
		}

		EditorGUILayout.LabelField ("This is a label");
	}
	
}
