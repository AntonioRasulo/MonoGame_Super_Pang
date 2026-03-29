using System;
using System.Collections.Generic;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace MonoGame_Super_Pang.GameObjects;

public class PowerUpHandler
{
    private List<PowerUp> _powerUps;

    private Random _powerUpRand;

    private readonly Vector2 SCALE = new(4.0f, 4.0f);
    private readonly Vector2 INV_SCALE = new(1.5f, 1.5f);

    public static Sprite _livesSprite;
    public static Sprite _freezeSprite;
    public static Sprite _invincibilitySprite; 
    public static Sprite _bombSprite;

    private SoundEffect _collectPowerUp;

    private const int LIVES_PROB = 5;
    private const int FREEZE_PROB = 10;
    private const int INVINCIBILITY_PROB = 15;
    private const int BOMB_PROB = 20;

    public PowerUpHandler()
    {
        LoadContent();
        _powerUps = new List<PowerUp>();

        _powerUpRand = new Random();
    }

    private void LoadContent()
    {
        TextureAtlas itemsAtlas = TextureAtlas.FromFile(Core.Content, "images/items-atlas.xml");

        _livesSprite = itemsAtlas.CreateSprite("livesSprite");
        _livesSprite.Scale = SCALE;

        _freezeSprite = itemsAtlas.CreateSprite("freezeSprite");
        _freezeSprite.Scale = SCALE;

        _invincibilitySprite = itemsAtlas.CreateSprite("invincibilitySprite");
        _invincibilitySprite.Scale = INV_SCALE;

        _bombSprite = itemsAtlas.CreateSprite("bombSprite");
        _bombSprite.Scale = SCALE;

        _collectPowerUp = Core.Content.Load<SoundEffect>("audio/Fruit collect 1");

    }

    public void Update()
    {
        foreach(PowerUp powerUp in _powerUps)
        {
            powerUp.Update();
        }
    }

    public void Draw()
    {
        foreach(PowerUp powerUp in _powerUps)
        {
            powerUp.Draw();
        }

        float distanceFromTopWall = 2.0f;
        float livesIndex = 2.5f;
        int roomWidth = Core.GraphicsDevice.PresentationParameters.BackBufferWidth;
        Vector2 livesSpritePosition = new Vector2(roomWidth - _livesSprite.Width * livesIndex, distanceFromTopWall);
        _livesSprite.Draw(Core.SpriteBatch, livesSpritePosition);
    }

    private void AddPowerUp(Vector2 position, powerUpType type)
    {
        switch (type)
        {
            case powerUpType.LIVES:
                _powerUps.Add(new PowerUp(position, type));
            break;
            case powerUpType.CLOCK:
                _powerUps.Add(new PowerUp(position, type));
            break;
            case powerUpType.INVINCIBILITY:
                _powerUps.Add(new PowerUp(position, type));
            break;
            case powerUpType.BOMB:
                _powerUps.Add(new PowerUp(position, type));
            break;
        }
    }

    public void GeneratePowerUp(Vector2 position)
    {
        int rand = _powerUpRand.Next(0, 100);
        if (rand < LIVES_PROB)
        {
            AddPowerUp(position, powerUpType.LIVES);
            _powerUps.Add(new PowerUp(position, powerUpType.LIVES));
        }
        else if (rand < FREEZE_PROB)
        {
            AddPowerUp(position, powerUpType.CLOCK);
        }
        else if (rand < INVINCIBILITY_PROB)
        {
            AddPowerUp(position, powerUpType.INVINCIBILITY);
        }
        else if (rand < BOMB_PROB)
        {
            AddPowerUp(position, powerUpType.BOMB);
        }
    }

    public powerUpType CheckCharacterCollision(Rectangle charBounds)
    {
        foreach(PowerUp powerUp in _powerUps)
        {
            Rectangle powerUpBounds = powerUp.getBounds();

            if (powerUpBounds.Intersects(charBounds))
            {
                Core.Audio.PlaySoundEffect(_collectPowerUp);
                _powerUps.Remove(powerUp);
                return powerUp.GetPowerUpType();
            }

        }

        return powerUpType.NONE;

    }

}