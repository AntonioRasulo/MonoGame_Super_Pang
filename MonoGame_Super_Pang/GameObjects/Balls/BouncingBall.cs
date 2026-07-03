using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace MonoGame_Super_Pang.GameObjects;

public class BouncingBall : Ball
{
    private const float GRAVITY = 0.15f;

    private float _jumpStrength;

    private float _speedX;

    public BouncingBall(BallSize ballSize, float dirX, BallType ballType, Vector2 ballInitialPosition = default)
                        :base(ballSize, dirX, ballType, ballInitialPosition)
    {
        (_jumpStrength, _speedX) = _ballSize switch
        {
            BallSize.LARGE => (10f, 1.5f),
            BallSize.MEDIUM => (9f, 2.0f),
            BallSize.SMALL => (8f, 2.5f),
            _ => (8f, 2.5f)
        };

        // Start moving upward
        _velocity = new Vector2(_speedX * dirX, -_jumpStrength);

    }

    public override void Bounce(Vector2 normal)
    {
        base.Bounce(normal);
        if (normal.X != 0)
        {
            // normal.X is +1 or -1, directly sets correct direction
            _velocity.X = _speedX * normal.X;
        }

         // Floor
        if (normal.Y < 0)
        {
            _velocity.Y = -_jumpStrength;
        }
        else if (normal.Y > 0) // Ceiling
        {
            _velocity.Y = Math.Abs(_velocity.Y);
        }

        Vector2 newPosition = Position;

        // Adjust the position based on the normal to prevent sticking to walls.
        if (normal.X != 0)
        {
            // We are bouncing off a vertical wall (left/right).
            // Move slightly away from the wall in the direction of the normal.
            newPosition.X += normal.X * (_ballSprite.Width * 0.05f);
        }

        if (normal.Y != 0)
        {
            // We are bouncing off a horizontal wall (top/bottom).
            // Move slightly way from the wall in the direction of the normal.
            newPosition.Y += normal.Y * (_ballSprite.Height * 0.05f);
        }

        Position = newPosition;

    }

    public override void Update(GameTime gameTime)
    {
        if(FreezeHandler.freezeTimer <= 0)
        {
            // Apply gravity each frame
            _velocity.Y += GRAVITY;

            // Update the position of the ball based on the velocity.
            Position += _velocity;
        }
    }

    protected override void LoadSprite()
    {
        _ballSprite = _ballType switch
        {
            BallType.RED_ROUND => new Sprite(_redBallRoundTexture),
            BallType.GREEN_ROUND => new Sprite(_greenBallRoundTexture),
            BallType.BLUE_ROUND => new Sprite(_blueBallRoundTexture),
            _ => new Sprite(_blueBallRoundTexture)
        };
    }

}
