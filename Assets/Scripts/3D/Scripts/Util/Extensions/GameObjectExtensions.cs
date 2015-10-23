using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static partial class Extensions
{

	public static void SetTextMeshTexts(this GameObject go, string text){
		var tms = go.GetComponentsInChildren<TextMesh>();
		foreach(var t in tms) t.text = text;
	}
	
	public static GameObject Nearest(this GameObject[] gos, Transform transform){
		var pos = transform.position;
		GameObject nearest = null;
		float shortest = 0;

		foreach(var go in gos){
			var dist = Vector3.Distance(pos, go.transform.position);
			
			if(nearest == null){
				nearest = go;
				shortest = dist;
			}
			else if(dist < shortest) {
				nearest = go;
				shortest = dist;
			}
		}
		
		return nearest;
	}	
	
	public static T Nearest<T>(this GameObject go, IEnumerable<T> components) where T : Component{
		var pos = go.transform.position;
		T nearest = null;
		float shortest = 0;

		foreach(var c in components){
			var dist = Vector3.Distance(pos, c.gameObject.transform.position);
			
			if(nearest == null){
				nearest = c;
				shortest = dist;
			}
			else if(dist < shortest) {
				nearest = c;
				shortest = dist;
			}
		}
		
		return nearest;
	}

    public static Transform FindChildWithTag(this Transform transform, string tag) {
        foreach (var t in transform.GetComponentsInChildren<Transform>()) {
            if (t.CompareTag(tag)) {
                return t;
            }
        }
        return null;
    }

    public static T GetOrAddComponent<T>(this GameObject go) where T : Component {
        if (go.GetComponent<T>()) {
            return go.GetComponent<T>();
        } else {
            return go.AddComponent<T>();
        }
    }
	
	/// <summary>
    /// Returns all monobehaviours (casted to T)
    /// </summary>
    /// <typeparam name="T">interface type</typeparam>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public static T[] GetInterfaces<T>(this GameObject gObj)
    {
        if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
        var mObjs = gObj.GetComponents<MonoBehaviour>();

        return (from a in mObjs 
			where a.GetType().GetInterfaces().Any(k => k == typeof(T)) 
			select (T)(object)a).ToArray();
    }

    /// <summary>
    /// Returns the first monobehaviour that is of the interface type (casted to T)
    /// </summary>
    /// <typeparam name="T">Interface type</typeparam>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public static T GetInterface<T>(this GameObject gObj)
    {
        if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");

        if (gObj == null) {
            return default(T);
        }

        return gObj.GetInterfaces<T>().FirstOrDefault();
    }

	public static T GetInterfaceInSelfOrParent<T>(this GameObject gObj)
	{
		//if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
		var i = gObj.GetInterface<T> ();
		if (i != null) {
			return i;
		}

		if (gObj.transform.parent) {
			return gObj.transform.parent.gameObject.GetInterface<T>();
		}

		return default(T);
	}

	public static T GetInterfaceInSelfOrChild<T>(this GameObject gObj)
	{
		//if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
		var i = gObj.GetInterface<T> ();
		if (i != null) {
			return i;
		}
		
		return gObj.GetInterfaceInChildren<T> ();
	}

    /// <summary>
    /// Returns the first instance of the monobehaviour that is of the interface type T (casted to T)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public static T GetInterfaceInChildren<T>(this GameObject gObj)
    {
        if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");

        return gObj.GetInterfacesInChildren<T>().FirstOrDefault();
    }

    /// <summary>
    /// Gets all monobehaviours in children that implement the interface of type T (casted to T)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public static T[] GetInterfacesInChildren<T>(this GameObject gObj)
    {
        if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");

        var mObjs = gObj.GetComponentsInChildren<MonoBehaviour>();
 
        return (from a in mObjs 
			where a.GetType().GetInterfaces().Any(k => k == typeof(T)) 
			select (T)(object)a).ToArray();
    }
	
//	static Dictionary<GameObject, bool> gameObjectLiveStatus = new Dictionary<GameObject, bool>();
	/*public static void SetAlive(this GameObject gameObject, bool alive){
		var living = gameObject.GetInterfacesInChildren<ILiveBehavior>();
		foreach(var l in living){
			l.OnAliveChanged(alive);
		}
		
		var comps = gameObject.GetComponentsInChildren<Component>();
		foreach(var c in comps){
			var mb = c as MonoBehaviour;
			if(mb){
				mb.enabled = alive;
				continue;
			}
			var r = c as Renderer;
			if(r){
				r.enabled = alive;
				continue;
			}
			var cl = c as Collider;
			if(cl){
				cl.enabled = alive;
				continue;
			}
			var rb = c as Rigidbody;
			if(rb){
				rb.isKinematic = !alive;
				continue;
			}
			
			if(c is Transform){
				continue;
			} else if(c is MeshFilter){
				continue;
			}
			Debug.Log(c + " not changed by alive set.");
		}
		gameObject.SetActive(false);
	}*/
	
	/*public static bool GetAlive(this GameObject gameObject){
		if(gameObjectLiveStatus.ContainsKey(gameObject)){
			return gameObjectLiveStatus[gameObject];
		}
		return true;
	}*/

    public static void HideAll (this Transform parent) {
        Renderer[] tempRenderers = parent.GetComponentsInChildren<Renderer>();
        foreach (Renderer tempRenderer in tempRenderers){
			tempRenderer.enabled = false;
		}
    }

    public static void ShowAll (this Transform parent) {
        Renderer[] tempRenderers = parent.GetComponentsInChildren<Renderer>();
        foreach (Renderer tempRenderer in tempRenderers) {
			tempRenderer.enabled = true;
		}
    }

    public static Transform FindBone(this Transform root, string name) {
        Transform[] children = root.GetComponentsInChildren<Transform>();
        foreach (Transform bone in children) {
			if (bone.name == name) {
				return bone;
			}
		}
        return null;
    }
	
	public static Transform FindCenteralBone(this Transform root){
		string[] centerStrings = {"Chest", "Torso", "Center"};
		var children = root.GetComponentsInChildren<Transform>();
		foreach(var child in children){
			foreach(var name in centerStrings){
				if(child.name == name){
					return child;
				}
			}
		}
		return root;
	}
	
	public static Transform FindChildWithNameContains(this Transform root, string containedString){
		Transform[] children = root.GetComponentsInChildren<Transform>();
        foreach (Transform bone in children) {
			if (bone.name.Contains(containedString)) {
				return bone;
			}
		}
        return null;
	}

    public static T AddSingleton<T>(this GameObject go) where T : MonoBehaviour {
        var instances = go.GetComponentsInChildren<T>();
        if (instances.Length > 1) {
            //Debug.LogError("More than one instance of " + typeof(T) + " . Only one instance per GameObject allowed.");
            return null;
        }
        else if (instances.Length == 1) return instances[0];
        else return go.AddComponent<T>();
    }
	
	public static Vector3 ToPlanar(this Vector3 position, Transform transform){
		return position.ToPlanar(transform.position.y);
	}
	
	public static Vector3 ToPlanar(this Vector3 position, float y){
		position.y = y;
		return position;
	}
	
	public static string GetPathString(this Transform t){
		var path = t.name;
		var parent = t.parent;
		while(parent != null){
			path = parent.name + "\\" + path;
			parent = parent.parent;
		}
		return Application.loadedLevelName + "\\" + path;
	}
	
	public static Bounds CombinedBounds(this Transform transform){
		var bounds = new Bounds(transform.position, Vector3.zero);
		if(transform.GetComponent<Rigidbody>()){
			var colliders = transform.GetComponentsInChildren<Collider>();
			foreach(var collider in colliders){
				if(!collider.isTrigger){
					bounds.Encapsulate(collider.bounds);
				}
			}
		} else {
			var renderers = transform.GetComponentsInChildren<Renderer>();
			foreach(var render in renderers){
				bounds.Encapsulate(render.bounds);
			}
		}
		return bounds;
	}
	
	/// <summary>
	/// Tries to find the center of an object using various methods. Probably slow.
	/// </summary>
	/// <returns>
	/// The center of a transform.
	/// </returns>
	/// <param name='transform'>
	/// Transform.
	/// </param>
	public static Vector3 FindCenterOffset(this Transform transform){
		var animator = transform.GetComponent<Animator>();
		if(animator){
			if(animator.isHuman){
				return animator.bodyPosition - transform.position;
			}
		}
		var collider = transform.GetComponent<Collider>();
		if(collider){
			return collider.bounds.center - transform.position;
		}
		var bounds = transform.CombinedBounds();
		return bounds.center - transform.position;
	}
	
	public static Vector3 FindCenterPoint(this Transform transform){
		var collider = transform.GetComponent<Collider>();	
		if(collider){
			return collider.bounds.center;
		}
		var rigidbody = transform.GetComponent<Rigidbody>();
		if(rigidbody){
			return rigidbody.worldCenterOfMass;
		}
		var bounds = transform.CombinedBounds();
		return bounds.center;
	}
	
	public static Bounds GetCombinedBounds(this Rigidbody rigidbody){
		var colliders = rigidbody.GetComponentsInChildren<Collider>();
		Bounds bounds = new Bounds(rigidbody.centerOfMass, Vector3.zero);
		foreach(var collider in colliders){
			bounds.Encapsulate(collider.bounds);
		}
		return bounds;
	}	
	
	public static float FindSize(this Transform transform){
		var bounds = transform.CombinedBounds();
		return Mathf.Max(bounds.size.x, bounds.size.z);
	}
	
	public static IEnumerator LerpColorOverTimeSequence(this Material material, Color finalColor, float time){
		var originalCol = material.color;
		for(float t = 0; t < time; t += Time.deltaTime){
			material.color = Color.Lerp(originalCol, finalColor, t / time);
			
			yield return null;
		}
	}
	
	public static void SetStraightLineBetween(this LineRenderer line, Vector3 first, Vector3 last){
		var delta = last - first;
		line.SetPosition(0, first);
		line.SetPosition(1, first + delta * 0.05f);
		line.SetPosition(2, first + delta * 0.95f);
		line.SetPosition(3, last);
	}
	
	public static Transform GetCoreTransform(this Transform transform){
		while(transform.tag == "Equipment" || transform.tag == "Untagged"){
			if(transform.parent == null){
				return transform;
			}
			
			transform = transform.parent;
		}
		//Debug.Log("Core transform is " + transform);
		return transform;
	}
	
	public static void Stop(this Rigidbody rigidbody){
	    if(!rigidbody.isKinematic){
			rigidbody.velocity = Vector3.zero;
	    	rigidbody.angularVelocity = Vector3.zero;
		}
	    rigidbody.useGravity = false;
	    rigidbody.isKinematic = true;
	}
	
	public static void Start(this Rigidbody rigidbody){
		rigidbody.isKinematic = false; 
	    rigidbody.useGravity = true;
	}
	
	public static void SetAllCollidersEnabled(this Transform transform, bool enabled){
		var all = transform.GetComponentsInChildren<Collider>();
		foreach(var c in all){
			c.enabled = enabled;
		}
	}
	
	public static void SetChildCollidersEnabled(this Transform transform, bool enabled){
		if(transform.GetComponent<Collider>()){
			transform.GetComponent<Collider>().enabled = enabled;
		}
		foreach(Transform c in transform){
			if(c.GetComponent<Collider>()){
				c.GetComponent<Collider>().enabled = enabled;
			}
		}
	}
	
	public static void SetAllMaterialColors(this Renderer renderer, Color color){
		foreach(var mat in renderer.materials){
			mat.color = color;
		}
	}
	
	public static GameObject ReplicateVisual(this GameObject go, Vector3 position){
		var newGO = GameObject.Instantiate(go, position, Quaternion.identity) as GameObject;
		var comps = (from o in newGO.GetComponentsInChildren<Component>()
			where !(o is Renderer) && !(o is MeshFilter)  && !(o is Transform)
			select o).ToList();
		comps.Reverse();
		foreach(var c in comps){
			GameObject.Destroy(c);
		}
		return newGO;
	}
	
	/*public static bool InvokeResult(this GameObject go, string method, float val){
		var comps = go.GetComponents<Component>();
		foreach(var comp in comps){
			
			Assembly.GetExecutingAssembly().GetType("Test").GetMethod(method);
		}
		throw new NotImplementedException();
	}*/
	
	public static T GetComponentInSelfOrChild<T>(this GameObject go) where T : Component {
		var comp = go.GetComponent<T>();
		if(!comp){
			comp = go.GetComponentInChildren<T>();
		}
		return comp;
	}
	
	public static T GetComponentInSelfOrParent<T>(this GameObject go) where T : Component {
		var comp = go.GetComponent<T>();
		if(!comp){
			if(go.transform.parent){
				comp = go.transform.parent.GetComponent<T>();
			}
		}
		return comp;
	}

	public static Vector3 Snap(this Vector3 v, float amount){
		return new Vector3(Mathf.RoundToInt(v.x / amount) * amount,
		                   Mathf.RoundToInt(v.y / amount) * amount,
		                   Mathf.RoundToInt(v.z / amount) * amount);
	}

	public static Vector3 ToXZ(this Vector3 v){
		v.y = 0;
		return v;
	}

    public static Vector2 XZToVector2(this Vector3 v) {
        return new Vector2(v.x, v.z);
    }

    public static float[] ToFloatArray(this Vector3 v) {
        return new float[] { v.x, v.y, v.z };
    }

    public static Vector3 ToVector3(this float[] a) {
        var v = new Vector3();
        for (int i = 0; i < 3 && i < a.Length; i++) {
            v[i] = a[i];
        }
        return v;
    }

	public static float GetColliderHeight(this Collider collider){
//		var cap = collider as CapsuleCollider;
//		if (cap) {
//			return cap.height * collider.transform.lossyScale.y;
//		}
//
//		var box = collider as BoxCollider;
//		if (box) {
//			return box.size.y;
//		}
//
//		var ball = collider as SphereCollider;
//		if (ball) {
//			return ball.radius * 2f;
//		}

		var bounds = collider.bounds;
		return bounds.max.y - bounds.min.y;
	}

	public static string GetTransformPath(this Transform transform){
		var s = transform.name;
		var p = transform.parent;
		while(p != null){
			s +=  "/" + p.name;
			p = p.parent;
		}
		return s;
	}

    public static GameObject GetBaseGameObject(this Collider c) {
        if (!c.attachedRigidbody) {
            return c.gameObject;
        }

        return c.attachedRigidbody.gameObject;
    }

    public static bool IsPlayer(this Collider c) {
        if (!c.attachedRigidbody) {
            return false;
        }

        if (PlayerManager.Instance.PlayerGameObject != c.attachedRigidbody.gameObject) {
            return false;
        }

        if (c.CompareTag("ClothCollider")) {
            return false;
        }

        return true;
    }

    public static bool IsHumanControlled(this Component c) {
        if (c is Collider) {
            var col = (Collider)c;
            if (col.attachedRigidbody) {
                return IsHumanControlled(col.attachedRigidbody);
            }
        }

        return c.gameObject.CompareTag("Player") || c.gameObject.CompareTag("OtherPlayer");
    }

    public static List<T> PickN<T>(this List<T> list, int count) {
        count = Mathf.Min(count, list.Count);
        
        var indicies = new List<int>();
        var choices = new List<T>();
        while (indicies.Count < count) {
            var rand = UnityEngine.Random.Range(0, list.Count);
            if (!indicies.Contains(rand)) {
                choices.Add(list[rand]);
                indicies.Add(rand);
            }
        }
        return choices;
    }

    //public static bool BoundsInView(this Camera cam, ) {
    //    var planes = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(cam), )
    //}
	
}