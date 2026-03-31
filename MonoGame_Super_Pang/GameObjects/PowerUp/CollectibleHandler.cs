using System;
using System.Collections.Generic;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace MonoGame_Super_Pang.GameObjects;

public class CollectibleHandler
{
    private List<Collectible> _collectibles;

    private Random _collectibleRand;

    public static readonly Vector2 SCALE = new(4.0f, 4.0f);
    private readonly Vector2 INV_SCALE = new(1.5f, 1.5f);

    public static Sprite _livesSprite;
    public static Sprite _freezeSprite;
    public static Sprite _invincibilitySprite; 
    public static Sprite _bombSprite;

    public static Animation _goldCoinAnimation;
    public static Animation _silverCoinAnimation;
    public static Animation _bronzeCoinAnimation;

    private SoundEffect _collectSound;

    private const int LIVES_PROB = 5; // 5%
    private const int FREEZE_PROB = 10; // 5%
    private const int INVINCIBILITY_PROB = 15; // 5%
    private const int BOMB_PROB = 20; // 5%
    private const int GOLD_COIN_PROB = 23; //3%
    private const int SILVER_COIN_PROB = 27; // 4%
    private const int BRONZE_COIN_PROB = 32; // 5%

    public CollectibleHandler()
    {
        LoadContent();
        _collectibles = new List<Collectible>();

        _collectibleRand = new Random();
    }

    private void LoadContent()
    {
        TextureAtlas itemsAtlas = TextureAtlas.FromFile(Core.Content, "images/items-atlas.xml");
        TextureAtlas coinsAtlas = TextureAtlas.FromFile(Core.Content, "images/Coins/coins_atlas.xml");

        _livesSprite = itemsAtlas.CreateSprite("livesSprite");
        _livesSprite.Scale = SCALE;

        _freezeSprite = itemsAtlas.CreateSprite("freezeSprite");
        _freezeSprite.Scale = SCALE;

        _invincibilitySprite = itemsAtlas.CreateSprite("invincibilitySprite");
        _invincibilitySprite.Scale = INV_SCALE;

        _bombSprite = itemsAtlas.CreateSprite("bombSprite");
        _bombSprite.Scale = SCALE;

        _collectSound = Core.Content.Load<SoundEffect>("audio/Fruit collect 1");

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

        float distanceFromTopWall = 2.0f;
        float livesIndex = 2.5f;
        int roomWidth = Core.GraphicsDevice.PresentationParameters.BackBufferWidth;
        Vector2 livesSpritePosition = new Vector2(roomWidth - _livesSprite.Width * livesIndex, distanceFromTopWall);
        _livesSprite.Draw(Core.SpriteBatch, livesSpritePosition);
    }

    public void GenerateCollectible(Vector2 position)
    {
        int rand = _collectibleRand.Next(0, 100);
        if (rand < LIVES_PROB)
        {
            _collectibles.Add(new PowerUp(position, collectibleType.LIVES));
        }
        else if (rand < FREEZE_PROB)
        {
            _collectibles.Add(new PowerUp(position, collectibleType.CLOCK));
        }
        else if (rand < INVINCIBILITY_PROB)
        {
            _collectibles.Add(new PowerUp(position, collectibleType.INVINCIBILITY));
        }
        else if (rand < BOMB_PROB)
        {
            _collectibles.Add(new PowerUp(position, collectibleType.BOMB));
        }
        else if (rand < GOLD_COIN_PROB)
        {
            _collectibles.Add(new Coin(position, collectibleType.GOLD_COIN));
        }
        else if (rand < SILVER_COIN_PROB)
        {
            _collectibles.Add(new Coin(position, collectibleType.SILVER_COIN));
        }
        else if(rand < BRONZE_COIN_PROB)
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
                Core.Audio.PlaySoundEffect(_collectSound);
                _collectibles.Remove(collectible);
                return collectible.GetCollectibleType();
            }

        }

        return collectibleType.NONE;

    }

}