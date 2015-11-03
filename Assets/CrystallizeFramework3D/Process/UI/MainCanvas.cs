using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CanvasElement {
    public Transform BranchRoot { get; private set; }
    public Dictionary<CanvasBranch, CanvasElement> Branches { get; private set; }
    public Stack<CanvasElement> LayerStack { get; private set; }

    public CanvasElement TopLayer {
        get {
            if (LayerStack.Count == 0) {
                LayerStack.Push(GetNewLayer("DefaultLayer"));
            }
            return LayerStack.Peek();
        }
    }

    public CanvasElement(Transform branchRoot) {
        BranchRoot = branchRoot;
        Branches = new Dictionary<CanvasBranch, CanvasElement>();
        LayerStack = new Stack<CanvasElement>();
    }

    public void Add(Transform child, CanvasBranch branch) {
        if (branch == CanvasBranch.None) {
            child.SetParent(TopLayer.BranchRoot, false);
        } else {
            GetBranch(branch).Add(child, CanvasBranch.None);
        }
    }

    public CanvasElement GetBranch(CanvasBranch branch) {
        if (branch == CanvasBranch.None) {
            return this;
        }

        if (!Branches.ContainsKey(branch)) {
            Branches[branch] = GetNewLayer(branch.ToString());
            var branchList = Branches.ToList();
            branchList.Add(new KeyValuePair<CanvasBranch, CanvasElement>(CanvasBranch.None, TopLayer));
            var orderedBranches = from kv in branchList
                                  orderby (int)kv.Key descending
                                  select kv;
            foreach (var kv in orderedBranches) {
                kv.Value.BranchRoot.transform.SetAsFirstSibling();
            }
        }
        return Branches[branch];
    }

    public void PushLayer() {
        TopLayer.BranchRoot.gameObject.SetActive(false);
        LayerStack.Push(GetNewLayer("LayerStack" + LayerStack.Count));
    }

    public void PopLayer() {
        if (LayerStack.Count <= 1) {
            Debug.LogWarning("Layer stack is empty");
            return;
        }

        GameObject.Destroy(LayerStack.Pop().BranchRoot.gameObject);
        LayerStack.Peek().BranchRoot.gameObject.SetActive(true);
    }

    CanvasElement GetNewLayer(string name) {
        var newLayer = new GameObject(name).AddComponent<RectTransform>();
        newLayer.anchorMin = Vector2.zero;
        newLayer.anchorMax = Vector2.one;
        newLayer.sizeDelta = Vector2.zero;
        newLayer.SetParent(BranchRoot.transform, false);
        return new CanvasElement(newLayer);
    }
}

public class MainCanvas : MonoBehaviour {

    const string ResourcePath = "MainCanvas";

    static MainCanvas _main;
    public static MainCanvas main {
        get {
            if (!_main) {
                _main = GameObjectUtil.GetResourceInstance<MainCanvas>(ResourcePath);
            }
            return _main;
        }
        set {
            _main = value;
        }
    }

    CanvasElement root;

    // Use this for initialization
    void Awake() {
        main = this;

        root = new CanvasElement(transform);
    }

    public void Add(Transform ui, CanvasBranch branch = CanvasBranch.Default) {
        if (branch == CanvasBranch.Root) {
            ui.SetParent(transform, false);
        } else {
            root.TopLayer.Add(ui, branch);
        }
    }

    public GameObject AddNew(GameObject prefab, CanvasBranch branch = CanvasBranch.Default) {
        var instance = Instantiate<GameObject>(prefab);
        Add(instance.transform, branch);
        return instance;
    }

    public void PushLayer(CanvasBranch branch = CanvasBranch.None) {
        if (branch == CanvasBranch.None) {
            root.PushLayer();
        } else {
            root.TopLayer.GetBranch(branch).PushLayer();
        }
    }

    public void PopLayer(CanvasBranch branch = CanvasBranch.None) {
        if (branch == CanvasBranch.None) {
            root.PopLayer();
        } else {
            root.TopLayer.GetBranch(branch).PopLayer();
        }
    }

}
