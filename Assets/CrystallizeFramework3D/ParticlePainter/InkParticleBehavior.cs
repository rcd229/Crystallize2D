using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SParticle = UnityEngine.ParticleSystem.Particle;

public class InkParticleBehavior : MonoBehaviour {
	protected List<SParticle> particles = new List<SParticle>();
	
	public float density = 5f;
	public float maxDistance = 0.5f;
	public float minSize = 0.5f;
	public float maxSize = 2f;
	public float sizeRangePercentVeriance = 0.1f;
	public Vector3 offset = Vector3.zero;
	public Color color = Color.black;
	public bool useAngle = true;

	public bool forceRenderQueue = false;
	public int renderQueue = 3000;

	public int Count {
		get {
			return particles.Count;
		}
	}

	void Start(){
		if (forceRenderQueue) {
			GetComponent<Renderer>().material.renderQueue = renderQueue;
		}
	}

	public void Apply(){
		GetComponent<ParticleSystem>().SetParticles(particles.ToArray(), particles.Count);
	}

	public void Clear(){
		particles.Clear ();
		Apply ();
	}
	
	public void AddCluster(Vector3 point, float size, float angle, bool refresh = true){
		for(int k = 0; k < density; k++){
			var part = new SParticle();
			part.color = color;
			part.size = Random.Range(size * (1f - sizeRangePercentVeriance), size * (1f + sizeRangePercentVeriance));
			part.position = point + 
				new Vector3(
					(2f*maxDistance)*Random.value - maxDistance, 
					(2f*maxDistance)*Random.value - maxDistance, 
					(2f*maxDistance)*Random.value - maxDistance)
					+ offset;
			if(useAngle){
				part.rotation = angle;
			}
			particles.Add(part);
		}
		if(refresh) {
			Apply();
		}
	}

	public void SetColor(Color c){
		for (int i = 0; i < particles.Count; i++) {
			var p = particles[i];
			p.color = c;
			particles[i] = p;
		}
		Apply ();
	}
}
