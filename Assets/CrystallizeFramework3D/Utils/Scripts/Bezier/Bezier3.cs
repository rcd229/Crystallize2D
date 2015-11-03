using UnityEngine;
using System.Collections;

public class Bezier3 {
	
	public static Vector3 Point(Vector3 start, Vector3 controlPoint, Vector3 end, float t){
		return (1-t)*(1-t)*start + 2*(1-t)*t*controlPoint + t*t*end;
	}
	
	Vector3 start;
	Vector3 controlPoint;
	Vector3 end;
	
	public Bezier3 (Vector3 start, Vector3 controlPoint, Vector3 end){
		this.start = start;
		this.controlPoint = controlPoint;
		this.end = end;
	}
	
	public Vector3 Point(float t){
		return (1-t)*(1-t)*start + 2*(1-t)*t*controlPoint + t*t*end;
	}
	
	public Vector3 Tangent(float t){
		return 2*(1-t)*(controlPoint - start) + 2*t*(end - controlPoint);
	}
	
}
