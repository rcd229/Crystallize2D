using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WorkPlace : MonoBehaviour {
	//TODO specify a resource path
	List<GameObject> actors = new List<GameObject>();
	int index = 0;
	int ActorCount {get{return actors.Count;}}
	public int MaxActors = 5;
	JobID jobId;

	void Awake(){
		jobId = GetComponent<JobRefContainer>().jobID;
	}

	//requires the region to have a trigger collider and a rigidbody
	void OnTriggerEnter(Collider other) {
		if(other.IsPlayer()){
			StartCoroutine(SpawnJobNPC());
		}
	}

	void OnTriggerExit(Collider other){
		if(other.IsPlayer()){
			StopCoroutine(SpawnJobNPC());
		}
	}

	IEnumerator SpawnJobNPC(){
		yield return new WaitForSeconds(10f);
		if(ActorCount < MaxActors){
			var go = GameObjectUtil.GetResourceInstance(JobNPC.ResourcePath);
			List<GameObject> list = new List<GameObject>();
			list.Add(go);
			SceneAreaUtil.ScatterTargets(gameObject.name, list, default(Vector3));
			go.GetComponent<JobRefContainer>().jobID = jobId;
			actors.Add(go);
			JobNPC npc = go.GetComponent<JobNPC>();
			npc = JobNPCGameData.DefaultJobNPCGameData.Get(npc);
			go.name = npc.CharacterName + index;
			npc.OnDestroyed += HandleGOOnDestroyed;
			EnergyManager.Instance.AddConsumerOrDefault(npc);
			//generate avatar
			var loader = new TargetLoadUtil(go.name, AppearanceLibrary.GetRandomAppearance());
			loader.Instantiate();
			index++;
		}
	}

	void HandleGOOnDestroyed (object sender, EventArgs<object> e)
	{
		var npc = (JobNPC) sender ;
		npc.OnDestroyed -= HandleGOOnDestroyed;
		actors.Remove(npc.gameObject);
	}


}
