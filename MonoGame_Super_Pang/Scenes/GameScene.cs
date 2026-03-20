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
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using MonoGame_Super_Pang.UI;
using MonoGameGum;
using MonoGame_Super_Pang.Backgrounds;

namespace MonoGame_Super_Pang.Scenes;

public class GameScene : Scene
{
    private enum GameState
    {
        Playing,
        Paused
    }

    private Character _character;

    private List<Ball> _balls;

    private List<Platform> _platforms;

    private Rectangle _roomBounds;

    private const int HARPOON_DELAY = 5;

    // The SpriteFont Description used to draw text.
    private SpriteFont _font;

    // Tracks the players score.
    private int _score;

    private int _lives = 3;

    private Sprite _livesSprite;
    private Sprite _freezeSprite;
    private Sprite _invincibilitySprite; 
    private Sprite _bombSprite;
    private Sprite _redBallRoundSprite;
    private Sprite _blueBallRoundSprite;
    private Sprite _greenBallRoundSprite;

    private Sprite _greenBallSquaredSprite;

    private Sprite _grayHorizontalPlatform;

    private List<Sprite> _horizontalBreakableBlueSprites;

    private int _currentLevelIndex;

    private Random _powerUpRand;

    private List<PowerUp> _powerUps;

    private const int LIVES_PROB = 5;
    private const int FREEZE_PROB = 10;
    private const int INVINCIBILITY_PROB = 15;
    private const int BOMB_PROB = 20;

    private GameSceneUI _ui;

    private GameState _state;

    // The grayscale shader effect.
    private Effect _grayscaleEffect;

    // The amount of saturation to provide the grayscale shader effect.
    private float _saturation = 1.0f;

    // The speed of the fade to grayscale effect.
    private const float FADE_SPEED = 0.02f;

    private SoundEffect _collectPowerUp;

    private SoundEffect _blockBreakEffect;

    private Background _levelBackground;

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

        // Create any UI elements from the root element created in previous
        // scenes.
        GumService.Default.Root.Children.Clear();

        // Initialize the user interface for the game scene.
        InitializeUI();

        // Initialize a new game to be played.
        InitializeNewGame();

