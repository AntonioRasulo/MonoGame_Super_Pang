using MonoGameLibrary.Graphics;
using MonoGameLibrary;
using Microsoft.Xna.Framework;

public class PowerUp
{
    private Sprite _sprite;
    private Vector2 _position;
    private const float SCALE = 4.0f;
    private Vector2 VELOCITY_Y = new Vector2 (0f, 4.0f);

    public PowerUp(Sprite sprite, Vector2 position)
    {
        _sprite = sprite;
        _sprite.Scale = new Vector2(SCALE, SCALE);
        _position = position;
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

}