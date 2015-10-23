using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class IndicatorComponent : MonoBehaviour {

    static HashSet<IndicatorComponent> _indicators = new HashSet<IndicatorComponent>();
    public static IEnumerable<IndicatorComponent> Indicators { get { return _indicators; } }

    public string Name { get; set; }
    public OverheadIcon Icon { get; set; }
    public MapIndicator MapIndicator { get; set; }
    public bool HasNew { get; set; }

    public void Initialize(string name, OverheadIcon icon, MapIndicator map, bool hasNew) {
        Name = name;
        Icon = icon;
        MapIndicator = map;
        HasNew = hasNew;
    }

    void Start() {
        IndicatorManager.SetIndicatorsChanged();
    }

    void OnEnable(){
        _indicators.Add(this);
    }

    void OnDisable() {
        _indicators.Remove(this);
    }
    
}
