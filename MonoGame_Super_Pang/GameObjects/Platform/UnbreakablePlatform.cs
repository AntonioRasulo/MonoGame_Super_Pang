using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework;

namespace MonoGame_Super_Pang.GameObjects;

public class UnbreakablePlatform : Platform
{
    private Sprite _sprite;

    public UnbreakablePlatform(Sprite sprite, Vector2 position, PlatformType platformType) : base(position, platformType)
    {
        _sprite = sprite;
        _sprite.Scale = new Vector2(SCALE, SCALE);
        _breakable = false;
    }

    public override void Draw()
    {
        _sprite.Draw(Core.SpriteBatch, _position);
    }

    public override Rectangle getBounds()
    {
        // Creating a bounding rectangle for the platform
        Rectangle platformBounds = new Rectangle(
            (int)_position.X,
            (int)_position.Y,
            (int)_sprite.Width,
            (int)_sprite.Height
        );

        return platformBounds;
    }
}
