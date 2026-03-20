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
        new LevelConfig // Level 0
        {
            Balls = new List<BallSpawnConfig>
            {
                new BallSpawnConfig { Size = BallSize.LARGE, DirectionX = 1f, BallType = BallType.RED_ROUND }
            },
            Platforms = new List<PlatformConfig>
            {

            },
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds1/1",
                "images/backgrounds/clouds1/2",
                "images/backgrounds/clouds1/3",
                "images/backgrounds/clouds1/4"
            }
        },
        new LevelConfig // Level 1
        {
            Balls = new List<BallSpawnConfig>
            {
                new BallSpawnConfig { Size = BallSize.LARGE, DirectionX = -1f, BallType = BallType.RED_ROUND }
            },

            Platforms = new List<PlatformConfig>
            {
                new PlatformConfig
                {
                    Position = new Vector2(Core.GraphicsDevice.PresentationParameters.Bounds.Width * 0.25f, Core.GraphicsDevice.PresentationParameters.Bounds.Height * 0.60f),
                    platformType = PlatformType.HORIZONTAL_GRAY
                }
            },
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds2/1",
                "images/backgrounds/clouds2/2",
                "images/backgrounds/clouds2/3",
                "images/backgrounds/clouds2/4"
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
                    Size = BallSize.LARGE,
                    DirectionX = 1f,
                    BallType = BallType.GREEN_SQUARED,
                    Position = new Vector2(Core.GraphicsDevice.PresentationParameters.Bounds.Width * 0.75f, Core.GraphicsDevice.PresentationParameters.Bounds.Height * 0.5f - 30f)
                }
            },
            Platforms = new List<PlatformConfig>
            {

            },
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds3/1",
                "images/backgrounds/clouds3/2",
                "images/backgrounds/clouds3/3",
                "images/backgrounds/clouds3/4"
            }
        },
        new LevelConfig // Level 3
        {
            Balls = new List<BallSpawnConfig>
            {
                new BallSpawnConfig
                {
                    Size = BallSize.MEDIUM,
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
                    Position = new Vector2(Core.GraphicsDevice.PresentationParameters.Bounds.Width * 0.25f, Core.GraphicsDevice.PresentationParameters.Bounds.Height * 0.65f),
                    platformType = PlatformType.HORIZONTAL_GRAY
                }
            },
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds4/1",
                "images/backgrounds/clouds4/2",
                "images/backgrounds/clouds4/3",
                "images/backgrounds/clouds4/4"
            }
        },
        new LevelConfig // Level 4
        {
            Balls = new List<BallSpawnConfig>
            {
                new BallSpawnConfig
                {
                    Size = BallSize.MEDIUM,
                    DirectionX = -1f,
                    BallType = BallType.RED_ROUND,
                    Position = new Vector2(Core.GraphicsDevice.PresentationParameters.Bounds.Width * 0.25f, Core.GraphicsDevice.PresentationParameters.Bounds.Height * 0.5f - 30f)
                },
                new BallSpawnConfig 
                { 
                    Size = BallSize.MEDIUM,
                    DirectionX = 1f,
                    BallType = BallType.RED_ROUND,
                    Position = new Vector2(Core.GraphicsDevice.PresentationParameters.Bounds.Width * 0.75f, Core.GraphicsDevice.PresentationParameters.Bounds.Height * 0.5f - 30f)
                }
            },

            Platforms = new List<PlatformConfig>
            {
                new PlatformConfig
                {
                    Position = new Vector2(Core.GraphicsDevice.PresentationParameters.Bounds.Width * 0.25f, Core.GraphicsDevice.PresentationParameters.Bounds.Height * 0.65f),
                    platformType = PlatformType.BREAKABLE_LARGE_HORIZONTAL_BLUE,
                    platformState = PlatformState.Stage1
                }
            },
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds5/1",
                "images/backgrounds/clouds5/2",
                "images/backgrounds/clouds5/3",
                "images/backgrounds/clouds5/4",
                "images/backgrounds/clouds5/5",
            }
        }
    };
}