using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using MonoGameLibrary;
using System;

namespace MonoGame_Super_Pang.GameObjects;

public class BigBat : Bat
{
    private const int NUM_LIVES = 3;
    private const int ENEMY_SCORE = 7;
    private const float MOVEMENT_SPEED = 200f;

    private Vector2 _target;

    private Random _positionRand;

    private readonly int minPosX;
    private readonly int maxPosX;
    private readonly int minPosY;
    private readonly int maxPosY;

    public BigBat(Vector2 position): base(position)
    {
        _hurtAnimation.Scale = new Vector2(SCALE, SCALE);
        _hurtAnimation.CenterOrigin();

        _lives = NUM_LIVES;
        _score = ENEMY_SCORE;
        _movementSpeed = MOVEMENT_SPEED;

        _positionRand = new Random();

        minPosX = (int)(_idleAnimation.Width * 0.5f);
        minPosY = (int)(_idleAnimation.Height * 0.5f);
        maxPosX = Core.GraphicsDevice.PresentationParameters.BackBufferWidth - (int)(_idleAnimation.Width * 0.5f);
        maxPosY = Core.GraphicsDevice.PresentationParameters.BackBufferHeight - (int)(_idleAnimation.Height * 0.5f);

        UpdateTargetPosition();

    }

    private void UpdateTargetPosition()
    {
        int targetY = (int)_position.Y;
        while(!(targetY > _position.Y + _landAnimation.Height*0.5f || targetY < _position.Y - _landAnimation.Height*0.5f))
        {
            targetY = _positionRand.Next(minPosY, maxPosY);
        }

        int targetX = _positionRand.Next(minPosX, maxPosX);
        
        _target = new Vector2(targetX, targetY);
    }

    protected override void UpdateMovement(GameTime gameTime)
    {
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Vector2 direction = _target - _position;
        float distance = direction.Length();

        if(distance <= _movementSpeed * delta)
        {
            _position = _target;
            UpdateTargetPosition();
        }
        else
        {
            direction.Normalize();
            _position += direction * _movementSpeed * delta;
        }
    }

    protected override void LoadContent()
    {
        TextureAtlas bigBatAtlas = TextureAtlas.FromFile(Core.Content, "images/enemies/bat_atlas.xml");

        _idleAnimation = bigBatAtlas.CreateAnimatedSprite("idle-animation");
        _hurtAnimation = bigBatAtlas.CreateAnimatedSprite("hurt-animation");
        _fallAnimation = bigBatAtlas.CreateAnimatedSprite("fall-animation");
        _landAnimation = bigBatAtlas.CreateAnimatedSprite("land-animation");
        _deathSprite = bigBatAtlas.CreateSprite("land5");

    }

}
