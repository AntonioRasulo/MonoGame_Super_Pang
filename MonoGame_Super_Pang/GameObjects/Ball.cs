using System;
using MonoGameLibrary;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGame_Super_Pang.GameObjects;

public enum BallSize
{
    SMALL,
    MEDIUM,
    LARGE
}

public enum BallType
{
    GREEN_ROUND,
    RED_ROUND,
    BLUE_ROUND,
    GREEN_SQUARED
}

abstract public class Ball
{
    private Sprite _ballSprite;

    protected Vector2 _velocity;

    private const float MOVEMENT_SPEED = 5.0f;

    private float _scale;

    protected BallSize _ballSize;

    protected BallType _ballType;

    /// <summary>
    /// Gets or Sets the position of the ball.
    /// </summary>
    public Vector2 Position { get; set; }

    public Ball(Sprite ballSprite, BallSize ballsize, float dirX, BallType ballType, Vector2 ballInitialPosition = default)
    {
        _ballSprite = ballSprite;

        _ballSize = ballsize;

        (_scale) = _ballSize switch
        {
            BallSize.LARGE => 4.0f,
            BallSize.MEDIUM => 2.0f,
            BallSize.SMALL => 1.0f,
            _ => 1.0f
        };

        _ballSprite.Scale = new Vector2(_scale, _scale);

        if(ballInitialPosition == default)
        {
            Rectangle roomBounds = Core.GraphicsDevice.PresentationParameters.Bounds;
            // at the moment, set ball position in the centre of screen
            float roomCenterX = roomBounds.X + roomBounds.Width * 0.5f;
            float roomCenterY = roomBounds.Y + roomBounds.Height * 0.5f;
            Vector2 roomCenter = new Vector2(roomCenterX, roomCenterY);
            Position = roomCenter;
        }
        else
        {
            Position = ballInitialPosition;
        }
        
    }

    /// <summary>
    /// Handles a bounce event when the ball collides with a wall or boundary.
    /// </summary>
    /// <param name="normal">The normal vector of the surface the ball is bouncing against.</param>
    public abstract void Bounce(Vector2 normal);

    /// <summary>
    /// Returns a Circle value that represents collision bounds of the ball.
    /// </summary>
    /// <returns>A Circle value.</returns>
    public Circle GetBounds()
    {
        int x = (int)(Position.X + _ballSprite.Width * 0.5f);
        int y = (int)(Position.Y + _ballSprite.Height * 0.5f);
        int radius = (int)(_ballSprite.Width * 0.4f);

        return new Circle(x, y, radius);
    }

    /// <summary>
    /// Updates the ball.
    /// </summary>
    public abstract void Update();

    /// <summary>
    /// Draws the ball.
    /// </summary>
    public void Draw()
    {
        _ballSprite.Draw(Core.SpriteBatch, Position);
    }

    public Sprite GetSprite()
    {
        return new Sprite(_ballSprite.Region);
    }

    public BallSize GetBallSize()
    {
        return _ballSize;
    }

    public BallType GetBallType()
    {
        return _ballType;
    }

    public int spriteWidth => (int)(_ballSprite.Width);
    public int spriteHeight => (int)(_ballSprite.Height);

    /// <summary>
    /// Randomizes the velocity of the ball.
    /// </summary>
    /// TODO: move this in ReflectiveBall class
    public void RandomizeVelocity()
    {
        // Generate a random angle
        float angle = (float)(Random.Shared.NextDouble() * MathHelper.TwoPi);

        // Convert the angle to a direction vector
        float x = (float)Math.Cos(angle);
        float y = (float)Math.Sin(angle);
        Vector2 direction = new Vector2(x, y);

        // Multiply the direction vector by the movement speed to get the
        // final velocity
        _velocity = direction * MOVEMENT_SPEED;
    }

}
