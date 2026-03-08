using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonoGame_Super_Pang.GameObjects;
using MonoGameLibrary.Graphics;
using System;

namespace MonoGame_Super_Pang.Config;

public class LevelConfig
{
    public List<BallSpawnConfig> Balls { get; set; }
    public Color BackgroundColor { get; set; }

    public List<PlatformConfig> Platforms{ get; set;}
    // later: tilemap, time limit, etc.
}

public class BallSpawnConfig
{
    public BallSize Size { get; set; }
    public float DirectionX { get; set; }

    public BallType BallType{get; set;}

    public Vector2 Position { get; set; }

}

public class PlatformConfig
{
    public Vector2 Position {get; set;}

    public PlatformType platformType{get; set;}

    public PlatformState platformState{get; set;}
}
