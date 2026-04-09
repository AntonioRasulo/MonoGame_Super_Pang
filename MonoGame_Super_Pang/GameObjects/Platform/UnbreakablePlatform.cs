using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework;

namespace MonoGame_Super_Pang.GameObjects;

public class UnbreakablePlatform : Platform
{
    private Sprite _sprite;

    public UnbreakablePlatform(Vector2 position, PlatformType platformType) : base(position, platformType)
    {
        _sprite.Scale = new Vector2(SCALE, SCALE);
        _sprite.CenterOrigin();
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
            (int)(_position.X - _sprite.Width*0.5f),
            (int)(_position.Y - _sprite.Height*0.5f),
            (int)_sprite.Width,
            (int)_sprite.Height
        );

        return platformBounds;
    }

    protected override void LoadSprite()
    {
        _sprite = _platformType switch
        {
            PlatformType.HORIZONTAL_GRAY => new Sprite(_grayHorizontalPlatform),
            _ => new Sprite(_grayHorizontalPlatform)
        };
    }
}
