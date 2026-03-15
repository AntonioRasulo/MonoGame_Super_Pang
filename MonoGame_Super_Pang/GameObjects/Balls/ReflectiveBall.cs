using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;

namespace MonoGame_Super_Pang.GameObjects;

public class ReflectiveBall : Ball
{

    private float _rotationSpeed = 0.05f;

    public ReflectiveBall(Sprite ballSprite, BallSize ballSize, float dirX, BallType ballType, Vector2 ballInitialPosition = default)
                        :base(ballSprite, ballSize, dirX, ballType, ballInitialPosition)
    {
        Vector2 direction = new Vector2(dirX, -1);

        // Multiply the direction vector by the movement speed to get the
        // final velocity
        _velocity = direction * MOVEMENT_SPEED;

        _rotationSpeed = _rotationSpeed * dirX;
    }

    public ReflectiveBall(Sprite ballSprite, BallSize ballSize, float dirX, BallType ballType, bool isFreezed)
                        :base(ballSprite, ballSize, dirX, ballType, isFreezed)
    {
        Vector2 direction = new Vector2(dirX, -1);

        // Multiply the direction vector by the movement speed to get the
        // final velocity
        _velocity = direction * MOVEMENT_SPEED;

        _rotationSpeed = _rotationSpeed * dirX;
    }

    public override void Bounce(Vector2 normal)
    {
        base.Bounce(normal);
        Vector2 newPosition = Position;

        // Adjust the position based on the normal to prevent sticking to walls.
        if (normal.X != 0)
        {
            // We are bouncing off a vertical wall (left/right).
            // Move slightly away from the wall in the direction of the normal.
            newPosition.X += normal.X * (_ballSprite.Width * 0.1f);
        }

        if (normal.Y != 0)
        {
            // We are bouncing off a horizontal wall (top/bottom).
            // Move slightly way from the wall in the direction of the normal.
            newPosition.Y += normal.Y * (_ballSprite.Height * 0.1f);
        }

        // Apply the new position
        Position = newPosition;

        // Normalize before reflecting
        normal.Normalize();

        // Apply reflection based on the normal.
        _velocity = Vector2.Reflect(_velocity, normal);

    }

    public override void Update(GameTime gameTime)
    {
        if(!_isFreezed)
        {
            // Update the position of the ball based on the velocity.
            Position += _velocity;

            _ballSprite.Rotation += _rotationSpeed;
        }
        else
        {
            updateFreeze(gameTime);
        }
    }

}
