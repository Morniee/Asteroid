using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroid
{
    /// <summary>
    /// 
    /// </summary>
    public class Asteroid
    {
        #region Fields
        Texture2D _spriteStrip;
        Rectangle _drawRectangle, _srcRectangle;
        Vector2 _velocity;
        AsteroidSize _size;
        SoundEffect _bounceSound;

        int _frameWidth, _frameHeight, _health;
        bool _isActive;
        #endregion

        #region Constructors
        public Asteroid(Texture2D spriteStrip, Vector2 position, Vector2 velocity, SoundEffect bounceSound)
        {
            _spriteStrip = spriteStrip;
            _velocity = velocity;
            _bounceSound = bounceSound;
            _isActive = true;
            GetRandomAsteroidSize();
            SetHealthBySize();
            SetRectangles(position);
        }

        public Asteroid(Texture2D spriteStrip, AsteroidSize size, Vector2 position, Vector2 velocity, SoundEffect bounceSound)
        {
            _spriteStrip = spriteStrip;
            _velocity = velocity;
            _bounceSound = bounceSound;
            _size = size;
            _isActive = true;
            SetHealthBySize();
            SetRectangles(position);
        }
        #endregion

        #region Properties
        public Rectangle CollisionRectangle
        {
            get { return _drawRectangle; }
        }

        public AsteroidSize Size
        {
            get { return _size; }
        }

        public Vector2 Position
        {
            get { return new Vector2(_drawRectangle.X, _drawRectangle.Y); }
        }

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }
        #endregion

        #region Public Methods
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_spriteStrip, _drawRectangle, _srcRectangle, Color.White);
        }

        public void Update(GameTime gameTime, GameState gameState)
        {
            _drawRectangle.X += (int)(_velocity.X * gameTime.ElapsedGameTime.Milliseconds);
            _drawRectangle.Y += (int)(_velocity.Y * gameTime.ElapsedGameTime.Milliseconds);
            Bounce();
        }
        #endregion

        #region Private Methods

        private void GetRandomAsteroidSize()
        {
            int rnd = RandomNumberGenerator.Next(3);

            switch (rnd)
            {
                case 0:
                    _size = AsteroidSize.LARGE;
                    break;
                case 1:
                    _size = AsteroidSize.MEDIUM;
                    break;
                default:
                    _size = AsteroidSize.SMALL;
                    break;
            }
        }

        private void SetRectangles(Vector2 position)
        {
            _frameWidth = _spriteStrip.Width / GameConstants.ASTEROID_IMAGESPARROW;
            _frameHeight = _spriteStrip.Height;
            _srcRectangle = new Rectangle(_frameWidth * RandomNumberGenerator.Next(3), 0, _frameWidth, _frameHeight);

            switch (_size)
            {
                case AsteroidSize.LARGE:
                    _drawRectangle = new Rectangle((int)position.X, (int)position.Y, _frameWidth, _frameHeight);
                    break;
                case AsteroidSize.MEDIUM:
                    _drawRectangle = new Rectangle((int)position.X, (int)position.Y, _frameWidth / 2, _frameHeight / 2);
                    break;
                case AsteroidSize.SMALL:
                    _drawRectangle = new Rectangle((int)position.X, (int)position.Y, _frameWidth / 3, _frameHeight / 3);
                    break;
            }

            
        }

        private void SetHealthBySize()
        {
            switch (_size)
            {
                case AsteroidSize.LARGE:
                    _health = GameConstants.ASTEROID_LARGE_HEALTH;
                    break;
                case AsteroidSize.MEDIUM:
                    _health = GameConstants.ASTEROID_MEDIUM_HEALTH;
                    break;
                case AsteroidSize.SMALL:
                    _health = GameConstants.ASTEROID_SMALL_HEALTH;
                    break;
            }
        }

        private void Bounce()
        {

            if (_drawRectangle.X < 0)
            {
                _drawRectangle.X = 0;
                _velocity.X *= -1;
                _bounceSound.Play();
            }
            else if ((_drawRectangle.X + _drawRectangle.Width) > GameConstants.WINDOW_WIDTH)
            {
                _drawRectangle.X = GameConstants.WINDOW_WIDTH - _drawRectangle.Width;
                _velocity.X *= -1;
                _bounceSound.Play();
            }

            if (_drawRectangle.Y < 0)
            {
                _drawRectangle.Y = 0;
                _velocity.Y *= -1;
                _bounceSound.Play();
            }
            else if ((_drawRectangle.Y + _drawRectangle.Height) > GameConstants.WINDOW_HEIGHT)
            {
                _drawRectangle.Y = GameConstants.WINDOW_HEIGHT - _drawRectangle.Height;
                _velocity.Y *= -1;
                _bounceSound.Play();
            }


        }
        #endregion
    }
}
