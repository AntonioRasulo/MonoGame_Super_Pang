using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Scenes;
using MonoGame_Super_Pang.GameObjects;
using System.Collections.Generic;

namespace MonoGame_Super_Pang.Scenes;

public class GameScene : Scene
{
    private Character _character;

    private List<Ball> _balls;

    private Rectangle _roomBounds;

    private const int HARPOON_DELAY = 25;

    // The SpriteFont Description used to draw text.
    private SpriteFont _font;

    // Tracks the players score.
    private int _score;

    // Defines the position to draw the score text at.
    private Vector2 _scoreTextPosition;

    // Defines the origin used when drawing the score text.
    private Vector2 _scoreTextOrigin;

    public override void Initialize()
    {

        base.Initialize();

        // During the game scene, we want to disable exit on escape. Instead,
        // the escape key will be used to return back to the title screen
        Core.ExitOnEscape = false;

        _roomBounds = Core.GraphicsDevice.PresentationParameters.Bounds;

        _character.Initialize(_roomBounds.Width, _roomBounds.Height);
        
        foreach(Ball ball in _balls)
        {
            ball.RandomizeVelocity();
            // at the moment, set ball position in the centre of screen
            float roomCenterX = _roomBounds.X + _roomBounds.Width * 0.5f;
            float roomCenterY = _roomBounds.Y + _roomBounds.Height * 0.5f;
            Vector2 roomCenter = new Vector2(roomCenterX, roomCenterY);
            ball.Position = roomCenter;
        }

        // Set the position of the score text to align to the left edge of the
        // room bounds, and to vertically be at the center of the first tile.
        //_scoreTextPosition = new Vector2(_roomBounds.Left, _tilemap.TileHeight * 0.5f); TODO: implement tilemap
        _scoreTextPosition = new Vector2(_roomBounds.Left, 10);

        // // Set the origin of the text so it is left-centered.
        float scoreTextYOrigin = _font.MeasureString("Score").Y * 0.5f;
        _scoreTextOrigin = new Vector2(0, scoreTextYOrigin);
    }

    public override void LoadContent()
    {
        // Create the texture atlas from the XML configuration file
        TextureAtlas characterAtlas = TextureAtlas.FromFile(Content, "images/character_atlas.xml");
        TextureAtlas itemsAtlas = TextureAtlas.FromFile(Content, "images/items-atlas.xml");

        // Retrieve regions and animations from the atlas
        Sprite idleRegion = characterAtlas.CreateSprite("characterStanding");
        AnimatedSprite walkAnimation = characterAtlas.CreateAnimatedSprite("walk-animation");
        AnimatedSprite shootAnimation = characterAtlas.CreateAnimatedSprite("shooting-animation");

        // Retrieve balls sprites
        Sprite redBallSprite = itemsAtlas.CreateSprite("redBall");
        Sprite blueBallSprite = itemsAtlas.CreateSprite("blueBall");
        Sprite greenBallSprite = itemsAtlas.CreateSprite("greenBall");

        // Retrieve harpoons frames
        List<TextureRegion> harpoonFrames = new List<TextureRegion>();
        for (int harpoonIndex = 100; harpoonIndex <= 170; harpoonIndex++)
        {
            String harpoonImagePath = "images/items_" + harpoonIndex;
            Texture2D harpoon2DTexture = Content.Load<Texture2D>(harpoonImagePath);
            TextureRegion harpoonRegion = new TextureRegion(harpoon2DTexture, 0, 0, harpoon2DTexture.Width, harpoon2DTexture.Height);
            harpoonFrames.Add(harpoonRegion);
        }

        Animation harpoonAnimation = new Animation(harpoonFrames, TimeSpan.FromMilliseconds(HARPOON_DELAY));

        _character = new Character(idleRegion, walkAnimation, shootAnimation, harpoonAnimation);

        _balls = new List<Ball>();
        _balls.Add(new Ball(redBallSprite));
        _balls.Add(new Ball(blueBallSprite));
        _balls.Add(new Ball(greenBallSprite));
        _balls.Add(new Ball(redBallSprite));
        _balls.Add(new Ball(blueBallSprite));
        _balls.Add(new Ball(greenBallSprite));

        // Load the font
        _font = Content.Load<SpriteFont>("fonts/04B_30");
    }

