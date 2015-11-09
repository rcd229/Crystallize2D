using UnityEngine;
using System;
using System.Collections.Generic;
using Util;
using System.Linq;

public class GameLevel2D {
    public static GameLevel2D DefaultLevel = new GameLevel2D("Default", new Guid("5eb60c05-72de-4b0a-a50a-611c380008cc"));

    public static GameLevel2D GetGameLevel(string name) {
        return new GameLevel2D(name, new Guid("6e66783988a746c8b403a4b11220191c"));
    }

    public Dictionary<SpriteLayer, TileMap2D> layers;
    public string levelname;
    public Guid guid;

    public GameLevel2D(string levelname, Guid g) {
        this.levelname = levelname;
        layers = new Dictionary<SpriteLayer, TileMap2D>();
        layers[SpriteLayer.Path] = new TileMap2D(SpriteLayer.Path, levelname);
        layers[SpriteLayer.Building] = new TileMap2D(SpriteLayer.Building, levelname);
        layers[SpriteLayer.Door] = new TileMap2D(SpriteLayer.Door, levelname);
        layers[SpriteLayer.Environment] = new TileMap2D(SpriteLayer.Environment, levelname);
        guid = g;
    }

    public GameLevel2D(string levelname) : this(levelname, Guid.NewGuid()) {

    }
}
