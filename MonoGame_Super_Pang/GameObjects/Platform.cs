//using System.Numerics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework;

namespace MonoGame_Super_Pang.GameObjects;

public enum PlatformType
{
    HORIZONTAL_GREEN
};

public class Platform
{
    private Sprite _sprite;

    private Vector2 _position;

    private PlatformType _platformType;

    private const float SCALE = 4f;

    public Platform(Sprite sprite, Vector2 position, PlatformType platformType)
    {
        _sprite = sprite;
        _sprite.Scale = new Vector2(SCALE, SCALE);
        _position = position;
        _platformType = platformType;
    }

    public void Draw()
    {
        _sprite.Draw(Core.SpriteBatch, _position);
    }
}