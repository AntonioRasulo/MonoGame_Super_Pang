using System;
using System.Collections.Generic;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using MonoGame_Super_Pang.Config;

namespace MonoGame_Super_Pang.GameObjects;

public class CollectibleHandler
{
    private List<Collectible> _collectibles;

    private Random _collectibleRand;

    public static readonly Vector2 SCALE = new(4.0f, 4.0f);
    private readonly Vector2 INV_SCALE = new(0.2f, 0.2f);

    public static Sprite _livesSprite;
    public static Sprite _freezeSprite;
    public static Sprite _invincibilitySprite; 
    public static Sprite _bombSprite;

    public static Animation _goldCoinAnimation;
    public static Animation _silverCoinAnimation;
    public static Animation _bronzeCoinAnimation;

    private static SoundEffect _collectSound;

    private const int LIVES_MAX_PROB = 5; // 5%
    private const int FREEZE_MAX_PROB = 10; // 5%
    private const int INVINCIBILITY_MAX_PROB = 15; // 5%
    private const int BOMB_MAX_PROB = 20; // 5%
    private const int GOLD_COIN_PROB = 25; //5%
    private const int SILVER_COIN_PROB = 30; // 5%
    private const int BRONZE_COIN_PROB = 35; // 5%

    public CollectibleHandler()
    {
        LoadContent();
        _collectibles = new List<Collectible>();

        _collectibleRand = new Random();
    }

    private void LoadContent()
    {
        //TextureAtlas itemsAtlas = TextureAtlas.FromFile(Core.Content, "images/Items/items-atlas.xml");
        TextureAtlas coinsAtlas = TextureAtlas.FromFile(Core.Content, "images/Coins/coins_atlas.xml");

        _livesSprite = new Sprite(Core.Content.Load<Texture2D>("images/PowerUps/lives"));
        _livesSprite.Scale = new Vector2(3.0f, 1.5f);

        _freezeSprite = new Sprite(Core.Content.Load<Texture2D>("images/PowerUps/Clock"));
        _freezeSprite.Scale = new Vector2(0.07f, 0.07f);

        _invincibilitySprite = new Sprite(Core.Content.Load<Texture2D>("images/PowerUps/shield"));
        _invincibilitySprite.Scale = INV_SCALE;

        Texture2D bombTexture2D = Core.Content.Load<Texture2D>("images/PowerUps/dynamite-pack");
        TextureRegion bombTexture = new TextureRegion(bombTexture2D, 5, 2, 21, 27);
        _bombSprite = new Sprite(bombTexture);
        _bombSprite.Scale = new Vector2(2.5f, 2.5f);

        _collectSound = Core.Content.Load<SoundEffect>("audio/Sound effects/Fruit collect 1");

        _goldCoinAnimation = coinsAtlas.GetAnimation("gold-animation");

        _silverCoinAnimation = coinsAtlas.GetAnimation("silver-animation");

        _bronzeCoinAnimation = coinsAtlas.GetAnimation("bronze-animation");

    }

    public void Update(GameTime gameTime)
    {
        foreach(Collectible collectible in _collectibles)
        {
            collectible.Update(gameTime);
        }
    }

    public void Draw()
    {
        foreach(Collectible collectible in _collectibles)
        {
            collectible.Draw();
        }

        float distanceFromTopWall = 17.0f;
        float livesIndex = 3.2f;
        int roomWidth = Core.GraphicsDevice.PresentationParameters.BackBufferWidth;
        Vector2 livesSpritePosition = new Vector2(roomWidth - _livesSprite.Width * livesIndex, distanceFromTopWall);
        _livesSprite.Draw(Core.SpriteBatch, livesSpritePosition);
    }

    public void GenerateCollectible(Vector2 position)
    {
        int rand = _collectibleRand.Next(0, 100);
        if (rand < PlayerStatsManager.currentStats.LivesProb)
        {
            _collectibles.Add(new PowerUp(position, collectibleType.LIVES));
        }
        else if (rand < PlayerStatsManager.currentStats.clockProbability && rand >LIVES_MAX_PROB)
        {
            _collectibles.Add(new PowerUp(position, collectibleType.CLOCK));
        }
        else if (rand < PlayerStatsManager.currentStats.InvincibilityProb && rand > FREEZE_MAX_PROB)
        {
            _collectibles.Add(new PowerUp(position, collectibleType.INVINCIBILITY));
        }
        else if (rand < PlayerStatsManager.currentStats.bombProbability && rand > INVINCIBILITY_MAX_PROB)
        {
            _collectibles.Add(new PowerUp(position, collectibleType.BOMB));
        }
        else if (rand < GOLD_COIN_PROB && rand > BOMB_MAX_PROB)
        {
            _collectibles.Add(new Coin(position, collectibleType.GOLD_COIN));
        }
        else if (rand < SILVER_COIN_PROB && rand > GOLD_COIN_PROB)
        {
            _collectibles.Add(new Coin(position, collectibleType.SILVER_COIN));
        }
        else if(rand < BRONZE_COIN_PROB && rand > SILVER_COIN_PROB)
        {
            _collectibles.Add(new Coin(position, collectibleType.BRONZE_COIN));
        }
    }

    public collectibleType CheckCharacterCollision(Rectangle charBounds)
    {
        foreach(Collectible collectible in _collectibles)
        {
            Rectangle collectibleBounds = collectible.getBounds();

            if (collectibleBounds.Intersects(charBounds))
            {
                Core.Input.GamePads[(int)PlayerIndex.One].SetVibration(0.1f, TimeSpan.FromMilliseconds(100));
                Core.Audio.PlaySoundEffect(_collectSound);
                _collectibles.Remove(collectible);
                return collectible.GetCollectibleType();
            }

        }

        return collectibleType.NONE;

    }

    public static void PlayCollectibleSound()
    {
        Core.Audio.PlaySoundEffect(_collectSound);
    }

}