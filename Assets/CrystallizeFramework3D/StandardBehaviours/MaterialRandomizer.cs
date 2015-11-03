using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MaterialRandomizer : MonoBehaviour {

    public List<Renderer> randomMaterials = new List<Renderer>();

    static Color GetRandomColor() {
        return new Color(Random.value, Random.value, Random.value);
    }

    void Start() {
        Randomize();
    }

    public void Randomize() {
        foreach (var m in randomMaterials) {
            m.material.color = GetRandomColor();
        }
    }

}