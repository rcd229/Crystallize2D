using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public static class ByteExtensions {
    public static byte[] ConvertHexToByteArray(this string hexString) {
        var bytes = new byte[hexString.Length / 2];
        for (int i = 0; i < bytes.Length; i++) {
            bytes[i] = Convert.ToByte(hexString[i * 2].ToString() + hexString[i * 2 + 1].ToString(), 16);
        }
        return bytes;
    }

    public static string ConvertToHexString(this byte[] bytes) {
        return BitConverter.ToString(bytes).Replace("-", "");
    }
}
