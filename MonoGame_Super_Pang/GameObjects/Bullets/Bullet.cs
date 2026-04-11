using Microsoft.Xna.Framework;
using MonoGame_Super_Pang.GameObjects;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using System;

namespace MonoGame_Super_Pang.GameObjects;

public abstract class Bullet
{
    protected AnimatedSprite _bulletAnimation;

    protected const float SCALE = 4.0f;

    protected Vector2 _position;

    protected Vector2 _direction;

    protected const float MOVEMENT_SPEED = 200f;

    public bool _isToRemove;

    public Bullet(Animation bulletAnimation, Vector2 position)
    {
        _bulletAnimation = new AnimatedSprite(bulletAnimation);
        _bulletAnimation.Scale = new Vector2(SCALE, SCALE);

        _position = position;

        _isToRemove = false;
    }

    public abstract void Update(GameTime gameTime);

    public void Draw()
    {
        _bulletAnimation.Draw(Core.SpriteBatch, _position);
    }

    public abstract Circle GetBounds();

}
    