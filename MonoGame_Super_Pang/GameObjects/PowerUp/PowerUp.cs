using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework;

namespace MonoGame_Super_Pang.GameObjects;

public class PowerUp : Collectible
{
    private Sprite _sprite;

    public PowerUp(Vector2 position, collectibleType type):
    base(position, type)
    {
        _sprite = type switch
        {
            collectibleType.LIVES => CollectibleHandler._livesSprite,
            collectibleType.BOMB => CollectibleHandler._bombSprite,
            collectibleType.CLOCK => CollectibleHandler._freezeSprite,
            collectibleType.INVINCIBILITY => CollectibleHandler._invincibilitySprite,
            _ => null
        };   
    }

    public override void Update(GameTime gameTime)
    {
        int screenHeight = Core.GraphicsDevice.PresentationParameters.BackBufferHeight;
        if(_position.Y < screenHeight - _sprite.Height)
        {
            _position += VELOCITY_Y;
        }
        else
        {
            _position.Y = screenHeight - _sprite.Height;
        }   
    }

    public override void Draw()
    {
        _sprite.Draw(Core.SpriteBatch, _position);
    }

    public override Rectangle getBounds()
    {
        Rectangle bounds = new Rectangle(
            (int)_position.X,
            (int)_position.Y,
            (int)_sprite.Width,
            (int)_sprite.Height
        );

        return bounds;

    }

}