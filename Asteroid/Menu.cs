using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Asteroid
{
    /// <summary>
    /// 
    /// </summary>
    public class Menu
    {
        #region Fields
        Texture2D _background, _backgroundGameOver, _startButton, _resumeButton, _controlsButton, _leaderboardsButton, _playAgainButton, _mainMenuButton, _controls;
        Vector2 _startResumePosition, _controlsPosition, _leaderboardsPosition, _playAgainPosition, _mainMenuPosition;
        SpriteFont _font;
        GameState _gameState;
        bool _showControls, _showLeaderboards;
        #endregion

        #region Constructors
        public Menu(ContentManager content)
        {
            _gameState = GameState.STARTUP;
            _showControls = true;
            _showLeaderboards = false;
            LoadContent(content);
        }
        #endregion

        #region Properties
        public GameState Gamestate {
            get { return _gameState; }
            set { _gameState = value; }
        }

        public bool ShowControls
        {
            get { return _showControls; }
            set { _showControls = value; }
        }

        public bool ShowLeaderboards
        {
            get { return _showLeaderboards; }
            set { _showLeaderboards = value; }
        }
        #endregion

        #region Public Methods
        public void Draw(SpriteBatch spriteBatch)
        {
            if(_gameState != GameState.PLAYING && _gameState != GameState.GAMEOVER && _gameState != GameState.NAME_ENTRY)
            {
                spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            }

            if(_gameState == GameState.GAMEOVER)
            {
                spriteBatch.Draw(_backgroundGameOver, Vector2.Zero, Color.White);
                spriteBatch.Draw(_playAgainButton, _playAgainPosition, Color.White);
                spriteBatch.Draw(_mainMenuButton, _mainMenuPosition, Color.White);
            }
           
            if(_gameState == GameState.STARTUP)
            {
                spriteBatch.Draw(_startButton, _startResumePosition , Color.White);
                spriteBatch.Draw(_controlsButton, _controlsPosition, Color.White);
                spriteBatch.Draw(_leaderboardsButton, _leaderboardsPosition, Color.White);
            }

            if (_gameState == GameState.NAME_ENTRY)
            {
                Vector2 nameCommandPos = new Vector2(
                    GameConstants.WINDOW_WIDTH / 2 - _font.MeasureString(GameConstants.MENU_ENTER_NAME_COMMAND).X / 2,
                    GameConstants.WINDOW_HEIGHT / 2 - _font.MeasureString(GameConstants.MENU_ENTER_NAME_COMMAND).Y / 2- GameConstants.MENU_ENTER_NAME_COMMAND_MARGIN);
                spriteBatch.DrawString(_font, GameConstants.MENU_ENTER_NAME_COMMAND, nameCommandPos, Color.White);
            }

            if (_gameState == GameState.PAUSED)
            {
                spriteBatch.Draw(_resumeButton, _startResumePosition, Color.White);
                spriteBatch.Draw(_controlsButton, _controlsPosition, Color.White);
                spriteBatch.Draw(_leaderboardsButton, _leaderboardsPosition, Color.White);
            }

            if(_showControls && _gameState != GameState.PLAYING && _gameState != GameState.GAMEOVER)
            {
                spriteBatch.Draw(_controls, new Vector2(GameConstants.WINDOW_WIDTH / 2, GameConstants.MENU_BUTTON_AREA_Y), Color.White);
            }

            if (_showLeaderboards && _gameState != GameState.PLAYING && _gameState != GameState.GAMEOVER)
            {
                HighScoreData data = HighScoreData.LoadHighScores();
                for (int i = 0; i < data.Count; i++)
                {
                    spriteBatch.DrawString(_font, data.PlayerName[i], new Vector2(GameConstants.WINDOW_WIDTH / 2, GameConstants.MENU_LEADERBOARD_Y + GameConstants.MENU_LEADERBOARD_MARGIN_Y * i), Color.White);
                    spriteBatch.DrawString(_font, data.Score[i].ToString(), new Vector2(GameConstants.WINDOW_WIDTH / 2 + GameConstants.MENU_LEADERBOARD_MARGIN_X, GameConstants.MENU_LEADERBOARD_Y + GameConstants.MENU_LEADERBOARD_MARGIN_Y * i), Color.White);
                }
                
            }
        }

        public void Update(MouseState mouseState)
        {
            if(mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 mousePosition = mouseState.Position.ToVector2();
             
                #region Start / Resume Support
                if (mousePosition.X > _startResumePosition.X &&
                       mousePosition.X < _startResumePosition.X + _startButton.Width &&
                       mousePosition.Y > _startResumePosition.Y &&
                       mousePosition.Y < _startResumePosition.Y + _startButton.Height)
                {
                    if(_gameState == GameState.STARTUP)
                    {
                        _gameState = GameState.NAME_ENTRY;
                        _showControls = false;
                        _showLeaderboards = false;
                    } else if( _gameState == GameState.PAUSED)
                    {
                        _gameState = GameState.PLAYING;
                    }
                    
                }
                #endregion

                #region Leaderboard & Controls Support
                if (_gameState == GameState.STARTUP || _gameState == GameState.PAUSED)
                {

                    if (mousePosition.X > _controlsPosition.X &&
                       mousePosition.X < _controlsPosition.X + _controlsButton.Width &&
                       mousePosition.Y > _controlsPosition.Y &&
                       mousePosition.Y < _controlsPosition.Y + _controlsButton.Height)
                    {
                        _showControls = true;
                        _showLeaderboards = false;
                    }

                    if (mousePosition.X > _leaderboardsPosition.X &&
                       mousePosition.X < _leaderboardsPosition.X + _leaderboardsButton.Width &&
                       mousePosition.Y > _leaderboardsPosition.Y &&
                       mousePosition.Y < _leaderboardsPosition.Y + _leaderboardsButton.Height)
                    {
                        _showControls = false;
                        _showLeaderboards = true;
                    }
                }
                #endregion

                #region Game Over Menu Support
                if (_gameState == GameState.GAMEOVER)
                {
                    if (mousePosition.X > _playAgainPosition.X &&
                       mousePosition.X < _playAgainPosition.X + _playAgainButton.Width &&
                       mousePosition.Y > _playAgainPosition.Y &&
                       mousePosition.Y < _playAgainPosition.Y + _playAgainButton.Height)
                    {
                        _gameState = GameState.PLAYING;
                        Game1.SetRestart();
                    }

                    if (mousePosition.X > _mainMenuPosition.X &&
                       mousePosition.X < _mainMenuPosition.X + _mainMenuButton.Width &&
                       mousePosition.Y > _mainMenuPosition.Y &&
                       mousePosition.Y < _mainMenuPosition.Y + _mainMenuButton.Height)
                    {
                        _gameState = GameState.STARTUP;
                        Game1.SetStart(false);
                    }
                }
                #endregion
            }
        }
        #endregion

        #region Private Methods
        private void LoadContent(ContentManager content)
        {
            _background = content.Load<Texture2D>(@"graphics/Background");
            _backgroundGameOver = content.Load<Texture2D>(@"graphics/BackgroundGameOver");
            _controls = content.Load<Texture2D>(@"graphics/ControlsPanel");
            _startButton = content.Load<Texture2D>(@"graphics/StartButton");
            _resumeButton = content.Load<Texture2D>(@"graphics/ResumeButton");
            _controlsButton = content.Load<Texture2D>(@"graphics/ControlsButton");
            _leaderboardsButton = content.Load<Texture2D>(@"graphics/LeaderboardsButton");
            _playAgainButton = content.Load<Texture2D>(@"graphics/PlayAgainButton");
            _mainMenuButton = content.Load<Texture2D>(@"graphics/MainMenuButton");
            _font = content.Load<SpriteFont>(@"fonts/baseFont");

            _startResumePosition = new Vector2(GameConstants.MENU_BUTTON_AREA_X, GameConstants.MENU_BUTTON_AREA_Y);
            _controlsPosition = _startResumePosition + new Vector2(0, GameConstants.MENU_BUTTON_MARGIN + _startButton.Height);
            _leaderboardsPosition = _controlsPosition + new Vector2(0, GameConstants.MENU_BUTTON_MARGIN + _controlsButton.Height);
            _playAgainPosition = new Vector2((GameConstants.WINDOW_WIDTH - _playAgainButton.Width) / 2, _leaderboardsPosition.Y);
            _mainMenuPosition = _playAgainPosition + new Vector2(0, GameConstants.MENU_BUTTON_MARGIN + _playAgainButton.Height);
        }
        #endregion
    }
}
