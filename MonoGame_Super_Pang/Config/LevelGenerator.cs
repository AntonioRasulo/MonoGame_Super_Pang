using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame_Super_Pang.Backgrounds;
using MonoGame_Super_Pang.GameObjects;
using MonoGame_Super_Pang.Utility;
using MonoGameLibrary;

namespace MonoGame_Super_Pang.Config;

class LevelGenerator
{
    private const int MAX_ENEMY_NUMBER = 4;
    private const int MAX_BALLS_NUMBER = 4;
    private const int MAX_PLATFORM_NUMBER = 2;

    private static int SCREEN_WIDTH = Core.GraphicsDevice.PresentationParameters.BackBufferWidth;
    private static int SCREEN_HEIGHT = Core.GraphicsDevice.PresentationParameters.BackBufferHeight;

    private static Random xRandom = new Random();
    private static Random yRandom = new Random();

    public static List<Enemy> generateEnemies()
    {
        List<Enemy> enemies = new List<Enemy>();

        Random randomGen = new Random();

        int enemy_number = randomGen.Next(0, MAX_ENEMY_NUMBER);

        for(int i = 0; i<enemy_number; i++)
        {
            EnemyType enemyType = (EnemyType)Random.Shared.Next((int)EnemyType.LAST_ENEMY);

            Vector2 position = generatePosition(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT*0.75f);

            switch (enemyType)
            {
                case EnemyType.MINI_BAT:
                    enemies.Add(new MiniBat(position));
                    break;
                case EnemyType.BIG_BAT:
                    enemies.Add(new BigBat(position));
                    break;
            }
        }

        return enemies;
    }

    public static List<Ball> generateBalls()
    {
        List<Ball> balls = new List<Ball>();

        int balls_number = Random.Shared.Next(1, MAX_BALLS_NUMBER);

        for(int i = 0; i<balls_number; i++)
        {
            BallType ballType = (BallType)Random.Shared.Next((int)BallType.LAST_BALL_TYPE);
            BallSize ballSize = (BallSize)Random.Shared.Next((int)BallSize.LAST_BALL_SIZE);
            int direction = 0;
            while (direction == 0)
            {
                direction = Random.Shared.Next(-1, 2);
            }

            Vector2 position = generatePosition(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT*0.75f);

            switch (ballType)
            {
                case BallType.GREEN_ROUND:
                case BallType.RED_ROUND:
                case BallType.BLUE_ROUND:
                    balls.Add(new BouncingBall(ballSize, direction, ballType, position));
                break;
                case BallType.GREEN_SQUARED:
                    balls.Add(new ReflectiveBall(ballSize, direction, ballType, position));
                break;
            }
        }

        return balls;
    }

    public static Background generateBackground()
    {
        int bg_num = Random.Shared.Next(BackgroundRegistry.backgrounds.Count);

        return BackgroundRegistry.backgrounds[bg_num];
    }

    public static List<Platform> generatePlatforms(List<Ball> balls)
    {
        List<Platform> platforms = new List<Platform>();

        int platform_num = Random.Shared.Next(1, MAX_PLATFORM_NUMBER);

        for(int i = 0; i< platform_num; i++)
        {
            PlatformType platformType = (PlatformType)Random.Shared.Next((int)PlatformType.LAST_PLATFORM_TYPE);
            platforms.Add(GeneratePlatform(balls, platformType));
        }

        return platforms;
    }

    private static Vector2 generatePosition(float minX, float minY, float maxX, float maxY)
    {
        float positionX = xRandom.Next(
                (int)minX,
                (int)maxX
            );

        float positionY = yRandom.Next(
                (int)minY,
                (int)maxY
            );

        return new Vector2(positionX, positionY);
    }

    private static Platform GeneratePlatform(List<Ball> balls, PlatformType platformType)
    {

        Vector2 position = new Vector2();

        Platform platform = null;

        while(platform == null)
        {
            position = generatePosition(SCREEN_WIDTH * 0.2f, SCREEN_HEIGHT*0.25f, SCREEN_WIDTH * 0.8f, SCREEN_HEIGHT * 0.5f);

            switch (platformType)
            {
                case PlatformType.HORIZONTAL_GRAY:
                    platform = new UnbreakablePlatform(position, platformType);
                break;
                case PlatformType.BREAKABLE_LARGE_HORIZONTAL_BLUE:
                    platform = new BreakablePlatform(position, platformType);
                break;
            }

            Rectangle platformBounds = platform.getBounds();

            for(int indexBall = 0; indexBall<balls.Count; indexBall++)
            {
                Circle ballBounds = balls[indexBall].GetBounds();
                if(CollisionChecker.areIntersecting(ballBounds, platformBounds))
                {
                    platform = null;
                    break;
                }
            }
        }

        return platform;

    }
}