using MonoGameLibrary;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

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
    GREEN_SQUARED,
    LBLUE_SQUARED,
    DBLUE_SQUARED
}

abstract public class Ball
{
    protected Sprite _ballSprite;

    protected Vector2 _velocity;

    protected const float MOVEMENT_SPEED = 5.0f;

    private float _scale;

    protected BallSize _ballSize;

    protected BallType _ballType;

    /// <summary>
    /// Gets or Sets the position of the ball.
    /// </summary>
    public Vector2 Position { get; set; }

    private static SoundEffect _bounceSoundEffect;
    private static SoundEffect _popSoundEffect;

    protected static Texture2D _redBallRoundTexture;
    protected static Texture2D _blueBallRoundTexture;
    protected static Texture2D _greenBallRoundTexture;
    protected static Texture2D _greenBallSquaredTexture;
    protected static Texture2D _lBlueBallSquaredTexture;
    protected static Texture2D _dBlueBallSquaredTexture;

    public static Animation _explosionAnimation;

    public Ball(BallSize ballsize, float dirX, BallType ballType, Vector2 ballInitialPosition = default)
    {
        _ballSize = ballsize;

        _ballType = ballType;

        LoadSprite();

        _scale = _ballSize switch
        {
            BallSize.LARGE => 1.0f,
            BallSize.MEDIUM => 0.5f,
            BallSize.SMALL => 0.25f,
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

        _ballSprite.CenterOrigin();

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

    public float getRadius()
    {
        return _ballSprite.Width * 0.5f;
    }

    public static void playPopSound()
    {
        Core.Audio.PlaySoundEffect(_popSoundEffect);
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
        TextureAtlas itemsAtlas = TextureAtlas.FromFile(Core.Content, "images/Items/items-atlas.xml");

        _redBallRoundTexture = Core.Content.Load<Texture2D>("images/balls/Bouncing/RedBounceBall");
        _blueBallRoundTexture = Core.Content.Load<Texture2D>("images/balls/Bouncing/BlueBounceBall");
        _greenBallRoundTexture = Core.Content.Load<Texture2D>("images/balls/Bouncing/GreenBounceBall");
        _greenBallSquaredTexture = Core.Content.Load<Texture2D>("images/balls/Reflective/GreenReflBall");
        _dBlueBallSquaredTexture = Core.Content.Load<Texture2D>("images/balls/Reflective/DarkBlueReflBall");
        _lBlueBallSquaredTexture = Core.Content.Load<Texture2D>("images/balls/Reflective/LightBlueReflBall");

        _popSoundEffect = Core.Content.Load<SoundEffect>("audio/Sound effects/Balloon Pop 1");
        _bounceSoundEffect = Core.Content.Load<SoundEffect>("audio/Sound effects/bounce");

        _explosionAnimation = itemsAtlas.GetAnimation("Explosion-animation");
    }

    protected abstract void LoadSprite();

}
