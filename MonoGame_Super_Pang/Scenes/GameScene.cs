using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Scenes;
using MonoGame_Super_Pang.GameObjects;
using MonoGame_Super_Pang.Config;
using System.Collections.Generic;

namespace MonoGame_Super_Pang.Scenes;

public class GameScene : Scene
{
    private Character _character;

    private List<Ball> _balls;

    private List<Platform> _platforms;

    private Rectangle _roomBounds;

    private const int HARPOON_DELAY = 5;

    // The SpriteFont Description used to draw text.
    private SpriteFont _font;

    // Tracks the players score.
    private int _score;

    // Defines the position to draw the score text at.
    private Vector2 _scoreTextPosition;

    // Defines the origin used when drawing the score text.
    private Vector2 _scoreTextOrigin;

    private int _lives = 3;

    private Sprite _livesSprite;

    private Sprite _redBallRoundSprite;
    private Sprite _blueBallRoundSprite;
    private Sprite _greenBallRoundSprite;

    private Sprite _greenBallSquaredSprite;

    private Sprite _grayHorizontalPlatform;

    private int _currentLevelIndex;

    public GameScene(int startingLevel)
    {
        _currentLevelIndex = startingLevel;
    }

    public override void Initialize()
    {

        base.Initialize();

        // During the game scene, we want to disable exit on escape. Instead,
        // the escape key will be used to return back to the title screen
        Core.ExitOnEscape = false;

        _roomBounds = Core.GraphicsDevice.PresentationParameters.Bounds;

        _character.Initialize(_roomBounds.Width, _roomBounds.Height);

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
        TextureAtlas baloonsAtlas = TextureAtlas.FromFile(Content, "images/baloons_atlas.xml");
        TextureAtlas platformAtlas = TextureAtlas.FromFile(Content, "images/terrain_atlas.xml");

        // Retrieve regions and animations from the atlas
        Sprite idleRegion = characterAtlas.CreateSprite("characterStanding");
        AnimatedSprite walkAnimation = characterAtlas.CreateAnimatedSprite("walk-animation");
        AnimatedSprite shootAnimation = characterAtlas.CreateAnimatedSprite("shooting-animation");

        // Retrieve balls sprites
        _redBallRoundSprite = itemsAtlas.CreateSprite("redBall");
        _blueBallRoundSprite = itemsAtlas.CreateSprite("blueBall");
        _greenBallRoundSprite = itemsAtlas.CreateSprite("greenBall");
        _greenBallSquaredSprite = baloonsAtlas.CreateSprite("greenBall");

        // Retrieve platforms sprites
        _grayHorizontalPlatform = platformAtlas.CreateSprite("horizontalGrayPlatform");

        _livesSprite = itemsAtlas.CreateSprite("livesSprite");
        _livesSprite.Scale = new Vector2(4.0f, 4.0f);

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

        _platforms = new List<Platform>();

        //_balls.Add(new Ball(redBallSprite, BallSize.LARGE, 1f));

        //_balls.Add(new Ball(redBallSprite, BallSize.LARGE, -1f));

        // Load the font
        _font = Content.Load<SpriteFont>("fonts/04B_30");

        LoadLevel(LevelRegistry.AllLevels[_currentLevelIndex]);
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

        checkChangeScene();

    }

    private void CollisionChecks()
    {
        Rectangle characterBounds = _character.getBounds();

        var toAddBall = new List<Ball>();
        var toRemoveBall = new List<Ball>();
        var toRemoveHarpoon = new List<Harpoon>();

        foreach(Harpoon harpoon in _character.getHarpoons())
        {

            // If the harpoon is on the remove list
            if(toRemoveHarpoon.Contains(harpoon)) continue;

            Rectangle harpoonBound = harpoon.getBounds();

            foreach(Ball ball in _balls)
            {
                // If the ball has been already hit in this frame skip
                if(toRemoveBall.Contains(ball)) continue;

                if(areIntersecting(ball.GetBounds(), harpoonBound))
                {
                    _score++;
                    toAddBall.AddRange(splitBall(ball));
                    toRemoveBall.Add(ball);
                    toRemoveHarpoon.Add(harpoon);
                }
            }
        }

        _balls.RemoveAll(ball => toRemoveBall.Contains(ball));
        _balls.AddRange(toAddBall);
        _character.removeHarpoons(toRemoveHarpoon);

        foreach(Ball ball in _balls)
        {
            if(areIntersecting(ball.GetBounds(), characterBounds) && (_character.IsImmune == false))
            {
                _score--;
                _lives--;
                _character.TakeHit();
                if(_lives == 0)
                    return;
            }
        }

        // Finally, check if the ball is colliding with a wall by validating if
        // it is within the bounds of the room.  If it is outside the room
        // bounds, then it collided with a wall, and the ball should bounce
        // off of that wall.
        foreach(Ball ball in _balls)
        {
            Circle ballBounds = ball.GetBounds();
            Vector2 pos = ball.Position;

            if (ballBounds.Top < _roomBounds.Top)
            {
                ball.Bounce(Vector2.UnitY);
                // Clamp to ceiling
                pos.Y = _roomBounds.Top + (pos.Y - (ballBounds.Y - ballBounds.Radius));
                ball.Position = pos;
            }
            else if (ballBounds.Bottom > _roomBounds.Bottom)
            {
                ball.Bounce(-Vector2.UnitY);
                // Clamp to floor
                pos.Y = _roomBounds.Bottom - ball.spriteHeight
                        - (ballBounds.Bottom - _roomBounds.Bottom);
                ball.Position = pos;
            }

            if (ballBounds.Left < _roomBounds.Left)
            {
                ball.Bounce(Vector2.UnitX);
                // Clamp to left wall
                pos.X = _roomBounds.Left + (pos.X - (ballBounds.X - ballBounds.Radius));
                ball.Position = pos;
            }
            else if (ballBounds.Right > _roomBounds.Right)
            {
                ball.Bounce(-Vector2.UnitX);
                // Clamp to right wall
                pos.X = _roomBounds.Right - ball.spriteWidth
                        - (ballBounds.Right - _roomBounds.Right);
                ball.Position = pos;
            }
        }
    }

