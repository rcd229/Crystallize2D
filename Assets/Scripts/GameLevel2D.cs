using UnityEngine;
using System;
using System.Collections.Generic;
using Util;

public class GameLevel2D
{
    public static GameLevel2D DefaultLevel = new GameLevel2D("Default");
    public static GameLevel2D TestLevel = new GameLevel2D("TestLevel");
    public Dictionary<SpriteLayer,TileMap2D> layers;
    public string levelname;
    
    public static GameLevel2D[] GetAllLevels() {
        return new GameLevel2D[] { DefaultLevel, TestLevel };
    }

	public GameLevel2D (string levelname)
	{
        this.levelname = levelname;
        layers = new Dictionary<SpriteLayer, TileMap2D>();
        layers[SpriteLayer.Path] = new TileMap2D(SpriteLayer.Path, levelname);
        layers[SpriteLayer.Building] = new TileMap2D(SpriteLayer.Building, levelname);
        layers[SpriteLayer.Door] = new TileMap2D(SpriteLayer.Door, levelname);
        layers[SpriteLayer.Environment] = new TileMap2D(SpriteLayer.Environment, levelname);     
	}
}
