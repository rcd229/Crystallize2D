using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class PropType : ResourceType<GameObject> {

    public static readonly PropType GreenArea = new PropType("ColoredArea/GreenArea", "green area");
    public static readonly PropType BlueArea = new PropType("ColoredArea/BlueArea", "blue area");
    public static readonly PropType RedArea = new PropType("ColoredArea/RedArea", "red area");
    public static readonly PropType YellowArea = new PropType("ColoredArea/YellowArea", "yellow area");
    public static readonly PropType PurpleArea = new PropType("ColoredArea/PurpleArea", "purple area");
    public static readonly PropType BrownArea = new PropType("ColoredArea/BrownArea", "brown area");
    public static readonly PropType WhiteArea = new PropType("ColoredArea/WhiteArea", "white area");
    public static readonly PropType BlackArea = new PropType("ColoredArea/BlackArea", "black area");

    public static readonly PropType Table = new PropType("Furnature/Table", "table");
    public static readonly PropType Chair = new PropType("Furnature/Chair", "chair");
    public static readonly PropType PottedPlant = new PropType("Furnature/Plant", "potted plant");

	public static readonly PropType ExclaimationMark = new PropType("TaskNotice/Exclaimation");


    protected override string ResourceDirectory { get { return "Prop/"; } }

    string[] labels = new string[0];

    public string Label {
        get {
            if (labels.Length > 0) {
                return labels[0];
            }
            return null;
        }
    }

    public string[] Labels {
        get {
            return labels;
        }
    }

    PropType(string name, params string[] labels) : base(name) {
        this.labels = labels;
    }

	/// <summary>
	/// put a prop at a target and return the prop
	/// </summary>
	public static GameObject GetPropForTarget(PropType theProp, Transform target){
		var instance = GameObjectUtil.GetResourceInstance(theProp.ResourcePath);
		instance.transform.parent = target.transform;
		instance.transform.localPosition = Vector3.zero;
		instance.transform.localRotation = Quaternion.identity;
		return instance;
	}

}