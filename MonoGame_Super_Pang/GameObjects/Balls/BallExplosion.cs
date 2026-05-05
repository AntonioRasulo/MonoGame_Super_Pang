using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace MonoGame_Super_Pang.GameObjects;

public class BallExplosion
{
    private static AnimatedSprite _explosion;

    private Vector2 _position;

    public BallExplosion(Color color, Vector2 Position) : base()
    {
        _explosion = new AnimatedSprite(Ball._explosionAnimation);
        _explosion.Color = color;
        _position = Position;
    }

    public void Update(GameTime gameTime)
    {
        _explosion.Update(gameTime);
    }

    public void Draw()
    {
        _explosion.Draw(Core.SpriteBatch, _position);
    }

    public bool isComplete()
    {
        return _explosion.IsComplete;
    }

}