    public override void Update(GameTime gameTime)
    {
        _character.Update(gameTime);

        // If the escape key is pressed, return to the title screen.
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            Core.ChangeScene(new TitleScene());
        }

        // Create a bounding rectangle for the screen.
        Rectangle screenBounds = new Rectangle(
            0,
            0,
            Core.GraphicsDevice.PresentationParameters.BackBufferWidth,
            Core.GraphicsDevice.PresentationParameters.BackBufferHeight
        );

        // Getting the bounding rectangle for the character
        Rectangle characterBounds = _character.getBounds();

        Vector2 newCharPosition = _character._characterPosition;

        // Use distance based checks to determine if the character is within the
        // bounds of the game screen, and if it is outside that screen edge,
        // move it back inside.
        if (characterBounds.Left < screenBounds.Left)
        {
            newCharPosition.X = screenBounds.Left;
            _character._characterPosition = newCharPosition;
        }
        else if (characterBounds.Right > screenBounds.Right)
        {
            newCharPosition.X = screenBounds.Right - _character.getWidth();
            _character._characterPosition = newCharPosition;
        }

        foreach(Ball ball in _balls)
        {
            ball.Update();
        }

        CollisionChecks();

    }

    private void CollisionChecks()
    {
        List<Rectangle> harpoonBounds = _character.getHarpoonBounds();
        Rectangle characterBounds = _character.getBounds();

        foreach(Rectangle harpoonBound in harpoonBounds)
        {
            _score += _balls.RemoveAll(ball => areIntersecting(ball.GetBounds(), harpoonBound));
        }

        foreach(Ball ball in _balls)
        {
            if(areIntersecting(ball.GetBounds(), characterBounds) && (_character.IsImmune == false))
            {
                _score--;
                _character.TakeHit();
            }
        }

        // Finally, check if the ball is colliding with a wall by validating if
        // it is within the bounds of the room.  If it is outside the room
        // bounds, then it collided with a wall, and the ball should bounce
        // off of that wall.
        foreach(Ball ball in _balls)
        {
            Circle ballBounds = ball.GetBounds();
            if (ballBounds.Top < _roomBounds.Top)
            {
                ball.Bounce(Vector2.UnitY);
            }
            else if (ballBounds.Bottom > _roomBounds.Bottom)
            {
                ball.Bounce(-Vector2.UnitY);
            }

            if (ballBounds.Left < _roomBounds.Left)
            {
                ball.Bounce(Vector2.UnitX);
            }
            else if (ballBounds.Right > _roomBounds.Right)
            {
                ball.Bounce(-Vector2.UnitX);
            }
        }
    }

    private bool areIntersecting(Circle circle, Rectangle rectangle)
    {
        int circleDistanceX = Math.Abs(circle.X - rectangle.Center.X);
        int circleDistanceY = Math.Abs(circle.Y - rectangle.Center.Y);

        float rectWidth = rectangle.Width * 0.5f;
        float rectHeight = rectangle.Height * 0.5f;

        if((circleDistanceX > (rectWidth + circle.Radius)) ||
           (circleDistanceY > (rectHeight + circle.Radius)))
        {
            return false;
        }

        if(circleDistanceX <= rectWidth ||
           circleDistanceY <= rectHeight)
        {
            return true;
        }

        double cornerDistanceSquare = Math.Pow(circleDistanceX-rectWidth, 2) + Math.Pow(circleDistanceY-rectHeight, 2);

        return cornerDistanceSquare <= Math.Pow(circle.Radius, 2);
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.Brown);

        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw the character
        _character.Draw(Core.SpriteBatch);

        foreach(Ball ball in _balls)
        {
            ball.Draw();
        }

        // Draw the score
        Core.SpriteBatch.DrawString(
            _font,              // spriteFont
            $"Score: {_score}", // text
            _scoreTextPosition, // position
            Color.White,        // color
            0.0f,               // rotation
            _scoreTextOrigin,   // origin
            1.0f,               // scale
            SpriteEffects.None, // effects
            0.0f                // layerDepth
        );

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();

        base.Draw(gameTime);
    }

}
