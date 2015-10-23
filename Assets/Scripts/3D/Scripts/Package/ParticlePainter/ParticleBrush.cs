using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleBrush : MonoBehaviour, IPointCloud {
	
	public GameObject particleObject;
	public float sizeRangePercentVeriance = 0.1f;
	public float maxStep = .1f;

	public bool randomColor = false;
	public Color color1 = Color.white;
	public Color color2 = Color.white;
	public float size;
	public bool sizeLocked = false;

	float baseSize;
	Vector3 lastUsedPoint;

	public Color color {
		get {
			return particleRenderer.color;
		} set{
			particleRenderer.color = value;
		}
	}

	InkParticleBehavior particleRenderer;

	public List<Vector3> points = new List<Vector3>();
	
	protected virtual void Awake () {
		particleRenderer = particleObject.GetComponent<InkParticleBehavior> ();
		particleRenderer.sizeRangePercentVeriance = sizeRangePercentVeriance;
		baseSize = size;
	}

	protected virtual void OnBeginStroke(){}
	protected virtual void OnCompleteStroke(){}

	public void BeginStroke(Vector3 point, float size = 1f){
		OnBeginStroke ();
		lastUsedPoint = point;
		points.Add(point);
		this.size = size;
	}

	public void CompleteStroke(){
		OnCompleteStroke ();
	}

	#region IPointCloud implementation

	public IEnumerable<Vector3> GetPoints ()
	{
		return points;
	}

	public void SetPoints (IEnumerable<Vector3> points)
	{
		Clear ();
		foreach (var p in points) {
			AddPoint(p);
			size = baseSize * (1f + Random.Range(-sizeRangePercentVeriance, sizeRangePercentVeriance));
		}
	}

	#endregion

	public bool AddSegment(Vector3 point){
		float d = Vector3.Distance (point, lastUsedPoint);
		if (d > 0.1f) {
			var a = - Mathf.Rad2Deg * Mathf.Atan2(point.z - lastUsedPoint.z, point.x - lastUsedPoint.x);
			int steps = (int)(d / maxStep) + 1;
			for(int i = 0; i < steps; i++){
				var p = Vector3.Lerp(lastUsedPoint, point, ((float)i+1f)/((float)steps));
				AddPoint(p, a);
			}
			lastUsedPoint = point;
			return true;
		}
		return false;
	}

	public void AddPoint(Vector3 point, float angle = 0){
		PaintPoint (point, angle);
		points.Add(point);
	}

	void PaintPoint(Vector3 point, float angle = 0){
		if(randomColor){
			var c = Color.Lerp(color1, color2, Random.value);
			particleRenderer.color = c;
			particleRenderer.AddCluster(point, size, angle);
		} else {
			particleRenderer.AddCluster(point, size, angle);
		}
	}

	public void EraseSphere(Vector3 center, float radius){
		var removed = new Stack<Vector3> ();
		foreach (var p in points) {
			if(Vector3.Distance(p, center) <= radius){
				removed.Push(p);
			}
		}

		foreach (var p in removed) {
			points.Remove(p);
		}

		particleRenderer.Clear ();
		foreach (var p in points) {
			PaintPoint(p);
		}
	}

	public void Clear(){
		points.Clear ();
		particleRenderer.Clear ();
	}
}
