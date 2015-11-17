using System;
using UnityEngine;
using System.Collections.Generic;

[Object2DBehaviour(typeof(SceneNPC2D))]
public class SceneNPC2DBehaviour : MonoBehaviour, IInteractable2D
{
    public void Interact()
    {
        List<string> strings = new List<string>();
        strings.Add("Hello World");
        strings.Add("Welcome to Crystallize 2D");
        strings.Add("...lol...");
        TextDisplayUI.Instance.Play(strings);
    }
}
