using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonoGame_Super_Pang.GameObjects;
using MonoGameLibrary.Graphics;

namespace MonoGame_Super_Pang.Config;

public class LevelConfig
{
    public List<PlatformConfig> Platforms{get; set;}

    public List<string> backgroundStr;
    // later: tilemap, time limit, etc.
}

public class PlatformConfig
{
    public Vector2 Position {get; set;}

    public PlatformType platformType{get; set;}

    public PlatformState platformState{get; set;}
}
