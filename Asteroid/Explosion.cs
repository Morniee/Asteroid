using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroid
{
    class Explosion
    {
        #region Fields
        Texture2D _spriteStrip;
        Rectangle _drawRectangle, _srcRectangle;
        int _currentFrame, _elapsedFrameTime, _frameWidth, _frameHeight;
        bool _isPlaying, _isFinished;
        #endregion

        #region Constructors
        public Explosion(Texture2D spriteStrip, int x, int y, SoundEffect explosionSound)
        {
            _currentFrame = 0;
            _elapsedFrameTime = 0;
            _isFinished = false;
            _isPlaying = false;
            Initialize(spriteStrip);
            Play(x, y);
            explosionSound.Play();
        }
        #endregion

        #region Properties
        public bool IsFinished
        {
            get { return _isFinished; }
        }
        #endregion

        #region Public Methods
        public void Update(GameTime gameTime)
        {
            if (_isPlaying)
            {
                _elapsedFrameTime += gameTime.ElapsedGameTime.Milliseconds;
                if (_elapsedFrameTime > GameConstants.EXPLOSION_DURATION)
                {
                    _elapsedFrameTime = 0;

                    if (_currentFrame < GameConstants.EXPLOSION_AMOUNT_FRAMES - 1)
                    {
                        _currentFrame++;
                        SetSourceRectangleLocation(_currentFrame);
                    }
                    else
                    {
                        _isPlaying = false;
                        _isFinished = true;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isPlaying)
            {
                spriteBatch.Draw(_spriteStrip, _drawRectangle, _srcRectangle, Color.White);
            }
        }
        #endregion

        #region Private Methods
        private void Initialize(Texture2D spriteStrip)
        {
            _spriteStrip = spriteStrip;

            _frameWidth = _spriteStrip.Width / GameConstants.EXPLOSION_IMAGE_COLUMNS;
            _frameHeight = _spriteStrip.Height / GameConstants.EXPLOSION_IMAGE_ROWS;

            _drawRectangle = new Rectangle(0, 0, _frameWidth, _frameHeight);
            _srcRectangle = new Rectangle(0, 0, _frameWidth, _frameHeight);
        }

        private void Play(int x, int y)
        {
            _isPlaying = true;
            _elapsedFrameTime = 0;
            _currentFrame = 0;
            
            _drawRectangle.X = x - _drawRectangle.Width / 2;
            _drawRectangle.Y = y - _drawRectangle.Height / 2;
            SetSourceRectangleLocation(_currentFrame);
        }

        private void SetSourceRectangleLocation(int frameNumber)
        {
            _srcRectangle.X = (frameNumber % GameConstants.EXPLOSION_IMAGE_COLUMNS) * _frameWidth;
            _srcRectangle.Y = (frameNumber / GameConstants.EXPLOSION_IMAGE_ROWS) * _frameHeight;
        }
        #endregion
    }
}
