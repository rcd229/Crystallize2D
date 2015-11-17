using System;
using UnityEngine;
using System.Collections.Generic;

[Object2DBehaviour(typeof(NPC2D))]
public class SceneNPC2DBehaviour : MonoBehaviour, IInteractable2D
{
    public void Interact()
    {
        List<string> strings = new List<string>();
        strings.Add("Hello World");
        strings.Add("Welcome to Crystallize 2D");
        strings.Add("...lol...");
        var npc = (NPC2D)GetComponent<Object2DComponent>().Object;
        TextDisplayUI.Instance.Play(npc.Dialogue.Description.Split('\n'));
    }
}
