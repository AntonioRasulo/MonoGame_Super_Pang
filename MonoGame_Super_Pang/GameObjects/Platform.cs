using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework;

namespace MonoGame_Super_Pang.GameObjects;

public enum PlatformType
{
    HORIZONTAL_GRAY,
    BREAKABLE_LARGE_HORIZONTAL_BLUE
};

abstract public class Platform
{
    //private Sprite _sprite;

    protected Vector2 _position;

    private PlatformType _platformType;

    protected const float SCALE = 4f;

    protected bool _breakable;

    public Platform(Vector2 position, PlatformType platformType)
    {
        _position = position;
        _platformType = platformType;
    }

    public abstract void Draw();

    public abstract Rectangle getBounds();

    public bool isBreakable()
    {
        return _breakable;
    }

}