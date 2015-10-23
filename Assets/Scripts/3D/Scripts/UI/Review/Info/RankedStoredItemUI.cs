using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class RankedStoredItemUI : MonoBehaviour, IInitializable<int>, IInitializable<Color> {

    public void Initialize(int rank) {
        transform.FindChild("Glow").GetComponent<Image>().enabled = false;
        GetComponent<CanvasGroup>().alpha = 1f;
        if (rank == 0) {
            GetComponent<CanvasGroup>().alpha = 0.7f;
        } else if (rank == 2) {
            transform.FindChild("Glow").GetComponent<Image>().enabled = true;
        }
    }

    public void Initialize(Color color) {
        transform.FindChild("Background").GetComponent<Image>().color = color;
    }
}