        _powerUpRand = new Random();
        _powerUps = new List<PowerUp>();
    }

    private void InitializeUI()
    {
        // Clear out any previous UI element incase we came here
        // from a different scene.
        GumService.Default.Root.Children.Clear();

        // Create the game scene ui instance.
        _ui = new GameSceneUI();

        // Subscribe to the events from the game scene ui.
        _ui.ResumeButtonClick += OnResumeButtonClicked;
        _ui.RetryButtonClick += OnRetryButtonClicked;
        _ui.QuitButtonClick += OnQuitButtonClicked;
    }

    // TODO complete implementation
    private void InitializeNewGame()
    {
        _state = GameState.Playing;
    }

    private void OnResumeButtonClicked(object sender, EventArgs args)
    {
        // Change the game state back to playing.
        _state = GameState.Playing;
    }

    private void OnRetryButtonClicked(object sender, EventArgs args)
    {
        // Player has chosen to retry, so initialize a new game.
        //InitializeNewGame(); TODO
    }

    private void OnQuitButtonClicked(object sender, EventArgs args)
    {
        // Player has chosen to quit, so return back to the title scene.
        Core.ChangeScene(new TitleScene());
    }

    public override void LoadContent()
    {
        // Load the background theme music
        Song theme = Content.Load<Song>("audio/16. Battle Theme III (loop)");
        Core.Audio.PlaySong(theme);

        // Load bouncing sound
        Ball.loadPopSound(Content.Load<SoundEffect>("audio/Balloon Pop 1"));
        Ball.loadBounceSound(Content.Load<SoundEffect>("audio/bounce"));

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

        _horizontalBreakableBlueSprites = new List<Sprite>();
        for(int indexPlatform = 1; indexPlatform<=5; indexPlatform++)
        {
            String spriteName = "largeBreakableBluePlatform"+indexPlatform;
            _horizontalBreakableBlueSprites.Add(itemsAtlas.CreateSprite(spriteName));
        }

        Texture2D greenSquaredTexture = Content.Load<Texture2D>("images/HexagonGreenBall");
        _greenBallSquaredSprite = new Sprite(greenSquaredTexture);

        // Retrieve platforms sprites
        _grayHorizontalPlatform = platformAtlas.CreateSprite("horizontalGrayPlatform");

        _livesSprite = itemsAtlas.CreateSprite("livesSprite");
        _livesSprite.Scale = new Vector2(4.0f, 4.0f);

        _freezeSprite = itemsAtlas.CreateSprite("freezeSprite");
        _freezeSprite.Scale = new Vector2(4.0f, 4.0f);

        _invincibilitySprite = itemsAtlas.CreateSprite("invincibilitySprite");
        _invincibilitySprite.Scale = new Vector2(1.5f, 1.5f);

        _bombSprite = itemsAtlas.CreateSprite("bombSprite");
        _bombSprite.Scale = new Vector2(4.0f, 4.0f);

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

        _character = new Character(idleRegion, walkAnimation, shootAnimation, harpoonAnimation, itemsAtlas.GetRegion("invincibilitySprite"));

        _character.loadHitSound(Content.Load<SoundEffect>("audio/Boss hit 1"));

        _balls = new List<Ball>();

        _platforms = new List<Platform>();

        // Load the font
        _font = Content.Load<SpriteFont>("fonts/04B_30");

        _blockBreakEffect = Content.Load<SoundEffect>("audio/Block Break 1");

        LoadLevel(LevelRegistry.AllLevels[_currentLevelIndex]);

        // Load the grayscale effect.
        _grayscaleEffect = Content.Load<Effect>("effects/grayscaleEffect");

        _collectPowerUp = Content.Load<SoundEffect>("audio/Fruit collect 1");

    }

    public override void Update(GameTime gameTime)
    {
        // Ensure the UI is always updated.
        _ui.Update(gameTime);

        if (_state != GameState.Playing)
        {
            // The game is in either a paused or game over state, so
            // gradually decrease the saturation to create the fading grayscale.
            _saturation = Math.Max(0.0f, _saturation - FADE_SPEED);

        }

        // If the pause button is pressed, toggle the pause state. TODO implement GameController
        // if (GameController.Pause())
        // {
        //     TogglePause();
        // }
        if(Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            TogglePause();
        }

        // At this point, if the game is paused, just return back early.
        if (_state == GameState.Paused)
        {
            return;
        }

        _character.Update(gameTime);

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
            ball.Update(gameTime);
        }

        foreach(PowerUp powerUp in _powerUps)
        {
            powerUp.Update();
        }

        CollisionChecks();

        checkChangeScene();

        _levelBackground.Update(gameTime);

    }

    private void TogglePause()
    {
        if (_state == GameState.Paused)
        {
            // We're now unpausing the game, so hide the pause panel.
            _ui.HidePausePanel();

            // And set the state back to playing.
            _state = GameState.Playing;
        }
        else
        {
            // We're now pausing the game, so show the pause panel.
            _ui.ShowPausePanel();

            // And set the state to paused.
            _state = GameState.Paused;

            // Set the grayscale effect saturation to 1.0f
            _saturation = 1.0f;
        }
    }

    private void CollisionChecks()
    {
        Rectangle characterBounds = _character.getBounds();

        var toRemovePowerUps = new List<PowerUp>();
        var toAddBall = new List<Ball>();
        var toRemoveBall = new List<Ball>();
        var toAddPowerUps = new List<PowerUp>();

        /* Character - PowerUp collision */
        foreach(PowerUp powerUp in _powerUps)
        {
            Rectangle powerUpBounds = powerUp.getBounds();
            if (powerUpBounds.Intersects(characterBounds))
            {
                Core.Audio.PlaySoundEffect(_collectPowerUp);
                switch (powerUp.GetPowerUpType())
                {
                    case powerUpType.LIVES:
                        _lives++;
                    break;
                    case powerUpType.CLOCK:
                        Ball.Freeze();        
                    break;
                    case powerUpType.INVINCIBILITY:
                        {
                            _character.activateImmunity(true);
                        }
                    break;
                    case powerUpType.BOMB:
                        {
                            foreach(Ball ball in _balls)
                            {
                                handleBallHit(ball, ref toAddBall, ref toRemoveBall, ref toAddPowerUps);
                            }
                            _balls.AddRange(toAddBall);
                            _balls.RemoveAll(ball => toRemoveBall.Contains(ball));
                        }
                        break;
                }
                toRemovePowerUps.Add(powerUp);
            }
        }

        _powerUps.AddRange(toAddPowerUps);
        _powerUps.RemoveAll(powerUp => toRemovePowerUps.Contains(powerUp));

        toAddBall.Clear();
        toRemoveBall.Clear();
        var toRemoveHarpoon = new List<Harpoon>();
        toAddPowerUps.Clear(); 

        /* Harpoon - Ball collision check */
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
                    handleBallHit(ball, ref toAddBall, ref toRemoveBall, ref toAddPowerUps);
                    toRemoveHarpoon.Add(harpoon);
                }
            }
        }

        _balls.RemoveAll(ball => toRemoveBall.Contains(ball));
        _balls.AddRange(toAddBall);
        _character.removeHarpoons(toRemoveHarpoon);
        _powerUps.AddRange(toAddPowerUps);

        /* Platform - Ball collision check */
        foreach(Platform platform in _platforms)
        {
            Rectangle platformBounds = platform.getBounds();
            foreach(Ball ball in _balls)
            {
                Circle ballBounds = ball.GetBounds();

                if(areIntersecting(ballBounds, platformBounds))
                {
                    Vector2 pos = ball.Position;
                    
                    int []distances =
                    {
                        Math.Abs(ballBounds.Top - platformBounds.Bottom), // From bottom
                        Math.Abs(ballBounds.Bottom -  platformBounds.Top), // From top
                        Math.Abs(ballBounds.Right - platformBounds.Left), // From Left
                        Math.Abs(ballBounds.Left - platformBounds.Right), // From right
                    };
                    int indexMin = 0;
                    int min = distances[0];
                    for (int i = 1; i < distances.Length; i++)
                    {
                        if (distances[i] < min)
                        {
                            min = distances[i];
                            indexMin = i;
                        }
                    }

                    switch (indexMin)
                    {
                        case 0:
                        ball.Bounce(Vector2.UnitY);
                        break;
                        case 1:
                        ball.Bounce(-Vector2.UnitY);
                        break;
                        case 2:
                        ball.Bounce(-Vector2.UnitX);
                        break;
                        case 3:
                        ball.Bounce(Vector2.UnitX);
                        break;
                    }
                }
            }
        }

        /* Character - Ball collision check */
        foreach(Ball ball in _balls)
        {
            if(areIntersecting(ball.GetBounds(), characterBounds) && (_character.IsImmune == false))
            {
                _score--;
                _lives--;
                _character.activateImmunity();
                // Update the score display on the UI.
                _ui.UpdateScoreText(_score);
                if(_lives == 0)
                    return;
            }
        }

        /* Harpoon - Platform collision check */
        var toRemovePlatform = new List<Platform>();
        toRemoveHarpoon = new List<Harpoon>();
        foreach(Harpoon harpoon in _character.getHarpoons())
        {
            Rectangle harpoonBounds = harpoon.getBounds();
            foreach(Platform platform in _platforms)
            {
                Rectangle platformBounds = platform.getBounds();

                if (harpoonBounds.Intersects(platformBounds))
                {
                    toRemoveHarpoon.Add(harpoon);
                    if(platform.isBreakable() == true)
                    {
                        ((BreakablePlatform)platform).hitPlatform();
                        if(((BreakablePlatform)platform).getState() == PlatformState.Delete)
                        {
                            toRemovePlatform.Add(platform);
                        }
                    }
                }
            }
        }

        _platforms.RemoveAll(platform => toRemovePlatform.Contains(platform));
        _character.removeHarpoons(toRemoveHarpoon);

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
                _ui.resetTimer();
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

    private void handleBallHit(Ball ball, ref List<Ball> toAddBall, ref List<Ball> toRemoveBall, ref List<PowerUp> toAddPowerUps)
    {
        _score++;
        _ui.UpdateScoreText(_score);
        toAddBall.AddRange(splitBall(ball));
        toRemoveBall.Add(ball);
        Ball.playPopSound();
        int rand = _powerUpRand.Next(0, 100);
        if (rand < LIVES_PROB)
        {
            toAddPowerUps.Add(new PowerUp(_livesSprite, ball.Position, powerUpType.LIVES));
        }
        else if (rand < FREEZE_PROB)
        {
            toAddPowerUps.Add(new PowerUp(_freezeSprite, ball.Position, powerUpType.CLOCK));
        }
        else if (rand < INVINCIBILITY_PROB)
        {
            toAddPowerUps.Add(new PowerUp(_invincibilitySprite, ball.Position, powerUpType.INVINCIBILITY));
        }
        else if (rand < BOMB_PROB)
        {
            toAddPowerUps.Add(new PowerUp(_bombSprite, ball.Position, powerUpType.BOMB));
        }

    }

    private bool areIntersecting(Circle circle, Rectangle rectangle)
    {
        int distanceX = Math.Abs(circle.X - rectangle.Center.X);
        int distanceY = Math.Abs(circle.Y - rectangle.Center.Y);

        float halfRectWidth = rectangle.Width * 0.5f;
        float halfRectHeight = rectangle.Height * 0.5f;

        if((distanceX > (halfRectWidth + circle.Radius)) ||
           (distanceY > (halfRectHeight + circle.Radius)))
        {
            return false;
        }

        if(distanceX <= halfRectWidth ||
           distanceY <= halfRectHeight)
        {
            return true;
        }

        double cornerDistanceSquare = Math.Pow(distanceX-halfRectWidth, 2) + Math.Pow(distanceY-halfRectHeight, 2);

        return cornerDistanceSquare <= Math.Pow(circle.Radius, 2);
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.White);

        // Draw the background
        _levelBackground.Draw();

        if (_state != GameState.Playing)
        {
            // We are in a game over state, so apply the saturation parameter.
            _grayscaleEffect.Parameters["Saturation"].SetValue(_saturation);

            // And begin the sprite batch using the grayscale effect.
            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: _grayscaleEffect);
        }
        else
        {
            // Begin the sprite batch to prepare for rendering.
            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        }

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

        foreach(PowerUp powerUp in _powerUps)
        {
            powerUp.Draw();
        }

        for(int livesIndex = 1; livesIndex <= _lives; livesIndex++)
        {
            int roomWidth = Core.GraphicsDevice.PresentationParameters.BackBufferWidth;
            float distanceFromRightWall = 5.0f;
            float distanceFromTopWall = 2.0f;
            float spaceBetweenSprites = 2.0f;

            Vector2 livesSpritePosition = new Vector2(roomWidth - (distanceFromRightWall + _livesSprite.Width + spaceBetweenSprites) * livesIndex, distanceFromTopWall);

            _livesSprite.Draw(Core.SpriteBatch, livesSpritePosition);
        }

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();

        // Draw the UI.
        _ui.Draw();

        base.Draw(gameTime);
    }

    private void LoadLevel(LevelConfig config)
    {
        _balls.Clear();
        _platforms.Clear();
        Ball.resetFreeze();
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
                case PlatformType.HORIZONTAL_GRAY:
                    _platforms.Add(new UnbreakablePlatform(new Sprite(_grayHorizontalPlatform.Region), platformSpawn.Position, platformType));
                break;
                case PlatformType.BREAKABLE_LARGE_HORIZONTAL_BLUE:
                    _platforms.Add(new BreakablePlatform(_horizontalBreakableBlueSprites, platformSpawn.Position, platformType, platformSpawn.platformState, _blockBreakEffect));
                    
                break;
            }
        }

        List<Texture2D> clouds = new List<Texture2D>();

        foreach(string backgroundStr in config.backgroundStr)
        {
            clouds.Add(Content.Load<Texture2D>(backgroundStr));
        }

        _levelBackground = new Background(clouds);
    }

    private List<Ball> splitBall(Ball ball)
    {
        List <Ball> toAddBall = new List<Ball>();

        BallSize ballSize = ball.GetBallSize();

        if(ballSize == BallSize.LARGE || ballSize == BallSize.MEDIUM)
        {
            BallType ballType = ball.GetBallType();
            float ballPositionX = ball.Position.X;

            switch(ballType)
            {
                case BallType.GREEN_ROUND:
                case BallType.RED_ROUND:
                case BallType.BLUE_ROUND:
                {    
                    Ball leftBall = new BouncingBall(ball.GetSprite(), ballSize-1, -1f, ballType);
                    Ball rightBall = new BouncingBall(ball.GetSprite(), ballSize-1, 1f, ballType);
                    leftBall.Position = new Vector2(ballPositionX - leftBall.getRadius(), ball.Position.Y);
                    rightBall.Position = new Vector2(ballPositionX + leftBall.getRadius(), ball.Position.Y);
                    toAddBall.Add(rightBall);
                    toAddBall.Add(leftBall);
                }
                break;
                case BallType.GREEN_SQUARED:
                {
                    Ball leftBall = new ReflectiveBall(ball.GetSprite(), ballSize-1, -1f, ballType);
                    Ball rightBall = new ReflectiveBall(ball.GetSprite(), ballSize-1, 1f, ballType);
                    leftBall.Position = new Vector2(ballPositionX - leftBall.getRadius(), ball.Position.Y);
                    rightBall.Position = new Vector2(ballPositionX + leftBall.getRadius(), ball.Position.Y);
                    toAddBall.Add(leftBall);
                    toAddBall.Add(rightBall);
                }
                break;
            }
        }

        return toAddBall;
    }

}
