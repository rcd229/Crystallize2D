using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

namespace CrystallizeData {
    public class StaticSerializedEquipmentGameData : StaticSerializedGameData {

        EquipmentGameData equipment = new EquipmentGameData();

        protected override void AddGameData() {
            GameData.Instance.Equipment.Classes.AddRange(equipment.Classes.Items);
            GameData.Instance.Equipment.Items.AddRange(equipment.Items.Items);
            //Debug.Log(GameData.Instance.Equipment.Items.Get("Broom"));
        }

        protected override void PrepareGameData() {
            var pole = AddClass("Pole");
            var greet = AddClass("Greet");

            AddItem("Broom", pole, EffectLibrary.DustCloud);
			AddItem("KittenItem", pole);
            AddItem("Greet", greet, false);
        }

        EquipmentItemGameData AddItem(string itemName, EquipmentClassGameData cls, bool required = true) {
            if (!GameData.Instance.Equipment.Items.ContainsKey(itemName)) {
                var i = new EquipmentItemGameData(itemName, cls.Name);
                equipment.Items.Add(i);
                if (required) {
                    AddRequiredResource(EquipmentItemGameData.ResourcePath, itemName);
                }
                return i;
            }
            Debug.LogError(itemName + " has already been added.");
            return null;
        }

        EquipmentItemGameData AddItem(string itemName, EquipmentClassGameData cls, string effect) {
            var i = AddItem(itemName, cls);
            i.Effect = effect;
            return i;
        }

        EquipmentClassGameData AddClass(string equipClass) {
            if (!GameData.Instance.Equipment.Classes.ContainsKey(equipClass)) {
                var c = new EquipmentClassGameData(equipClass);
                equipment.Classes.Add(c);
                AddRequiredResource(EquipmentClassGameData.ResourcePath, equipClass);
                return c;
            }
            Debug.LogError(equipClass + " has already been added.");
            return null;
        }

        void AddRequiredResource(string path, string name) {
            GameDataInitializer.AddRequiredResource(path + name);
        }

    }
}
