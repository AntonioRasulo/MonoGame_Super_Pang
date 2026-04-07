using System;
using System.Collections.Generic;
using System.Numerics;
using MonoGame_Super_Pang.GameObjects;
using MonoGameLibrary;

namespace MonoGame_Super_Pang.Config;

class LevelGenerator
{
    private const int MAX_ENEMY_NUMBER = 4;
    private const int MAX_BALLS_NUMBER = 4;

    public static List<Enemy> generateEnemies()
    {
        List<Enemy> enemies = new List<Enemy>();

        int enemy_number = Random.Shared.Next(0, MAX_ENEMY_NUMBER);

        for(int i = 0; i<enemy_number; i++)
        {
            EnemyType enemyType = (EnemyType)Random.Shared.Next(0, (int)EnemyType.LAST_ENEMY);

            switch (enemyType)
            {
                case EnemyType.MINI_BAT:
                    enemies.Add(new MiniBat(generatePosition()));
                    break;
                case EnemyType.BIG_BAT:
                    enemies.Add(new BigBat(generatePosition()));
                    break;
            }
        }

        return enemies;
    }

    public static List<Ball> generateBalls()
    {
        List<Ball> balls = new List<Ball>();

        int balls_number = Random.Shared.Next(0, MAX_BALLS_NUMBER);

        for(int i = 0; i<balls_number; i++)
        {
            BallType ballType = (BallType)Random.Shared.Next(0, (int)BallType.LAST_BALL_TYPE);
            BallSize ballSize = (BallSize)Random.Shared.Next(0, (int)BallSize.LAST_BALL_SIZE);
            int direction = 0;
            while (direction == 0)
            {
                direction = Random.Shared.Next(-1, 2);
            }

            switch (ballType)
            {
                case BallType.GREEN_ROUND:
                case BallType.RED_ROUND:
                case BallType.BLUE_ROUND:
                    balls.Add(new BouncingBall(ballSize, direction, ballType, generatePosition()));
                break;
                case BallType.GREEN_SQUARED:
                    balls.Add(new ReflectiveBall(ballSize, direction, ballType, generatePosition()));
                break;
            }
        }

        return balls;
    }

    private static Vector2 generatePosition()
    {
        float positionX = Random.Shared.Next(
                0,
                Core.GraphicsDevice.PresentationParameters.BackBufferWidth
            );

        float positionY = Random.Shared.Next(
                0,
                (int)(Core.GraphicsDevice.PresentationParameters.BackBufferHeight * 0.75f)
            );

        return new Vector2(positionX, positionY);
    }

}