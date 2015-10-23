using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PlayerAvatarSerializer : INetworkSerializer<PlayerAvatarData, byte[]> {

    public static readonly PlayerAvatarSerializer Instance = new PlayerAvatarSerializer();

    public byte[] Serialize(PlayerAvatarData data) {
        var apprBytes = data.Appearance.ToByteArray();
        var nameBytes = StringNetworkSerializer.Instance.Serialize(data.Name);
        var bytes = new byte[apprBytes.Length + nameBytes.Length];
        apprBytes.CopyTo(bytes, 0);
        nameBytes.CopyTo(bytes, apprBytes.Length);
        return bytes;
    }

    public PlayerAvatarData Deserialize(byte[] data) {
        var avaData = new PlayerAvatarData();
        avaData.Appearance.FromByteArray(data);
        var nameBytes = new byte[data.Length - AppearancePlayerData.ByteCount];
        Array.Copy(data, AppearancePlayerData.ByteCount, nameBytes, 0, nameBytes.Length);
        avaData.Name = StringNetworkSerializer.Instance.Deserialize(nameBytes);
        return avaData;
    }
}
