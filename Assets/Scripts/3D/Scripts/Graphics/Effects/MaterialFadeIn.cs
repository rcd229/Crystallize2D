using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class MaterialFadeIn : MonoBehaviour {
    public static MaterialFadeIn Get(GameObject target) {
        var p = target.GetComponent<MaterialFadeIn>();
        if (!p) {
            p = target.AddComponent<MaterialFadeIn>();
        }
        return p;
    }

    List<Renderer> renderers = new List<Renderer>();
    Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();

    IEnumerator Start() {
        foreach (var r in GetComponentsInChildren<Renderer>()) {
            renderers.Add(r);
            originalMaterials[r] = r.sharedMaterial;

            var material = new Material(r.material);
            material.SetFloat("_Mode", 2);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
            r.material = material;
        }

        for (float t = 0; t < 1f; t += Time.deltaTime) {
            if (!UpdateRenderers(t)) {
                yield break;
            }
            
            yield return null;
        }

        foreach (var r in renderers) {
            if (r) {
                r.material = originalMaterials[r];
            }
        }

        Destroy(this);
    }

    bool UpdateRenderers(float t) {
        foreach (var r in renderers) {
            if (r) {
                if (!UpdateRenderer(r, t)) {
                    return false;
                }
            }
        }
        return true;
    }

    bool UpdateRenderer(Renderer renderer, float t) {
        if (!renderer) {
            return false;
        }
        renderer.material.color = renderer.material.color.SetTransparency(t);
        return true;
    }

}
