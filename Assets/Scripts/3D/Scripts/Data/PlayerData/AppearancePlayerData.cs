using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum AppearanceGender {
    Male = 0,
    Female = 1
}

public enum AppearanceEyeColor {
    Black = 0,
    Blue = 1,
    Brown = 2,
    Green = 3,
    Yellow = 4
}

public enum AppearanceSkinColor {
    Light = 0,
    Medium = 1,
    Dark = 2
}

public enum AppearanceHairColor {
    Black = 0,
    Blond = 1,
    Blue = 2,
    Brown = 3,
    Gray = 4,
    Green = 5,
    Orange = 6,
    Pink = 7,
    Red = 8
}

public enum AppearanceShirt01Material {
    Totoro = 0,
    Stripe = 1,
    Black = 2,
    White = 3
}

public enum AppearanceShirt02Material {
    Black = 0,
    Gray = 1,
    Striped = 2,
    White = 3
}

public enum AppearanceLegs01Material {
    Blue = 0,
    Khaki = 1,
    Red = 2,
    Black = 3
}

public enum AppearanceLegs02Material {
    Gray = 0,
    Jeans = 1,
    Khaki = 2,
    Navy = 3
}

public enum BodyPartType {
    Chest = 0,
    Legs = 1
}

public class AppearancePlayerData {

    public static int ByteCount { get; private set; }

    static AppearancePlayerData() {
        ByteCount = new AppearancePlayerData().ToByteArray().Length;
    }

    public static AppearancePlayerData GetRandom() {
        var a = new AppearancePlayerData();
        a.Gender = RandomValueFromEnum<AppearanceGender>();
        a.EyeColor = RandomValueFromEnum<AppearanceEyeColor>();
        a.SkinColor = RandomValueFromEnum<AppearanceSkinColor>();
        a.HairType = RandomFromValues(MaleHairMesh.GetResourcePaths());
        a.HairColor = RandomValueFromEnum<AppearanceHairColor>();
        a.TopType = RandomFromValues(MaleShirtMesh.GetRecourcePaths());
        a.TopMaterial = RandomValueFromEnum<AppearanceShirt01Material>();
        a.BottomType = RandomFromValues(MaleShortMesh.GetRecourcePaths());
        a.BottomMaterial = RandomValueFromEnum<AppearanceLegs01Material>();
        return a;
    }

    static int RandomValueFromEnum<T>() {
        var values = Enum.GetValues(typeof(T));
        var index = UnityEngine.Random.Range(0, values.GetLength(0));
        return (int)values.GetValue(index);
    }

    static int RandomFromValues<T>(IEnumerable<T> collection) {
        return UnityEngine.Random.Range(0, collection.Count());
    }

    public int Gender { get; set; }
    public int EyeColor { get; set; }
    public int SkinColor { get; set; }
    public int HairType { get; set; }
    public int HairColor { get; set; }
    public int TopType { get; set; }
    public int TopMaterial { get; set; }
    public int BottomType { get; set; }
    public int BottomMaterial { get; set; }

    public AppearancePlayerData() { }

    public AppearancePlayerData(AppearanceGender gender, 
        AppearanceEyeColor eyeColor = AppearanceEyeColor.Black, AppearanceSkinColor skinColor = AppearanceSkinColor.Medium, 
        int hairType = 0, AppearanceHairColor hairColor = AppearanceHairColor.Black,
        int topType = 0, int topMaterial = 0,
        int bottomType = 0, int bottomMaterial = 0) {
            this.Gender = (int)gender;
            this.EyeColor = (int)eyeColor;
            this.SkinColor = (int)skinColor;
            this.HairType = hairType;
            this.HairColor = (int)hairColor;
            this.TopType = topType;
            this.TopMaterial = topMaterial;
            this.BottomType = bottomType;
            this.BottomMaterial = bottomMaterial;
    }

    public AppearanceGameData GetResourceData() {
        var app = new AppearanceGameData();
        if (Gender == 0) {
            app.Gender = AppearanceGenderData.Male;
            app.Body = new BodyDataRef(MaleBodyMesh.Instance.MeshList.GetSafely(0), SkinColor);
            app.Eye = new EyeDataRef(MaleEyeMesh.Instance.MeshList.GetSafely(0), EyeColor);
            app.Hair = new HairDataRef(MaleHairMesh.Instance.MeshList.GetSafely(HairType), HairColor);
            app.Shirt = new ShirtDataRef(MaleShirtMesh.Instance.MeshList.GetSafely(TopType), TopMaterial);
            app.Short = new ShortDataRef(MaleShortMesh.Instance.MeshList.GetSafely(BottomType), BottomMaterial);
        } else {
            app.Gender = AppearanceGenderData.Female;
            app.Body = new BodyDataRef(FemaleBodyMesh.Instance.MeshList.GetSafely(0), SkinColor);
            app.Eye = new EyeDataRef(FemaleEyeMesh.Instance.MeshList.GetSafely(0), EyeColor);
            app.Hair = new HairDataRef(FemaleHairMesh.Instance.MeshList.GetSafely(HairType), HairColor);
            app.Shirt = new ShirtDataRef(FemaleShirtMesh.Instance.MeshList.GetSafely(TopType), TopMaterial);
            app.Short = new ShortDataRef(FemaleShortMesh.Instance.MeshList.GetSafely(BottomType), BottomMaterial);
        }
        return app;
    }

    public void GetPartParameters(BodyPartType part, out int mesh, out int material) {
        if (part == BodyPartType.Chest) {
            mesh = TopType;
            material = TopMaterial;
        } else if (part == BodyPartType.Legs) {
            mesh = BottomType;
            material = TopType;
        } else {
            mesh = 0;
            material = 0;
        }
    }

    public byte[] ToByteArray() {
        var ints = new int[] { Gender, EyeColor, SkinColor, HairType, HairColor, TopType, TopMaterial, BottomType, BottomMaterial };
        return ints.Select(i => (byte)i).ToArray();
    }

    public void FromByteArray(byte[] bytes) {
        Gender = bytes[0];
        EyeColor = bytes[1];
        SkinColor = bytes[2];
        HairType = bytes[3];
        HairColor = bytes[4];
        TopType = bytes[5];
        TopMaterial = bytes[6];
        BottomType = bytes[7];
        BottomMaterial = bytes[8];
    }

}