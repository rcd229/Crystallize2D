using UnityEngine;
using System.Collections;
using Util.Serialization;

public class PlayerDataNetworkSerializer : INetworkSerializer<PlayerData, byte[]> {

    public byte[] Serialize(PlayerData data) {
        return new StringNetworkSerializer().Serialize(Serializer.SaveToXmlString<PlayerData>(data));
    }

    public PlayerData Deserialize(byte[] data) {
        string playerData;
        if (data.Length <= 1) {
            playerData = "";
        } else {
            playerData = new StringNetworkSerializer().Deserialize(data);
        }
        if (playerData.IsEmptyOrNull() || playerData[0] == '.') {
            return null;
        } else {
            return Serializer.LoadFromXmlString<PlayerData>(playerData);
        }
    }

}
