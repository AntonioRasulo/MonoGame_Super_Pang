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

    private List<Enemy> _enemies;

    private Rectangle _roomBounds;

    // The SpriteFont Description used to draw text.
    private SpriteFont _font;

    // Tracks the players score.
    private int _score;

    private int _currentLevelIndex;

    private CollectibleHandler _collectibleHandler;

    private GameSceneUI _ui;

    private GameState _state;

    // The grayscale shader effect.
    private Effect _grayscaleEffect;

    // The amount of saturation to provide the grayscale shader effect.
    private float _saturation = 1.0f;

    // The speed of the fade to grayscale effect.
    private const float FADE_SPEED = 0.02f;

    private SoundEffect _blockBreakEffect;

    private Background _levelBackground;

    private PlayerStats _pStats;

    public GameScene(int startingLevel, PlayerStats pStats)
    {
        _currentLevelIndex = startingLevel;
        _pStats = pStats;
    }

    public override void Initialize()
    {

        base.Initialize();

        // During the game scene, we want to disable exit on escape. Instead,
        // the escape key will be used to return back to the title screen
        Core.ExitOnEscape = false;

        _roomBounds = Core.GraphicsDevice.PresentationParameters.Bounds;

        // Create any UI elements from the root element created in previous
        // scenes.
        GumService.Default.Root.Children.Clear();

        // Initialize the user interface for the game scene.
        InitializeUI();

        // Initialize a new game to be played.
        InitializeNewGame();

        _collectibleHandler = new CollectibleHandler();
    }

    private void InitializeUI()
    {
        // Clear out any previous UI element incase we came here
        // from a different scene.
        GumService.Default.Root.Children.Clear();

        // Create the game scene ui instance.
        _ui = new GameSceneUI();
        _ui.UpdateLivesText(_character.getLives());
        _ui.UpdateMoneyText(_pStats.Money);

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
        PlayerStats.SaveGame(_character._pStats);
        // Player has chosen to quit, so return back to the title scene.
        Core.ChangeScene(new TitleScene());
    }

    public override void LoadContent()
    {
        // Load the background theme music
        Song theme = Content.Load<Song>("audio/16. Battle Theme III (loop)");
        Core.Audio.PlaySong(theme);

        // Load ball content
        Ball.LoadContent();

        // Load platform content
        Platform.LoadContent();

        _character = new Character(_pStats);

        _balls = new List<Ball>();

        _platforms = new List<Platform>();

        _enemies = new List<Enemy>();

        // Load the font
        _font = Content.Load<SpriteFont>("fonts/04B_30");

        _blockBreakEffect = Content.Load<SoundEffect>("audio/Block Break 1");

        LoadLevel(LevelRegistry.AllLevels[_currentLevelIndex]);

        // Load the grayscale effect.
        _grayscaleEffect = Content.Load<Effect>("effects/grayscaleEffect");

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

        Ball.updateFreeze(gameTime);

        foreach(Ball ball in _balls)
        {
            ball.Update(gameTime);
        }

        _collectibleHandler.Update(gameTime);

        foreach(Enemy enemy in _enemies)
        {
            enemy.Update(gameTime);
        }

        CollisionChecks();

        checkChangeScene();

        List<Enemy> toRemoveEnemies = new List<Enemy>();
        foreach(Enemy enemy in _enemies)
        {
            if (enemy.isToRemove())
            {
                toRemoveEnemies.Add(enemy);
            }
        }
        _enemies.RemoveAll(enemy => toRemoveEnemies.Contains(enemy));

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

        var toRemoveCollectibles = new List<Collectible>();
        var toAddBall = new List<Ball>();
        var toRemoveBall = new List<Ball>();

        /* Character - Collectible collision */
        collectibleType collectibleCollided = _collectibleHandler.CheckCharacterCollision(characterBounds);

        switch (collectibleCollided)
        {
            case collectibleType.LIVES:
                _character.increaseLives();
                _ui.UpdateLivesText(_character.getLives());
                break;
            case collectibleType.CLOCK:
                Ball.Freeze();
                break;
            case collectibleType.INVINCIBILITY:
                {
                    _character.activateImmunity(true);
                }
                break;
            case collectibleType.BOMB:
                {
                    foreach(Ball ball in _balls)
                    {
                        handleBallHit(ball, ref toAddBall, ref toRemoveBall);
                    }
                    _balls.AddRange(toAddBall);
                    _balls.RemoveAll(ball => toRemoveBall.Contains(ball));
                }
                break;
            case collectibleType.GOLD_COIN:
            case collectibleType.SILVER_COIN:
            case collectibleType.BRONZE_COIN:
                _character._pStats.Money += Coin.GetValue(collectibleCollided);
                _ui.UpdateMoneyText(_character._pStats.Money);
                break;
        }

        toAddBall.Clear();
        toRemoveBall.Clear();
        var toRemoveHarpoon = new List<Harpoon>();

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
                    handleBallHit(ball, ref toAddBall, ref toRemoveBall);
                    toRemoveHarpoon.Add(harpoon);
                }
            }
        }

        _balls.RemoveAll(ball => toRemoveBall.Contains(ball));
        _balls.AddRange(toAddBall);
        _character.removeHarpoons(toRemoveHarpoon);

        /* Harpoon - Enemies collision check */
        toRemoveHarpoon.Clear();

        foreach(Harpoon harpoon in _character.getHarpoons())
        {
            // If the harpoon is on the remove list
            if(toRemoveHarpoon.Contains(harpoon)) continue;
            Rectangle harpoonBounds = harpoon.getBounds();
            foreach(Enemy enemy in _enemies)
            {
                Rectangle enemyBounds = enemy.GetBounds();
                if(harpoonBounds.Intersects(enemyBounds))
                {
                    _score += enemy.TakeHit();
                    _ui.UpdateScoreText(_score);
                    toRemoveHarpoon.Add(harpoon);
                }
            }
        }

        _character.removeHarpoons(toRemoveHarpoon);

        /* Enemies character collision */
        foreach(Enemy enemy in _enemies)
        {
            Rectangle enemyBounds = enemy.GetBounds();
            if (characterBounds.Intersects(enemyBounds) && (_character.IsImmune == false))
            {
                _score--;
                _character.activateImmunity();
                _ui.UpdateLivesText(_character.getLives());
                // Update the score display on the UI.
                _ui.UpdateScoreText(_score);
                if(_character.isAlive() == false)
                    return;
            }
        }

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
                _character.activateImmunity();
                _ui.UpdateLivesText(_character.getLives());
                // Update the score display on the UI.
                _ui.UpdateScoreText(_score);
                if(_character.isAlive() == false)
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
            _score -= _ui.getTimer();
            if(_score < 0)
                _score = 0;
            _ui.UpdateScoreText(_score);
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
            PlayerStats.SaveGame(_character._pStats);
        }
        else if(_character.isAlive() == false)
        {
            _score -= _ui.getTimer();
            if(_score < 0)
                _score = 0;
            _ui.UpdateScoreText(_score);
            Core.ChangeScene(new GameOver(_score));
            PlayerStats.SaveGame(_character._pStats);
        }
    }

    private void handleBallHit(Ball ball, ref List<Ball> toAddBall, ref List<Ball> toRemoveBall)
    {
        _score += ball.getScore();
        _ui.UpdateScoreText(_score);
        toAddBall.AddRange(splitBall(ball));
        toRemoveBall.Add(ball);
        Ball.playPopSound();
        _collectibleHandler.GenerateCollectible(ball.Position);
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

        _collectibleHandler.Draw();

        foreach(Enemy enemy in _enemies)
        {
            enemy.Draw();
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
        _enemies.Clear();
        Ball.resetFreeze();
        foreach (var spawnConfig in config.Balls)
        {
            BallType ballType = spawnConfig.BallType;
            switch(spawnConfig.BallType)
            {
                case BallType.GREEN_ROUND:
                case BallType.RED_ROUND:
                case BallType.BLUE_ROUND:
                    _balls.Add(new BouncingBall(spawnConfig.Size, spawnConfig.DirectionX, ballType, spawnConfig.Position));
                break;
                case BallType.GREEN_SQUARED:
                    _balls.Add(new ReflectiveBall(spawnConfig.Size, spawnConfig.DirectionX, ballType, spawnConfig.Position));
                break;
            }
        }

        foreach(var platformSpawn in config.Platforms)
        {
            PlatformType platformType = platformSpawn.platformType;
            switch (platformType)
            {
                case PlatformType.HORIZONTAL_GRAY:
                    _platforms.Add(new UnbreakablePlatform(platformSpawn.Position, platformType));
                break;
                case PlatformType.BREAKABLE_LARGE_HORIZONTAL_BLUE:
                    _platforms.Add(new BreakablePlatform(platformSpawn.Position, platformType, platformSpawn.platformState, _blockBreakEffect));
                    
                break;
            }
        }

        List<Texture2D> clouds = new List<Texture2D>();

        foreach(string backgroundStr in config.backgroundStr)
        {
            clouds.Add(Content.Load<Texture2D>(backgroundStr));
        }

        _levelBackground = new Background(clouds);

        _enemies = LevelGenerator.generateEnemies();
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
                    Ball leftBall = new BouncingBall(ballSize-1, -1f, ballType);
                    Ball rightBall = new BouncingBall(ballSize-1, 1f, ballType);
                    leftBall.Position = new Vector2(ballPositionX - leftBall.getRadius(), ball.Position.Y);
                    rightBall.Position = new Vector2(ballPositionX + leftBall.getRadius(), ball.Position.Y);
                    toAddBall.Add(rightBall);
                    toAddBall.Add(leftBall);
                }
                break;
                case BallType.GREEN_SQUARED:
                {
                    Ball leftBall = new ReflectiveBall(ballSize-1, -1f, ballType);
                    Ball rightBall = new ReflectiveBall(ballSize-1, 1f, ballType);
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
