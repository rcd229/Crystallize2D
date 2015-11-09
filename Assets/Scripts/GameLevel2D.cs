using UnityEngine;
using System;
using System.Collections.Generic;
using Util;
using System.Linq;

public class GameLevel2D
{
    public static GameLevel2D DefaultLevel = new GameLevel2D("Default", new Guid("5eb60c05-72de-4b0a-a50a-611c380008cc"));
    public static GameLevel2D TestLevel = new GameLevel2D("TestLevel", new Guid("478523f7-d9b1-4e7a-92fa-d4c9e41400a1"));
    public Dictionary<SpriteLayer,TileMap2D> layers;
    public string levelname;
    public Guid guid;
    
    public static GameLevel2D[] GetAllLevels() {
        return new GameLevel2D[] { DefaultLevel, TestLevel };
    }

    public static GameLevel2D GetGameLevel(Guid g)
    {
        return (from l in GetAllLevels() where l.guid == g select l).FirstOrDefault();  
    }

    public static GameLevel2D GetGameLevel(string g)
    {
        return (from l in GetAllLevels() where l.levelname == g select l).FirstOrDefault();
    }

    public GameLevel2D (string levelname, Guid g)
	{
        this.levelname = levelname;
        layers = new Dictionary<SpriteLayer, TileMap2D>();
        layers[SpriteLayer.Path] = new TileMap2D(SpriteLayer.Path, levelname);
        layers[SpriteLayer.Building] = new TileMap2D(SpriteLayer.Building, levelname);
        layers[SpriteLayer.Door] = new TileMap2D(SpriteLayer.Door, levelname);
        layers[SpriteLayer.Environment] = new TileMap2D(SpriteLayer.Environment, levelname);
        guid = g;   
	}

    public GameLevel2D(string levelname):this(levelname, Guid.NewGuid())
    {

    }
}
