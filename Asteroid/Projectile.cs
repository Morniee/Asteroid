using Microsoft.Xna.Framework;
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
    public class Projectile
    {
        #region Fields
        Texture2D _sprite;
        Rectangle _drawRectangle;
        Vector2 _velocity;
        bool _isActive;
        #endregion

        #region Constructors
        public Projectile(Texture2D sprite, Vector2 velocity, Vector2 position)
        {
            _sprite = sprite;
            _velocity = velocity;
            _isActive = true;
            _drawRectangle = new Rectangle((int)position.X, (int)position.Y, _sprite.Width / 2, _sprite.Height / 2);
        }
        #endregion

        #region Properties
        public Rectangle CollisionRectangle
        {
            get { return _drawRectangle; }
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
            spriteBatch.Draw(_sprite, _drawRectangle, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            _drawRectangle.X += (int)(_velocity.X * gameTime.ElapsedGameTime.Milliseconds);
            _drawRectangle.Y += (int)(_velocity.Y * gameTime.ElapsedGameTime.Milliseconds);

            CheckOutOfBounds();
        }
        #endregion

        #region Private Methods
        private void CheckOutOfBounds()
        {
            if(_drawRectangle.X < 0 ||
               _drawRectangle.Y < 0 ||
               _drawRectangle.X + _drawRectangle.Width > GameConstants.WINDOW_WIDTH ||
               _drawRectangle.Y + _drawRectangle.Height > GameConstants.WINDOW_HEIGHT)
            {
                IsActive = false;
            }
        }
        #endregion
    }
}
