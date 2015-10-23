using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ResourcePath(DialogueActorUtil.ResourcePath)]
public class DialogueActorUtil {
    public const string ResourcePath = "Avatar/DialogueActor";

    public static GameObject GetNewActor(AppearancePlayerData appearance, Transform target) {
        var a = GetNewActor(appearance.GetResourceData());
        //a.transform.parent = target;
        a.transform.position = target.position;
        a.transform.rotation = target.rotation;
        return a;
    }

    /// <summary>
    /// Gets the new actor and set target as its parent
    /// </summary>
    /// <returns>The new actor.</returns>
    /// <param name="appearance">Appearance.</param>
    /// <param name="target">Target which will become parent.</param>
    public static GameObject GetNewActor(AppearanceGameData appearance, Transform target) {
        var a = GetNewActor(appearance);
        a.transform.SetParent(target);
        a.transform.localPosition = Vector3.zero;
        a.transform.localRotation = Quaternion.identity;
        return a;
    }

    public static GameObject GetNewActor(AppearanceGameData appearance, string name) {
        var a = GetNewActor(appearance);
        a.name = name;
        return a;
    }

    public static GameObject GetNewActor(NPCCharacterData characterData) {
        var actor = GetNewActor(characterData.Appearance.GetResourceData());
        characterData.Animation.SetTo(actor);
        return actor;
    }

    public static GameObject GetNewActor(AppearanceGameData appearance) {
        var avatar = AppearanceLibrary.CreateObject(appearance);
        var actor = GameObjectUtil.GetResourceInstance(ResourcePath);
        avatar.transform.SetParent(actor.transform);
        avatar.transform.localPosition = Vector3.zero;
        avatar.transform.localRotation = Quaternion.identity;
        return actor;
    }

    public static List<GameObject> GetActorsForTargets(List<SceneObjectGameData> sceneObjects, out StringMap actorMap) {
        var instances = new List<GameObject>();
        int index = 0;
        actorMap = new StringMap();
        foreach (var t in sceneObjects) {
            if (t.Name.ToLower().Contains("target")) {
                var sceneTarget = GameObject.Find(t.Name);

                var a = GetNewActor(AppearanceLibrary.GetRandomAppearance());
                a.name = string.Format("Actor{0:D2}", index);
                a.transform.parent = sceneTarget.transform;
                a.transform.localPosition = Vector3.zero;
                a.transform.localRotation = Quaternion.identity;
                instances.Add(a);

                actorMap.Set(new StringMapItem(t.Name, a.name));

                index++;
            }
        }
        return instances;
    }

    public static GameObject GenerateDefaultActor() {
        return DialogueActorUtil.GetNewActor(AppearanceLibrary.GetRandomAppearance());
    }

    public static List<GameObject> GenerateActorsForTargets(IEnumerable<string> sceneTargets, Func<GameObject> getActor = null) {
        var positions = sceneTargets.Select(t => GameObject.Find(t).transform.position);
        return GenerateActorsForTargets(positions, getActor);
    }

    public static List<GameObject> GenerateActorsForTargets(IEnumerable<Vector3> positions, Func<GameObject> getActor = null) {
        if (getActor == null) {
            getActor = GenerateDefaultActor;
        }

        List<GameObject> instances = new List<GameObject>();
        int count = 0;
        foreach (var t in positions) {
            var actor = getActor();
            actor.transform.position = t;
            actor.transform.rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360f), Vector3.up);
            actor.name = string.Format("Actor{0:D2}", count);
            instances.Add(actor);
            count++;
        }
        return instances;
    }

    public static List<GameObject> GetActorsForTargetsWithoutTags(List<string> sceneTargets,
        string genderTag = "",
        string hairTag = "",
        string eyeTag = "",
        string bodyTag = "",
        string shirtTag = "",
        string shortTag = "") {
        //List<GameObject> instances = new List<GameObject>();
        //int count = 0;
        //foreach (var n in sceneTargets) {
        //    var t = GameObject.Find(n);
        //    var actor = DialogueActorUtil.GetNewActor(
        //        AppearanceLibrary.RandomAppearanceWithoutTag(genderTag, hairTag, eyeTag, bodyTag, shirtTag, shortTag));
        //    actor.transform.parent = t.transform;
        //    actor.transform.position = t.transform.position;
        //    actor.transform.rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360f), Vector3.up); //t.transform.rotation;
        //    actor.name = string.Format("Actor{0:D2}", count);
        //    instances.Add(actor);
        //    count++;
        //}
        //return instances;
        Func<GameObject> getActor = () => 
            DialogueActorUtil.GetNewActor(AppearanceLibrary.RandomAppearanceWithoutTag(genderTag, hairTag, eyeTag, bodyTag, shirtTag, shortTag));
        return GenerateActorsForTargets(sceneTargets, getActor);
    }

    public static List<GameObject> GetActorsForTargetsWithTag(List<string> sceneTargets,
        string genderTag = "",
        string hairTag = "",
        string eyeTag = "",
        string bodyTag = "",
        string shirtTag = "",
        string shortTag = "") {
        List<GameObject> instances = new List<GameObject>();
        var t = GameObject.Find(sceneTargets[0]);
        var actor = DialogueActorUtil.GetNewActor(
            AppearanceLibrary.RandomAppearanceWithTag(genderTag, hairTag, eyeTag, bodyTag, shirtTag, shortTag));
        actor.transform.parent = t.transform;
        actor.transform.position = t.transform.position;
        actor.transform.rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360f), Vector3.up); //t.transform.rotation;
        actor.name = "Actor_Correct";
        instances.Add(actor);

        var l = GetActorsForTargetsWithoutTags(sceneTargets.Skip(1).Take(sceneTargets.Count - 1).ToList(),
            genderTag, hairTag, eyeTag, bodyTag, shirtTag, shortTag);
        instances.AddRange(l);

        return instances;
    }


    public static List<GameObject> GetActorsForTargetsWithoutTags(List<Vector3> positions,
        string genderTag = "",
        string hairTag = "",
        string eyeTag = "",
        string bodyTag = "",
        string shirtTag = "",
        string shortTag = "") {
        List<GameObject> instances = new List<GameObject>();
        int count = 0;
        foreach (var pos in positions) {
            var actor = DialogueActorUtil.GetNewActor(
                AppearanceLibrary.RandomAppearanceWithoutTag(genderTag, hairTag, eyeTag, bodyTag, shirtTag, shortTag));
            actor.transform.position = pos;
            actor.transform.rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360f), Vector3.up); //t.transform.rotation;
            actor.name = string.Format("Actor{0:D2}", count);
            instances.Add(actor);
            count++;
        }
        return instances;
    }

    public static List<GameObject> GetActorsForTargetsWithTag(List<Vector3> positions,
        string genderTag = "",
        string hairTag = "",
        string eyeTag = "",
        string bodyTag = "",
        string shirtTag = "",
        string shortTag = "") {
        List<GameObject> instances = new List<GameObject>();
        var actor = DialogueActorUtil.GetNewActor(
            AppearanceLibrary.RandomAppearanceWithTag(genderTag, hairTag, eyeTag, bodyTag, shirtTag, shortTag));
        actor.transform.position = positions[0];
        actor.transform.rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360f), Vector3.up); //t.transform.rotation;
        actor.name = "Actor_Correct";
        instances.Add(actor);

        var l = GetActorsForTargetsWithoutTags(positions.Skip(1).Take(positions.Count - 1).ToList(),
            genderTag, hairTag, eyeTag, bodyTag, shirtTag, shortTag);
        instances.AddRange(l);

        return instances;
    }

}
