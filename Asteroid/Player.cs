using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
    public class Player
    {
        #region Fields
        SpriteFont _font;
        Vector2 _position;
        KeyboardState _oldKeyboardState;
        string _name;
        int _index;
        #endregion

        #region Constructors
        public Player(ContentManager content, string fontName, Vector2 position)
        {
            _name = "___";
            _index = 0;
            _position = position;
            LoadContent(content, fontName);
        }
        #endregion

        #region Properties
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        #endregion

        #region Public Methods
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, _name, _position, Color.White);
        }

        public void Update(GameState gameState, KeyboardState currentKeyboardState)
        {
            if(gameState == GameState.NAME_ENTRY)
            {
                #region Alphabet Detection
                if(_index < 3)
                {
                    if (currentKeyboardState.IsKeyUp(Keys.A) && _oldKeyboardState.IsKeyDown(Keys.A))
                    {
                        SetCharacter('A');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.B) && _oldKeyboardState.IsKeyDown(Keys.B))
                    {
                        SetCharacter('B');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.C) && _oldKeyboardState.IsKeyDown(Keys.C))
                    {
                        SetCharacter('C');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.D) && _oldKeyboardState.IsKeyDown(Keys.D))
                    {
                        SetCharacter('D');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.E) && _oldKeyboardState.IsKeyDown(Keys.E))
                    {
                        SetCharacter('E');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.F) && _oldKeyboardState.IsKeyDown(Keys.F))
                    {
                        SetCharacter('F');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.G) && _oldKeyboardState.IsKeyDown(Keys.G))
                    {
                        SetCharacter('G');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.H) && _oldKeyboardState.IsKeyDown(Keys.H))
                    {
                        SetCharacter('H');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.I) && _oldKeyboardState.IsKeyDown(Keys.I))
                    {
                        SetCharacter('I');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.J) && _oldKeyboardState.IsKeyDown(Keys.J))
                    {
                        SetCharacter('J');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.K) && _oldKeyboardState.IsKeyDown(Keys.K))
                    {
                        SetCharacter('K');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.L) && _oldKeyboardState.IsKeyDown(Keys.L))
                    {
                        SetCharacter('L');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.M) && _oldKeyboardState.IsKeyDown(Keys.M))
                    {
                        SetCharacter('M');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.N) && _oldKeyboardState.IsKeyDown(Keys.N))
                    {
                        SetCharacter('N');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.O) && _oldKeyboardState.IsKeyDown(Keys.O))
                    {
                        SetCharacter('O');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.P) && _oldKeyboardState.IsKeyDown(Keys.P))
                    {
                        SetCharacter('P');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.Q) && _oldKeyboardState.IsKeyDown(Keys.Q))
                    {
                        SetCharacter('Q');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.R) && _oldKeyboardState.IsKeyDown(Keys.R))
                    {
                        SetCharacter('R');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.S) && _oldKeyboardState.IsKeyDown(Keys.S))
                    {
                        SetCharacter('S');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.T) && _oldKeyboardState.IsKeyDown(Keys.T))
                    {
                        SetCharacter('T');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.U) && _oldKeyboardState.IsKeyDown(Keys.U))
                    {
                        SetCharacter('U');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.V) && _oldKeyboardState.IsKeyDown(Keys.V))
                    {
                        SetCharacter('V');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.W) && _oldKeyboardState.IsKeyDown(Keys.W))
                    {
                        SetCharacter('W');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.X) && _oldKeyboardState.IsKeyDown(Keys.X))
                    {
                        SetCharacter('X');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.Y) && _oldKeyboardState.IsKeyDown(Keys.Y))
                    {
                        SetCharacter('Y');
                    }

                    if (currentKeyboardState.IsKeyUp(Keys.Z) && _oldKeyboardState.IsKeyDown(Keys.Z))
                    {
                        SetCharacter('Z');
                    }
                }
               
                #endregion

                #region Remove Characters
                if (currentKeyboardState.IsKeyUp(Keys.Back) && _oldKeyboardState.IsKeyDown(Keys.Back) && _index > 0)
                {
                    RemoveCharacter();
                }
                #endregion

                #region Confirm Name
                if(_index == 3 && currentKeyboardState.IsKeyUp(Keys.Enter) && _oldKeyboardState.IsKeyDown(Keys.Enter))
                {
                    Game1.SetStart(true);
                }
                #endregion

                _oldKeyboardState = currentKeyboardState;
            }
        }
        #endregion

        #region Private Methods
        private void LoadContent(ContentManager content, string fontName)
        {
            _font = content.Load<SpriteFont>(@"fonts/" + fontName);
            _position = _position - new Vector2(_font.MeasureString(_name).X, _font.MeasureString(_name).Y);
        }

        private void SetCharacter(char letter)
        {
            char[] characters = _name.ToCharArray();
            characters[_index] = letter;
            _name = characters[0].ToString() + characters[1].ToString() + characters[2].ToString();
            _index++;
        }

        private void RemoveCharacter()
        {
            _index--;
            char[] characters = _name.ToCharArray();
            characters[_index] = '_';
            _name = characters[0].ToString() + characters[1].ToString() + characters[2].ToString();
        }
        #endregion
    }
}
