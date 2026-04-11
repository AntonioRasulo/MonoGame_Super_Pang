using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using System;

namespace MonoGame_Super_Pang.GameObjects;

public class Fireball : Bullet
{
    private const int RADIUS = 10;
    private const int X_CENTRE = 43;
    private const int Y_CENTRE = 14;

    public Fireball(Animation fireballAnimation, Vector2 position) : base(fireballAnimation, position)
    {

        _bulletAnimation.Origin = new Vector2(X_CENTRE, Y_CENTRE);

        _direction = Character._characterPosition - _position;

        _bulletAnimation.Rotation =  (float)(Math.Atan2(Character._characterPosition.Y - _position.Y, Character._characterPosition.X - _position.X));

    }

    public override void Update(GameTime gameTime)
    {
        _bulletAnimation.Update(gameTime);
        UpdateMovement(gameTime);
    }

    public override Circle GetBounds()
    {
        // int x = (int)(_position.X + _bulletAnimation.Width * 0.5f);
        // int y = (int)(_position.Y + _bulletAnimation.Height * 0.5f);
        int radius = (int)(_bulletAnimation.Width * 0.5f);

        return new Circle((int)_position.X, (int)_position.Y, (int)(RADIUS * SCALE));
    }

    private void UpdateMovement(GameTime gameTime)
    {
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _direction.Normalize();
        _position += _direction * MOVEMENT_SPEED * delta;

        if(_position.Y > Core.GraphicsDevice.PresentationParameters.BackBufferHeight)
        {
            _isToRemove = true;
        }
    }

}