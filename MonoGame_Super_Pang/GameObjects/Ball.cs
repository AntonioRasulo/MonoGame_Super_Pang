using System;
using MonoGameLibrary;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace MonoGame_Super_Pang.GameObjects;

public enum BallSize
{
    SMALL,
    MEDIUM,
    LARGE,
    LAST_BALL_SIZE
}

public enum BallType
{
    GREEN_ROUND,
    RED_ROUND,
    BLUE_ROUND,
    GREEN_SQUARED,
    LAST_BALL_TYPE
}

abstract public class Ball
{
    protected Sprite _ballSprite;

    protected Vector2 _velocity;

    protected const float MOVEMENT_SPEED = 5.0f;

    private static float _freezeDuration = 4.0f;
    protected static float _freezeTimer = 0f;

    private float _scale;

    protected BallSize _ballSize;

    protected BallType _ballType;

    /// <summary>
    /// Gets or Sets the position of the ball.
    /// </summary>
    public Vector2 Position { get; set; }

    private static SoundEffect _bounceSoundEffect;
    private static SoundEffect _popSoundEffect;

    protected static TextureRegion _redBallRoundRegion;
    protected static TextureRegion _blueBallRoundRegion;
    protected static TextureRegion _greenBallRoundRegion;

    protected static TextureRegion _greenBallSquaredRegion;

    public Ball(BallSize ballsize, float dirX, BallType ballType, Vector2 ballInitialPosition = default)
    {
        _ballSize = ballsize;

        _ballType = ballType;

        LoadSprite();

        _scale = _ballSize switch
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

        _ballSprite.Origin = new Vector2(_ballSprite.Region.Width, _ballSprite.Region.Height) * 0.5f;

    }

    /// <summary>
    /// Handles a bounce event when the ball collides with a wall or boundary.
    /// </summary>
    /// <param name="normal">The normal vector of the surface the ball is bouncing against.</param>
    public virtual void Bounce(Vector2 normal)
    {
        Core.Audio.PlaySoundEffect(_bounceSoundEffect);
    }

    /// <summary>
    /// Returns a Circle value that represents collision bounds of the ball.
    /// </summary>
    /// <returns>A Circle value.</returns>
    public Circle GetBounds()
    {
        int x = (int)(Position.X + _ballSprite.Width * 0.5f);
        int y = (int)(Position.Y + _ballSprite.Height * 0.5f);
        int radius = (int)(_ballSprite.Width * 0.5f);

        return new Circle(x, y, radius);
    }

    /// <summary>
    /// Updates the ball.
    /// </summary>
    public abstract void Update(GameTime gameTime);

    /// <summary>
    /// Draws the ball.
    /// </summary>
    public void Draw()
    {
        Vector2 centeredPosition = Position + new Vector2(_ballSprite.Width * 0.5f, _ballSprite.Height * 0.5f);
        _ballSprite.Draw(Core.SpriteBatch, centeredPosition);
    }

    public Sprite GetSprite()
    {
        var copy = new Sprite(_ballSprite.Region);
        copy.Scale = _ballSprite.Scale; // copy current scale explicitly
        return copy;
    }

    public BallSize GetBallSize()
    {
        return _ballSize;
    }

    public BallType GetBallType()
    {
        return _ballType;
    }

    public static void updateFreeze(GameTime gameTime)
    {
        if (_freezeTimer > 0f)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _freezeTimer -= delta;

            if (_freezeTimer <= 0f)
            {
                _freezeTimer = 0f;
            }
        }
    }

    public static void Freeze()
    {
        _freezeTimer = _freezeDuration;
    }

    public int spriteWidth => (int)(_ballSprite.Width);
    public int spriteHeight => (int)(_ballSprite.Height);

    public float getRadius()
    {
        return _ballSprite.Width * 0.5f;
    }

    public static void playPopSound()
    {
        Core.Audio.PlaySoundEffect(_popSoundEffect);
    }

    public static void resetFreeze()
    {
        _freezeTimer = 0f;
    }

    public int getScore()
    {
        return _ballSize switch
        {
            BallSize.LARGE => 1,
            BallSize.MEDIUM => 2,
            BallSize.SMALL => 3,
            _ => 0
        };
    }

    public static void LoadContent()
    {
        TextureAtlas itemsAtlas = TextureAtlas.FromFile(Core.Content, "images/items-atlas.xml");

        _redBallRoundRegion = itemsAtlas.GetRegion("redBall");
        _blueBallRoundRegion = itemsAtlas.GetRegion("blueBall");
        _greenBallRoundRegion = itemsAtlas.GetRegion("greenBall");

        Texture2D greenSquaredTexture = Core.Content.Load<Texture2D>("images/HexagonGreenBall");

        _greenBallSquaredRegion = new TextureRegion(greenSquaredTexture, 0, 0, greenSquaredTexture.Width, greenSquaredTexture.Height);

        _popSoundEffect = Core.Content.Load<SoundEffect>("audio/Balloon Pop 1");
        _bounceSoundEffect = Core.Content.Load<SoundEffect>("audio/bounce");

    }

    protected abstract void LoadSprite();

}
