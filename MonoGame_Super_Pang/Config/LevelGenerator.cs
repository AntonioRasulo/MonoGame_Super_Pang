using System;
using System.Collections.Generic;
using System.Numerics;
using MonoGame_Super_Pang.GameObjects;
using MonoGameLibrary;

namespace MonoGame_Super_Pang.Config;

class LevelGenerator
{
    private const int MAX_ENEMY_NUMBER = 4;

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