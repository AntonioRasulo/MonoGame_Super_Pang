using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonoGame_Super_Pang.GameObjects;

namespace MonoGame_Super_Pang.Config;

public class LevelConfig
{
    public List<BallSpawnConfig> Balls {get; set;}

    public List<PlatformConfig> Platforms{get; set;}

    public List<string> backgroundStr;

    public List<EnemyConfig> Enemies{get; set;}
    // later: tilemap, time limit, etc.

    public static readonly int STARTING_LEVEL = 0;

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

    public PlatformRotation Rotation {get; set;}

    public PlatformType platformType{get; set;}

    public PlatformState platformState{get; set;}
}

public class EnemyConfig
{
    public EnemyType EnemyType {get; set;}
    public Vector2 Position{get;set;}
}
