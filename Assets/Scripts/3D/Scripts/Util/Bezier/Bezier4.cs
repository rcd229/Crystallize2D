using UnityEngine;
using System.Collections;

public class Bezier4 {
	
	public static Vector3 Point(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t){
		return (1 - t) * (1 - t) * (1 - t) * p0 
			+ 3 * (1 - t) * (1 - t) * t * p1 
			+ 3 * (1 - t) * t * t * p2 
			+ t * t * t * p3;
	}
	
	Vector3 p0;
	Vector3 p1;
	Vector3 p2;
	Vector3 p3;
	
	public Bezier4 (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3){
		this.p0 = p0;
		this.p1 = p1;
		this.p2 = p2;
		this.p3 = p3;
	}
	
	public Vector3 Point(float t){
		return (1 - t) * (1 - t) * (1 - t) * p0 
			+ 3 * (1 - t) * (1 - t) * t * p1 
			+ 3 * (1 - t) * t * t * p2 
			+ t * t * t * p3;
	}
	
	public Vector3 Tangent(float t){
		return 3 * (1 - t) * (1 - t) * (p1 - p0) 
			+ 6 * (1 - t) * t * (p2 - p1) 
			+ 3 * t * t * (p3 - p2);
	}
	
}
