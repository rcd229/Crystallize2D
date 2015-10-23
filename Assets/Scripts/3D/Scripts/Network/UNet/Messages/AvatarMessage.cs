using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections; 
using System.Collections.Generic;
using Util.Serialization;

public class AvatarMessage : MessageBase {

    public string name = "";
    public AppearancePlayerData appearance;

    public AvatarMessage() { }

    public AvatarMessage(string name, AppearancePlayerData appearance) {
        this.name = name;
        this.appearance = appearance;
    }

    public override void Serialize(NetworkWriter writer) {
        writer.Write(name);
        writer.Write(Serializer.SaveToXmlString<AppearancePlayerData>(appearance));
    }

    public override void Deserialize(NetworkReader reader) {
        name = reader.ReadString();
        appearance = Serializer.LoadFromXmlString<AppearancePlayerData>(reader.ReadString());
    }

}
