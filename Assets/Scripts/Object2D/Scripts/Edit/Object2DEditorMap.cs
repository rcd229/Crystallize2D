using UnityEngine;
using System.Collections;

public class Object2DEditorMap : TypeAttributeMap<Object2D, Object2DEditorBase, Object2DEditorAttribute> {
    public static readonly Object2DEditorMap Instance = new Object2DEditorMap();
}
