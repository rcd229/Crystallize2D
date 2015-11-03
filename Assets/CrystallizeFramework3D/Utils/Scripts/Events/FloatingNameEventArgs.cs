using UnityEngine;
using System.Collections;

public class FloatingNameEventArgs : System.EventArgs {

	public Transform Holder { get; private set; }
    public string Name { get; private set; }
    public Color Color { get; private set; }

	public FloatingNameEventArgs(Transform holder, string name, Color color = default(Color)){
		this.Holder = holder;
        this.Name = name;
        this.Color = Color.white;
        if (color != default(Color)) {
            this.Color = color;
        }
	}

}
