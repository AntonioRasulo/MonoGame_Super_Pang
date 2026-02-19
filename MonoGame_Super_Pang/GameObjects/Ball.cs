using System;
using MonoGameLibrary;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGame_Super_Pang.GameObjects;

public class Ball
{
    private Sprite _ballSprite;

    private Vector2 _velocity;

    private const float MOVEMENT_SPEED = 5.0f;

    private const float SCALE = 4.0f;

    /// <summary>
    /// Gets or Sets the position of the ball.
    /// </summary>
    public Vector2 Position { get; set; }

    public Ball(Sprite ballSprite)
    {
        _ballSprite = ballSprite;
        _ballSprite.Scale = new Vector2(SCALE, SCALE);
    }

    /// <summary>
    /// Randomizes the velocity of the ball.
    /// </summary>
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

    /// <summary>
    /// Handles a bounce event when the ball collides with a wall or boundary.
    /// </summary>
    /// <param name="normal">The normal vector of the surface the ball is bouncing against.</param>
    public void Bounce(Vector2 normal)
    {
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

    public Rectangle GetRectangleBounds()
    {
        // Creating a bounding rectangle for the character
        Rectangle characterBounds = new Rectangle(
            (int)(Position.X),
            (int)(Position.Y + (_ballSprite.Height * 0.5f)),
            (int)(_ballSprite.Width),
            (int)(_ballSprite.Height * 0.5f *SCALE)
        );

        return characterBounds;
    }

    /// <summary>
    /// Updates the ball.
    /// </summary>
    public void Update()
    {
        // Update the position of the ball based on the velocity.
        Position += _velocity;
    }

    /// <summary>
    /// Draws the ball.
    /// </summary>
    public void Draw()
    {
        _ballSprite.Draw(Core.SpriteBatch, Position);
    }

}
