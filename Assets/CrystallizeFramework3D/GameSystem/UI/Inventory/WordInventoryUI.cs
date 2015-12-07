using UnityEngine;
using System.Collections.Generic;

public class WordInventoryUI : MonoBehaviour {

    public RectTransform slotParent;
    public GameObject slotPrefab;

    Dictionary<GameObject, int> resources = new Dictionary<GameObject, int>();

    // Use this for initialization
    void Start() {
        Refresh();
        PlayerData.Instance.OnDataChanged += Instance_OnDataChanged;
    }

    void OnDestroy() {
        PlayerData.Instance.OnDataChanged -= Instance_OnDataChanged;
    }

    private void Instance_OnDataChanged(object sender, System.EventArgs e) {
        Debug.Log("Data updated: " + sender);
        if(sender is WordInventory) {
            Refresh();
        }
    }

    void Refresh() {
        foreach (var r in resources) {
            Destroy(r.Key);
        }
        resources = new Dictionary<GameObject, int>();

        for (int i = 0; i < PlayerData.Instance.Inventory.AvailableSlots; i++) {
            var instance = Instantiate(slotPrefab);
            instance.GetComponentInChildren<IInitializable<PhraseSequence>>().Initialize(PlayerData.Instance.Inventory.GetElement(i));
            instance.GetOrAddComponent<WordDropArea>().OnDropped += WordInventoryUI_OnDropped;
            instance.transform.SetParent(slotParent, false);
            resources[instance] = i;
        }
    }

    private void WordInventoryUI_OnDropped(object sender, WordDropArea.DroppedWordArgs e) {
        var c = sender as Component;
        if (c) {
            var i = resources[c.gameObject];
            Debug.Log("Drop complete");
            PlayerData.Instance.Inventory.SetElement(i, e.Phrase);
        }
    }
}
