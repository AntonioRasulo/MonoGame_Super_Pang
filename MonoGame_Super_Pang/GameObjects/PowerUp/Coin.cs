using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace MonoGame_Super_Pang.GameObjects;

public class Coin : Collectible
{
    AnimatedSprite _animatedSprite;
    private static int GOLD_VALUE = 5;
    private static int SILVER_VALUE = 3;
    private static int BRONZE_VALUE = 1;

    public Coin(Vector2 position, collectibleType type):
    base(position, type)
    {
        Animation coinAnimation = type switch
        {
            collectibleType.GOLD_COIN => CollectibleHandler._goldCoinAnimation,
            collectibleType.SILVER_COIN => CollectibleHandler._silverCoinAnimation,
            collectibleType.BRONZE_COIN => CollectibleHandler._bronzeCoinAnimation,
            _ => null
        };

        _animatedSprite = new AnimatedSprite(coinAnimation);
        _animatedSprite.Scale = CollectibleHandler.SCALE;
    }

    public override void Update(GameTime gameTime)
    {
        int screenHeight = Core.GraphicsDevice.PresentationParameters.BackBufferHeight;
        if(_position.Y < screenHeight - _animatedSprite.Height)
        {
            _position += VELOCITY_Y;
        }
        else
        {
            _position.Y = screenHeight - _animatedSprite.Height;
        }

        _animatedSprite.Update(gameTime);
    }

    public override Rectangle getBounds()
    {
        Rectangle bounds = new Rectangle(
            (int)_position.X,
            (int)_position.Y,
            (int)_animatedSprite.Width,
            (int)_animatedSprite.Height
        );

        return bounds;    }

    public override void Draw()
    {
        _animatedSprite.Draw(Core.SpriteBatch, _position);
    }

    public static int GetValue(collectibleType type)
    {
        return type switch
        {
            collectibleType.GOLD_COIN => GOLD_VALUE,
            collectibleType.SILVER_COIN => SILVER_VALUE,
            collectibleType.BRONZE_COIN => BRONZE_VALUE,
            _ => 0
        };
    }

}