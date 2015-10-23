using UnityEngine;
using System.Collections;

public class NavigationGameData {

    public SerializableDictionary<int, AreaGameData> Areas { get; set; }

    public NavigationGameData() {
        Areas = new SerializableDictionary<int, AreaGameData>();
    }

    public void AddNewArea() {
        int i = 0;
        while (Areas.Get(i) != null) {
            i++;
        }

        Areas.Add(new AreaGameData(i));
    }

}
