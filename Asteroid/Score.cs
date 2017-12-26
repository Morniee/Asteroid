using Microsoft.Xna.Framework;
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
    public class Score
    {
        #region Fields
        int _currentScore;
        Texture2D[] _sprites = new Texture2D[10];
        #endregion

        #region Constructors
        public Score(ContentManager content, string[] spriteNames)
        {
            _currentScore = 0;
            LoadContent(content, spriteNames);
            
        }
        #endregion

        #region Properties
        public int CurrentScore
        {
            get { return _currentScore; }
            set { _currentScore = value; }
        }
        #endregion

        #region Public Methods
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_currentScore.ToString().Length < 2)
            {
                Rectangle drawRectangle = new Rectangle(GameConstants.WINDOW_WIDTH - GameConstants.SCORE_POSITION_X - _sprites[_currentScore].Width, GameConstants.SCORE_POSITION_Y, _sprites[_currentScore].Width, _sprites[_currentScore].Height);
                spriteBatch.Draw(_sprites[_currentScore], drawRectangle, Color.White);
            }
            else
            {
                char[] numbers = _currentScore.ToString().ToCharArray();
                for (int i = numbers.Length - 1; i >= 0; i--)
                {
                    Rectangle drawRectangle = new Rectangle(GameConstants.WINDOW_WIDTH - GameConstants.SCORE_POSITION_X - _sprites[int.Parse(numbers[i].ToString())].Width * (numbers.Length - i), GameConstants.SCORE_POSITION_Y, _sprites[int.Parse(numbers[i].ToString())].Width, _sprites[int.Parse(numbers[i].ToString())].Height);
                    spriteBatch.Draw(_sprites[int.Parse(numbers[i].ToString())], drawRectangle, Color.White);
                }
            }
        }

        public void Reset()
        {
            _currentScore = 0;
        }
        #endregion

        #region Private Methods
        private void LoadContent(ContentManager content, string[] spriteNames)
        {
            for (int i = 0; i < spriteNames.Length; i++)
            {
                _sprites[i] = content.Load<Texture2D>(@"graphics/" + spriteNames[i]);
            }

        }
        #endregion
    }
}
