using MonoGameLibrary.Graphics;
using MonoGameLibrary;
using Microsoft.Xna.Framework;

public enum powerUpType
{
    LIVES,
    CLOCK,
    INVINCIBILITY,
    BOMB
}

public class PowerUp
{
    private Sprite _sprite;
    private Vector2 _position;
    private Vector2 VELOCITY_Y = new Vector2 (0f, 4.0f);

    private powerUpType _type;

    public PowerUp(Sprite sprite, Vector2 position, powerUpType type)
    {
        _sprite = sprite;
        _position = position;
        _type = type;
    }

    public void Update()
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

    public void Draw()
    {
        _sprite.Draw(Core.SpriteBatch, _position);
    }

    public Rectangle getBounds()
    {
        Rectangle bounds = new Rectangle(
            (int)_position.X,
            (int)_position.Y,
            (int)_sprite.Width,
            (int)_sprite.Height
        );

        return bounds;

    }

    public powerUpType GetPowerUpType()
    {
        return _type;
    }

}