using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class MapGenerator {

	GameObject terrain;

	float acc = 0.05f;
	float height = 200f;
	float range = 200f;

	int maxX = 0;
	int minX = 0;
	int maxZ = 0;
	int minZ = 0;

	
	public const float playerHeight = 2.2f;
	public const float noHit = -1000f;


	float minY = 10f;

	public string[] LayerNames;
	public string path = "/output.png";

	class CoordinateComparator : IEqualityComparer<int[]>{

		public bool Equals(int[] x, int[] y){
			if (x.Length != y.Length){
				return false;
			}
			for (int i = 0; i < x.Length; i++){
				if (x[i] != y[i]){
					return false;
				}
			}
			return true;
		}
		
		public int GetHashCode(int[] obj){
			int result = 17;
			for (int i = 0; i < obj.Length; i++){
				unchecked{
					result = result * 23 + obj[i];
				}
			}
			return result;
		}
	}

	public MapGenerator(GameObject terrain){
		this.terrain = terrain;
	}

	public void generateMap (){
		Color[] map = getMap ();
		Texture2D texture = new Texture2D (maxX - minX + 1, maxZ - minZ + 1);
		texture.SetPixels (map);
		texture.Apply();
		byte[] mapBytes = texture.EncodeToPNG ();
		//write to the directory
		Debug.Log("outputing");
		Debug.Log ("min Y = "+minY);
		File.WriteAllBytes (Application.dataPath + path, mapBytes);
	}
	/**
	 * Scans the [terrain] and generate a one-dimensional Color[] for picture generation
	 * Requires the terrain is simply connected, i.e. no holes, no discontinuities
	 * Returns the Color[] with white as passable area, black as blocked area and grey as area out of range
	 **/
	Color[] getMap() {
		//initializes a dictionary that stores the raycast result for each coordinate
		Dictionary<int[], bool> rayInfo = new Dictionary<int[], bool> (new CoordinateComparator());
		//positive x
		iterateX (true, rayInfo);
		//negative x
		iterateX (false, rayInfo);
		//based on the max and min values of x and z, create an array of pixels
		Color[,] pixels = new Color[maxZ - minZ + 1, maxX - minX + 1];
		//assign values to the pixels
		Color w = Color.white;
		Color b = Color.black;
		Color e = Color.gray;
		for (int i = 0; i < pixels.GetLength(0); i++) {
			for(int j = 0; j < pixels.GetLength(1); j++){
				int[] key = {j + minX, i + minZ};
				bool v;
				if(rayInfo.TryGetValue(key, out v)){
					if(v)
						pixels[i,j] = w;
					else
						pixels[i,j] = b;
				}
				else{
					pixels[i,j] = e;
				}
			}
		}
		//convert to oneDimensional array
		Color[] ret = new Color[pixels.Length];
		int width = pixels.GetLength (1);
		for (int i = 0; i < pixels.GetLength(0); i++) {
			for(int j = 0; j < width; j++){
				ret[i * width + j] = pixels[i,j];
			}
		}
		return ret;
	}
	/**
	 * Iterate through X axis of the terrain
	 * use a flag to decide whether to search in positive or negative direction
	 */
	void iterateX(bool isPositive, Dictionary<int[], bool> dict){
		int x = 0;
		bool hasNext = true;
		while (hasNext) {
			Debug.Log("x: "+ x);
			bool iterate1 = iterateZ(x, true, dict);
			bool iterate2 = iterateZ(x, false, dict);
			hasNext = iterate1 || iterate2;
			if(isPositive)
				x++;
			else
				x--;
		}
		maxX = Mathf.Max (maxX, x);
		minX = Mathf.Min (maxX, x);
	}
	bool iterateZ(int x, bool isPositive, Dictionary<int[], bool> dict){
		bool hasNext = false;
		int z = 0;

		Ray topDownRay = new Ray(new Vector3(x/acc, height, z/acc), Vector3.down);
		Ray bottomUpRay = new Ray(new Vector3(x/acc, -50f, z/acc), Vector3.up);

		RaycastHit[] topDownRayouts = Physics.RaycastAll(topDownRay).Where(s => !s.collider.isTrigger).ToArray();
		RaycastHit[] bottomUpRayouts = Physics.RaycastAll(bottomUpRay).Where(s => !s.collider.isTrigger).ToArray();

		while(topDownRayouts.Length > 0){
			Debug.Log("z: "+ z);
			hasNext = true;
			var bottomUpResults = bottomUpRayouts.OrderBy(s => s.point.y);
			var bottomUpHit = 0f;
			if(bottomUpResults.Count() == 0){
				bottomUpHit = noHit;
			}else{
				bottomUpHit = bottomUpResults.First().point.y;
			}
			var topDownHit = topDownRayouts.OrderBy(s => s.point.y).First().point.y;

			if(topDownHit< minY)
				minY = topDownHit;
			processHit(dict, x, z, topDownHit, bottomUpHit);
			if(isPositive)
				z++;
			else
				z--;

			topDownRay = new Ray(new Vector3(x/acc, height, z/acc), Vector3.down);
			bottomUpRay = new Ray(new Vector3(x/acc, 0f, z/acc), Vector3.up);
			
			topDownRayouts = Physics.RaycastAll(topDownRay).Where(s => !s.collider.isTrigger).ToArray();
			bottomUpRayouts = Physics.RaycastAll(bottomUpRay).Where(s => !s.collider.isTrigger).ToArray();
		}
		maxZ = Mathf.Max (z, maxZ);
		minZ = Mathf.Min (z, minZ);
		return hasNext;
	}

	void processHit(Dictionary<int[], bool> dict, int x, int z, float topHit, float bottomHit){
		int[] key = new int[] {x, z};
		//float point
		bool navigable = false;
		if(bottomHit == noHit){
			if(topHit <= 0f){
				navigable = true;
			}
		}else{
			if(bottomHit - topHit >= playerHeight){
				navigable = true;
			}
		}
		dict[key] = navigable;
	}
}
