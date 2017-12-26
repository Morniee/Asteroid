using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroid
{
    /// <summary>
    /// 
    /// </summary>
    public class Ship
    {
        #region Fields
        Texture2D _sprite;
        Circle _collisionCircle;
        Vector2 _velocity;
        SoundEffect _shootSound;
        int _health, _angle, _elapsedShotMilliseconds;
        float _speed;
        bool _isAlive, _canShoot;

        Vector2 _position;

        #endregion

        #region Constructors
        public Ship(ContentManager content, string spriteName, Vector2 position)
        {
            _position = position;
            _angle = 0;
            _velocity = Vector2.Zero;
            _speed = 0;
            _elapsedShotMilliseconds = 0;
            _isAlive = false;
            _canShoot = true;
            _health = 100;
            LoadContent(content, spriteName, position);
        }
        #endregion

        #region Properties
        public Circle CollisionCircle
        {
            get { return _collisionCircle; }
        }
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public bool IsAlive
        {
            get { return _isAlive; }
            set { _isAlive = value; }
        }

        public int Width
        {
            get { return _sprite.Width; }
        }

        public int Height
        {
            get { return _sprite.Height; }
        }

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }
        #endregion

        #region Public Methods
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(Width / 2, Height / 2);
            spriteBatch.Draw(_sprite, _position, null, Color.White, MathHelper.DegreesToRadians(_angle), origin, 1f, SpriteEffects.None, 1);
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, GameState gameState)
        {
            if(_isAlive && gameState == GameState.PLAYING)
            {
                #region Movement Support
                if (currentKeyboardState.IsKeyDown(Keys.Left))
                {
                    TurnLeft();
                }

                if (currentKeyboardState.IsKeyDown(Keys.Right))
                {
                    TurnRight();
                } 

                if(currentKeyboardState.IsKeyDown(Keys.Up))
                {
                    Accelerate(gameTime);
                } else {
                    Decellerate(gameTime);
                }

                Move(gameTime);
                #endregion

                #region Shoot Support
                if (currentKeyboardState.IsKeyDown(Keys.Space) && _canShoot)
                {
                    _canShoot = false;
                    Projectile newProjectile = new Projectile(
                        Game1.GetProjectileSprite(),
                        MathHelper.CalculateDirection(_angle, GameConstants.PROJECTILE_SPEED),
                        _position);
                    Game1.AddProjectile(newProjectile);
                    _shootSound.Play();
                }

                if (!_canShoot)
                {
                    _elapsedShotMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                    if (_elapsedShotMilliseconds % GameConstants.SHIP_FIRING_DELAY == 0)
                    {
                        _canShoot = true;
                    }
                }
                #endregion
            }

            _collisionCircle.Center = new Vector2(_position.X, _position.Y);
        }

        public void Reset()
        {
            _position = new Vector2(GameConstants.WINDOW_WIDTH / 2 - Width / 2, GameConstants.WINDOW_HEIGHT / 2 - Height / 2);
            _velocity = Vector2.Zero;
            _speed = 0;
            _angle = 0;
            _health = 100;
        }
        #endregion

        #region Private Methods
        private void LoadContent(ContentManager content, string spriteName, Vector2 position)
        {
            _sprite = content.Load<Texture2D>(@"graphics/" + spriteName);
            _shootSound = content.Load<SoundEffect>(@"audio/Shoot");
            _collisionCircle = new Circle(_position.X, _position.Y, GameConstants.SHIP_RADIUS);
            _position.X -= Width / 2;
            _position.Y -= Height / 2;
        }

        private void TurnLeft()
        {
            _angle -= 3;
        }

        private void TurnRight()
        {
            _angle += 3;
        }

        private void Accelerate(GameTime gameTime)
        {
            _speed += _speed * GameConstants.SHIP_ACCELERATION;

            if (_speed > GameConstants.SHIP_MAX_SPEED)
            {
                _speed = GameConstants.SHIP_MAX_SPEED;
            }
        }

        private void Decellerate(GameTime gameTime)
        {
            _speed -= _speed * GameConstants.SHIP_ACCELERATION;

            if (_speed < GameConstants.SHIP_BASE_SPEED)
            {
                _speed = GameConstants.SHIP_BASE_SPEED;
            }
        }

        public void Move(GameTime gameTime)
        {
            _position.X += _velocity.X;
            _position.Y += _velocity.Y;

            _velocity.X = MathHelper.CalculateDirection(_angle, _speed).X * gameTime.ElapsedGameTime.Milliseconds;
            _velocity.Y = MathHelper.CalculateDirection(_angle, _speed).Y * gameTime.ElapsedGameTime.Milliseconds;
        }

        #endregion
    }
}
