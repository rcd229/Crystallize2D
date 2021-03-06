﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PulseGlow : MonoBehaviour, IMouseOverEffect {

    List<Renderer> renderers = new List<Renderer>();

    void Start() {
        
    }

	// Update is called once per frame
	void Update () {
        foreach (var r in renderers) {
            if (r) {
                UpdateRenderer(r);
            }
        }
	}

    void OnEnable() {
        foreach (var r in GetComponentsInChildren<Renderer>()) {
            renderers.Add(r);
            r.material.EnableKeyword("_EMISSION");
        }
    }

    void OnDisable() {
        foreach (var r in renderers) {
            if (r) {
                r.material.SetColor("_EmissionColor", Color.clear);
            }
        }
    }

    public virtual void UpdateRenderer(Renderer renderer) {
        Material mat = renderer.material;

        float emission = Mathf.PingPong(Time.time * 2f, 1.0f) * 0.25f + 0.25f;
        Color baseColor = Color.yellow;

        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

        mat.SetColor("_EmissionColor", finalColor);
    }

    public void SetEnabled(bool enabled) {
        this.enabled = enabled;
    }
}
