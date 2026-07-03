using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonoGame_Super_Pang.GameObjects;
using MonoGameLibrary;

namespace MonoGame_Super_Pang.Config;

public static class LevelRegistry
{
    private static readonly float screenWidth = Core.GraphicsDevice.PresentationParameters.Bounds.Width;
    private static readonly float screenHeight = Core.GraphicsDevice.PresentationParameters.Bounds.Height;

    public static List<LevelConfig> AllLevels = new List<LevelConfig>
    {
        new LevelConfig // Level 0
        {
            Balls = new List<BallSpawnConfig>
            {
                new BallSpawnConfig { Size = BallSize.LARGE, DirectionX = 1f, BallType = BallType.RED_ROUND, Position = new Vector2(screenWidth * 0.75f, screenHeight * 0.5f) },
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
            },
            Enemies = new List<EnemyConfig>
            {
            }
        },
        new LevelConfig // Level 1
        {
            Balls = new List<BallSpawnConfig>
            {
                new BallSpawnConfig { Size = BallSize.LARGE, DirectionX = 1f, BallType = BallType.GREEN_ROUND, Position = new Vector2(screenWidth * 0.75f, screenHeight * 0.5f) },
                new BallSpawnConfig { Size = BallSize.LARGE, DirectionX = -1f, BallType = BallType.GREEN_ROUND, Position = new Vector2(screenWidth * 0.25f, screenHeight * 0.5f) }
            },
            Platforms = new List<PlatformConfig>
            {
                new PlatformConfig
                {
                    Position = new Vector2(screenWidth * 0.5f, screenHeight * 0.5f),
                    Rotation = PlatformRotation.HORIZONTAL,
                    platformType = PlatformType.GRAY
                }
            },
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds2/1",
                "images/backgrounds/clouds2/2",
                "images/backgrounds/clouds2/3",
                "images/backgrounds/clouds2/4"
            },
            Enemies = new List<EnemyConfig>
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
                    BallType = BallType.LBLUE_SQUARED,
                    Position = new Vector2(screenWidth * 0.25f, screenHeight * 0.5f - 30f)
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
            },
            Enemies = new List<EnemyConfig>
            {

            }
        },
        new LevelConfig // Level 3
        {
            Balls = new List<BallSpawnConfig>
            {
                new BallSpawnConfig { Size = BallSize.LARGE, DirectionX = 1f, BallType = BallType.BLUE_ROUND, Position = new Vector2(screenWidth * 0.5f, screenHeight * 0.5f) },
            },

            Platforms = new List<PlatformConfig>
            {
            },
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds4/1",
                "images/backgrounds/clouds4/2",
                "images/backgrounds/clouds4/3",
                "images/backgrounds/clouds4/4"
            },
            Enemies = new List<EnemyConfig>
            {
                new EnemyConfig
                {
                    EnemyType = EnemyType.MINI_BAT,
                    Position = new Vector2(screenWidth * 0.25f, screenHeight * 0.60f)
                },
                new EnemyConfig
                {
                    EnemyType = EnemyType.MINI_BAT,
                    Position = new Vector2(screenWidth * 0.75f, screenHeight * 0.60f)
                }
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
                    Position = new Vector2(screenWidth * 0.25f, screenHeight * 0.5f - 30f)
                },
                new BallSpawnConfig 
                { 
                    Size = BallSize.MEDIUM,
                    DirectionX = 1f,
                    BallType = BallType.RED_ROUND,
                    Position = new Vector2(screenWidth * 0.75f, screenHeight * 0.5f - 30f)
                }
            },

            Platforms = new List<PlatformConfig>
            {
                new PlatformConfig
                {
                    Position = new Vector2(screenWidth * 0.25f, screenHeight * 0.5f),
                    Rotation = PlatformRotation.HORIZONTAL,
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
            },
            Enemies = new List<EnemyConfig>
            {

            }
        },
        new LevelConfig // Level 5
        {
            Balls = new List<BallSpawnConfig>
            {
                new BallSpawnConfig
                {
                    Size = BallSize.MEDIUM,
                    DirectionX = -1f,
                    BallType = BallType.RED_ROUND,
                    Position = new Vector2(screenWidth * 0.25f, screenHeight * 0.5f - 30f)
                },
                new BallSpawnConfig 
                {
                    Size = BallSize.MEDIUM,
                    DirectionX = 1f,
                    BallType = BallType.RED_ROUND,
                    Position = new Vector2(screenWidth * 0.75f, screenHeight * 0.5f - 30f)
                }
            },

            Platforms = new List<PlatformConfig>
            {

            },
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds6/1",
                "images/backgrounds/clouds6/2",
                "images/backgrounds/clouds6/3",
                "images/backgrounds/clouds6/4",
                "images/backgrounds/clouds6/5",
                "images/backgrounds/clouds6/6"
            },
            Enemies = new List<EnemyConfig>
            {
                new EnemyConfig
                {
                    EnemyType = EnemyType.BIG_BAT,
                    Position = new Vector2(screenWidth * 0.25f, screenHeight * 0.60f)
                }
            }
        },
        new LevelConfig // Level 6
        {
            Balls = new List<BallSpawnConfig>
            {
                new BallSpawnConfig
                {
                    Size = BallSize.MEDIUM,
                    DirectionX = -1f,
                    BallType = BallType.GREEN_SQUARED,
                    Position = new Vector2(screenWidth * 0.25f, screenHeight * 0.5f - 30f)
                },
                new BallSpawnConfig 
                {
                    Size = BallSize.MEDIUM,
                    DirectionX = 1f,
                    BallType = BallType.GREEN_SQUARED,
                    Position = new Vector2(screenWidth * 0.75f, screenHeight * 0.5f - 30f)
                }
            },

            Platforms = new List<PlatformConfig>
            {

            },
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds7/1",
                "images/backgrounds/clouds7/2",
                "images/backgrounds/clouds7/3",
                "images/backgrounds/clouds7/4"
            },
            Enemies = new List<EnemyConfig>
            {
                new EnemyConfig
                {
                    EnemyType = EnemyType.BIG_BAT,
                    Position = new Vector2(screenWidth * 0.25f, screenHeight * 0.60f)
                }
            }
        },
        new LevelConfig // Level 7
        {
            Balls = new List<BallSpawnConfig>
            {
                new BallSpawnConfig
                {
                    Size = BallSize.MEDIUM,
                    DirectionX = -1f,
                    BallType = BallType.BLUE_ROUND,
                    Position = new Vector2(screenWidth * 0.25f, screenHeight * 0.5f - 30f)
                },
                new BallSpawnConfig
                {
                    Size = BallSize.MEDIUM,
                    DirectionX = -1f,
                    BallType = BallType.RED_ROUND,
                    Position = new Vector2(screenWidth * 0.5f, screenHeight * 0.5f - 30f)
                },
                new BallSpawnConfig 
                {
                    Size = BallSize.MEDIUM,
                    DirectionX = 1f,
                    BallType = BallType.GREEN_ROUND,
                    Position = new Vector2(screenWidth * 0.75f, screenHeight * 0.5f - 30f)
                }
            },
            Platforms = new List<PlatformConfig>
            {
                new PlatformConfig
                {
                    Position = new Vector2(screenWidth * 0.25f, screenHeight * 0.5f),
                    Rotation = PlatformRotation.VERTICAL,
                    platformType = PlatformType.BROWN
                },
                new PlatformConfig
                {
                    Position = new Vector2(screenWidth * 0.75f, screenHeight * 0.5f),
                    Rotation = PlatformRotation.VERTICAL,
                    platformType = PlatformType.BROWN
                }
            },
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds8/1",
                "images/backgrounds/clouds8/2",
                "images/backgrounds/clouds8/3",
                "images/backgrounds/clouds8/4",
                "images/backgrounds/clouds8/5",
                "images/backgrounds/clouds8/6"
            },
            Enemies = new List<EnemyConfig>
            {
                
            }
        },
        new LevelConfig // Level 8
        {
            Balls = new List<BallSpawnConfig>
            {
                new BallSpawnConfig { Size = BallSize.LARGE, DirectionX = 1f, BallType = BallType.RED_ROUND, Position = new Vector2(screenWidth * 0.75f, screenHeight * 0.5f) },
                new BallSpawnConfig { Size = BallSize.LARGE, DirectionX = -1f, BallType = BallType.DBLUE_SQUARED, Position = new Vector2(screenWidth * 0.25f, screenHeight * 0.5f) }
            },
            Platforms = new List<PlatformConfig>
            {

            },
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds9/1",
                "images/backgrounds/clouds9/2",
                "images/backgrounds/clouds9/3",
                "images/backgrounds/clouds9/4",
                "images/backgrounds/clouds9/5"
            },
            Enemies = new List<EnemyConfig>
            {
            }
        },
        new LevelConfig // Level 9
        {
            Balls = new List<BallSpawnConfig>
            {
                new BallSpawnConfig { Size = BallSize.LARGE, DirectionX = 1f, BallType = BallType.RED_ROUND, Position = new Vector2(screenWidth * 0.75f, screenHeight * 0.5f) },
                new BallSpawnConfig { Size = BallSize.LARGE, DirectionX = -1f, BallType = BallType.GREEN_ROUND, Position = new Vector2(screenWidth * 0.25f, screenHeight * 0.5f) },
                new BallSpawnConfig { Size = BallSize.LARGE, DirectionX = 1f, BallType = BallType.BLUE_ROUND, Position = new Vector2(screenWidth * 0.5f, screenHeight * 0.5f) }
            },
            Platforms = new List<PlatformConfig>
            {

            },
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds10/1",
                "images/backgrounds/clouds10/2",
                "images/backgrounds/clouds10/3",
                "images/backgrounds/clouds10/4"
            },
            Enemies = new List<EnemyConfig>
            {
            }
        },
        new LevelConfig // Level 10
        {
            Balls = new List<BallSpawnConfig>
            {
                new BallSpawnConfig { Size = BallSize.LARGE, DirectionX = 1f, BallType = BallType.RED_ROUND, Position = new Vector2(screenWidth * 0.75f, screenHeight * 0.25f) },
                new BallSpawnConfig { Size = BallSize.LARGE, DirectionX = -1f, BallType = BallType.GREEN_ROUND, Position = new Vector2(screenWidth * 0.25f, screenHeight * 0.25f) },
            },
            Platforms = new List<PlatformConfig>
            {
                new PlatformConfig
                {
                    Position = new Vector2(screenWidth * 0.25f, screenHeight * 0.5f),
                    Rotation = PlatformRotation.HORIZONTAL,
                    platformType = PlatformType.BREAKABLE_LARGE_HORIZONTAL_BLUE,
                    platformState = PlatformState.Stage1
                },
                new PlatformConfig
                {
                    Position = new Vector2(screenWidth * 0.75f, screenHeight * 0.5f),
                    Rotation = PlatformRotation.HORIZONTAL,
                    platformType = PlatformType.BREAKABLE_LARGE_HORIZONTAL_BLUE,
                    platformState = PlatformState.Stage1
                }
            },
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds11/1",
                "images/backgrounds/clouds11/2",
                "images/backgrounds/clouds11/3",
                "images/backgrounds/clouds11/4"
            },
            Enemies = new List<EnemyConfig>
            {
            }
        },
        new LevelConfig // Level 11
        {
            Balls = new List<BallSpawnConfig>
            {
            },

            Platforms = new List<PlatformConfig>
            {

            },
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds7/1",
                "images/backgrounds/clouds7/2",
                "images/backgrounds/clouds7/3",
                "images/backgrounds/clouds7/4"
            },
            Enemies = new List<EnemyConfig>
            {
                new EnemyConfig
                {
                    EnemyType = EnemyType.FLYING_DEMON,
                    Position = new Vector2(screenWidth * 0.5f, screenHeight * 0.25f)
                }
            }
        }
    };
}