    private void checkChangeScene()
    {
        if(_balls.Count == 0)
        {
            _currentLevelIndex++;
            if(_currentLevelIndex >= LevelRegistry.AllLevels.Count)
            {
                Core.ChangeScene(new GameOver(_score));
            }
            else
            {
                LoadLevel(LevelRegistry.AllLevels[_currentLevelIndex]);
            }
            return;
        }

        if(_lives == 0)
        {
            Core.ChangeScene(new GameOver(_score));
            return;
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

        foreach(Platform platform in _platforms)
        {
            platform.Draw();
        }

        for(int livesIndex = 1; livesIndex <= _lives; livesIndex++)
        {
            Rectangle roomBounds = Core.GraphicsDevice.PresentationParameters.Bounds;
            float distanceFromLeftWall = 5.0f;
            Vector2 livesSpritePosition = new Vector2((_livesSprite.Width * 0.5f + distanceFromLeftWall) * livesIndex, roomBounds.Height - _livesSprite.Height);
            _livesSprite.Draw(Core.SpriteBatch, livesSpritePosition);
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

    private void LoadLevel(LevelConfig config)
    {
        _balls.Clear();
        foreach (var spawnConfig in config.Balls)
        {
            BallType ballType = spawnConfig.BallType;
            switch(spawnConfig.BallType)
            {
                case BallType.GREEN_ROUND:
                    _balls.Add(new BouncingBall(new Sprite(_greenBallRoundSprite.Region), spawnConfig.Size, spawnConfig.DirectionX, ballType, spawnConfig.Position));
                break;
                case BallType.RED_ROUND:
                    _balls.Add(new BouncingBall(new Sprite(_redBallRoundSprite.Region), spawnConfig.Size, spawnConfig.DirectionX, ballType, spawnConfig.Position));
                break;
                case BallType.BLUE_ROUND:
                    _balls.Add(new BouncingBall(new Sprite(_blueBallRoundSprite.Region), spawnConfig.Size, spawnConfig.DirectionX, ballType, spawnConfig.Position));
                break;
                case BallType.GREEN_SQUARED:
                    _balls.Add(new ReflectiveBall(new Sprite(_greenBallSquaredSprite.Region), spawnConfig.Size, spawnConfig.DirectionX, ballType, spawnConfig.Position));
                break;
            }
        }

        foreach(var platformSpawn in config.Platforms)
        {
            PlatformType platformType = platformSpawn.platformType;
            switch (platformType)
            {
                case PlatformType.HORIZONTAL_GREEN:
                    _platforms.Add(new Platform(new Sprite(_grayHorizontalPlatform.Region), platformSpawn.Position, platformType));
                break;
            }
        }
    }

    private List<Ball> splitBall(Ball ball)
    {
        List <Ball> toAddBall = new List<Ball>();

        BallSize ballSize = ball.GetBallSize();

        if(ballSize == BallSize.LARGE || ballSize == BallSize.MEDIUM)
        {
            BallType ballType = ball.GetBallType();

            switch(ballType)
            {
                case BallType.GREEN_ROUND:
                case BallType.RED_ROUND:
                case BallType.BLUE_ROUND:
                    toAddBall.Add(new BouncingBall(ball.GetSprite(), ballSize-1, 1f, ballType, ball.Position));
                    toAddBall.Add(new BouncingBall(ball.GetSprite(), ballSize-1, -1f, ballType, ball.Position));
                break;
                case BallType.GREEN_SQUARED:
                    toAddBall.Add(new ReflectiveBall(ball.GetSprite(), ballSize-1, 1f, ballType, ball.Position));
                    toAddBall.Add(new ReflectiveBall(ball.GetSprite(), ballSize-1, -1f, ballType, ball.Position));
                break;
            }
        }

        return toAddBall;
    }

}
