using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonoGame_Super_Pang.GameObjects;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace MonoGame_Super_Pang.Config;

public static class LevelRegistry
{
    public static List<LevelConfig> AllLevels = new List<LevelConfig>
    {
        new LevelConfig // Level 1
        {
            Balls = new List<BallSpawnConfig>
            {
                new BallSpawnConfig { Size = BallSize.LARGE, DirectionX = 1f, BallType = BallType.RED_ROUND }
            },

            Platforms = new List<PlatformConfig>
            {
                
            }
        },
        new LevelConfig // Level 2
        {
            Balls = new List<BallSpawnConfig>
            {
                new BallSpawnConfig
                {
                    Size = BallSize.LARGE,
                    DirectionX = -1f,
                    BallType = BallType.GREEN_SQUARED,
                    Position = new Vector2(Core.GraphicsDevice.PresentationParameters.Bounds.Width * 0.25f, Core.GraphicsDevice.PresentationParameters.Bounds.Height * 0.5f - 30f)
                },
                new BallSpawnConfig 
                { 
                    Size = BallSize.MEDIUM,
                    DirectionX = 1f,
                    BallType = BallType.GREEN_SQUARED,
                    Position = new Vector2(Core.GraphicsDevice.PresentationParameters.Bounds.Width * 0.75f, Core.GraphicsDevice.PresentationParameters.Bounds.Height * 0.5f - 30f)
                }
            },

            Platforms = new List<PlatformConfig>
            {
                new PlatformConfig
                {
                    Position = new Vector2(Core.GraphicsDevice.PresentationParameters.Bounds.Width * 0.25f, Core.GraphicsDevice.PresentationParameters.Bounds.Height * 0.5f),
                    platformType = PlatformType.HORIZONTAL_GREEN
                }
            }
        }
    };
}