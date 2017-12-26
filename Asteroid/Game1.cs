using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Asteroid
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState _oldKeyboardState;
        Texture2D _explosionSpriteStrip, _asteroidSpriteStrip;
        SoundEffect _explosionSound, _bounceSound;
        Menu _menu;
        Score _score;
        Ship _ship;
        Player _player;
        Explosion _explosion;
        List<Asteroid> _asteroids;
        static List<Projectile> _projectiles;
        static Texture2D _projectileShipSprite;
        private static bool _restart, _canStart, _updateLeaderboard;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GameConstants.WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = GameConstants.WINDOW_HEIGHT;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            RandomNumberGenerator.Initialize();
            HighScoreData.Initialize();
            _ship = new Ship(Content, "Ship", new Vector2(GameConstants.WINDOW_WIDTH / 2, GameConstants.WINDOW_HEIGHT / 2));
            _asteroids = new List<Asteroid>();
            _projectiles = new List<Projectile>();
            _menu = new Menu(Content);
            _player = new Player(Content, "baseFont", new Vector2(GameConstants.WINDOW_WIDTH / 2, GameConstants.WINDOW_HEIGHT / 2));
            _score = new Score(Content, new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" });

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _projectileShipSprite = Content.Load<Texture2D>(@"graphics/ProjectileShip");
            _asteroidSpriteStrip = Content.Load<Texture2D>(@"graphics/Asteroid");
            _explosionSpriteStrip = Content.Load<Texture2D>(@"graphics/explosion");
            _explosionSound = Content.Load<SoundEffect>(@"audio/Explosion");
            _bounceSound = Content.Load<SoundEffect>(@"audio/Bounce");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            #region Mouse Visibility Support
            if (_menu.Gamestate != GameState.PLAYING)
            {
                IsMouseVisible = true;
            }
            else
            {
                IsMouseVisible = false;
            }
            #endregion

            #region Menu Support
            if(currentKeyboardState.IsKeyDown(Keys.Escape) && _oldKeyboardState.IsKeyUp(Keys.Escape) && _menu.Gamestate == GameState.PLAYING)
            {
                _menu.Gamestate = GameState.PAUSED;
                _menu.ShowControls = true;
                _menu.ShowLeaderboards = false;
            } else if(currentKeyboardState.IsKeyDown(Keys.Escape) && _oldKeyboardState.IsKeyUp(Keys.Escape) && _menu.Gamestate == GameState.PAUSED)
            {
                _menu.Gamestate = GameState.PLAYING;
            }

            if(_restart == true)
            {
                PlayAgain();
            }

            if (_canStart == true && _menu.Gamestate != GameState.GAMEOVER)
            {
                StartGame();
            }

            if(_updateLeaderboard)
            {
                HighScoreData.LoadHighScores().SaveHighScore(_score.CurrentScore, _player.Name);
                _updateLeaderboard = false;
            }
            #endregion

            #region Updates
            _menu.Update(mouseState);
            _player.Update(_menu.Gamestate, currentKeyboardState);
            #endregion

            #region GameObject Updates
            if (_menu.Gamestate == GameState.PLAYING)
            {
                if(_ship.IsAlive)
                {
                    _ship.Update(gameTime, Keyboard.GetState(), _menu.Gamestate);
                    UpdateAsteroids(gameTime);
                    UpdateProjectiles(gameTime);
                    GenerateNewAsteroids();
                }
                
                #region Collision Support
                CheckOutOfBounds();
                ResolveShipAsteroidCollision();
                ResolveAsteroidProjectileCollision();
                CheckShipHealth();
                #endregion
                
                #region Cleaning Support
                ClearFinishedExplosions();
                ClearOffScreenProjectiles();
                ClearDestroyedAsteroids();
                #endregion

                UpdateExplosions(gameTime);   
            }
            #endregion

            _oldKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            DrawGameObjects(spriteBatch);
            DrawMenu(spriteBatch);
            DrawPlayerName(spriteBatch);
            DrawScore(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Draw Methods
        private void DrawGameObjects(SpriteBatch spriteBatch)
        {
            if(_ship.IsAlive)
            {
                DrawProjectiles(spriteBatch);
                DrawShip(spriteBatch);
                DrawAsteroids(spriteBatch);
            }

            DrawExplosions(spriteBatch);
        }
        private void DrawMenu(SpriteBatch spriteBatch)
        {
            _menu.Draw(spriteBatch);
        }

        private void DrawPlayerName(SpriteBatch spriteBatch)
        {
            if(_menu.Gamestate == GameState.NAME_ENTRY) _player.Draw(spriteBatch);
        }

        private void DrawScore(SpriteBatch spriteBatch)
        {
            if(_menu.Gamestate != GameState.STARTUP && _menu.Gamestate != GameState.NAME_ENTRY)
            {
                _score.Draw(spriteBatch);
            }  
        }
        private void DrawShip(SpriteBatch spriteBatch)
        {
            if(_ship.IsAlive)
            {
                _ship.Draw(spriteBatch);
            }
        }

        private void DrawAsteroids(SpriteBatch spriteBatch)
        {
            foreach(Asteroid asteroid in _asteroids)
            {
                asteroid.Draw(spriteBatch);
            }
        }

        private void DrawProjectiles(SpriteBatch spriteBatch)
        {
            foreach (Projectile projectile in _projectiles)
            {
                projectile.Draw(spriteBatch);
            }
        }

        private void DrawExplosions(SpriteBatch spriteBatch)
        {
            if (_explosion != null) _explosion.Draw(spriteBatch);
        }
        #endregion

        #region Update Methods
        private void UpdateAsteroids(GameTime gameTime)
        {
            foreach (Asteroid asteroid in _asteroids)
            {
                asteroid.Update(gameTime, _menu.Gamestate);
            }
        }

        private void UpdateProjectiles(GameTime gameTime)
        {
            foreach (Projectile projectile in _projectiles)
            {
                projectile.Update(gameTime);
            }
        }

        private void UpdateExplosions(GameTime gameTime)
        {
            if (_explosion != null) _explosion.Update(gameTime);
        }
        #endregion

        #region Collision Detection Methods
        private void CheckOutOfBounds()
        {
            if (_ship.Position.X - _ship.Width / 2 < 0 ||
                _ship.Position.X + _ship.Width / 2 > GameConstants.WINDOW_WIDTH ||
                _ship.Position.Y - _ship.Height / 2 < 0 ||
                _ship.Position.Y + _ship.Height / 2 > GameConstants.WINDOW_HEIGHT)
            {
                _ship.Health = 0;
            }
            
        }

        private void ResolveShipAsteroidCollision()
        {
            foreach(Asteroid asteroid in _asteroids)
            {
                if(_ship.CollisionCircle.Intersects(asteroid.CollisionRectangle))
                {
                    switch(asteroid.Size)
                    {
                        case AsteroidSize.LARGE:
                            _ship.Health -= GameConstants.ASTEROID_LARGE_DAMAGE;
                            break;
                        case AsteroidSize.MEDIUM:
                            _ship.Health -= GameConstants.ASTEROID_MEDIUM_DAMAGE;
                            break;
                        case AsteroidSize.SMALL:
                            _ship.Health -= GameConstants.ASTEROID_SMALL_DAMAGE;
                            break;
                    }
                }
            }
        }

        private void ResolveAsteroidProjectileCollision()
        {
            foreach (Asteroid asteroid in _asteroids)
            {
                foreach (Projectile projectile in _projectiles)
                {
                    if (asteroid.CollisionRectangle.Intersects(projectile.CollisionRectangle))
                    {
                        asteroid.Health -= GameConstants.PROJECTILE_DAMAGE;

                        if(asteroid.Health == 0)
                        {
                            switch (asteroid.Size)
                            {
                                case AsteroidSize.SMALL:
                                    _score.CurrentScore += GameConstants.SCORE_ASTEROID_SMALL;
                                    break;
                                case AsteroidSize.MEDIUM:
                                    _score.CurrentScore += GameConstants.SCORE_ASTEROID_MEDIUM;
                                    break;
                                case AsteroidSize.LARGE:
                                    _score.CurrentScore += GameConstants.SCORE_ASTEROID_LARGE;
                                    break;
                            }

                            asteroid.IsActive = false;
                            projectile.IsActive = false;
                        }
                    }
                }
            }
        }
        #endregion

        #region Generate Asteroids Methods
        private void GenerateNewAsteroidsOnDestruction(Asteroid asteroid)
        {
            Vector2 velocity = MathHelper.GetRandomVelocity(GameConstants.ASTEROID_MIN_SPEED, GameConstants.ASTEROID_MAX_SPEED);
            for(int i = 0; i < GameConstants.ASTEROID_DESTROY_SPAWN_COUNT; i++)
            {
                if (asteroid.Size == AsteroidSize.MEDIUM)
                {
                    _asteroids.Add(new Asteroid(_asteroidSpriteStrip, AsteroidSize.SMALL, asteroid.Position, velocity, _bounceSound));
                } else
                {
                    _asteroids.Add(new Asteroid(_asteroidSpriteStrip, AsteroidSize.MEDIUM, asteroid.Position, velocity, _bounceSound));
                }

                velocity *= new Vector2(-1, -1);
            }
        }

        private void GenerateNewAsteroids()
        {
            int count = _asteroids.FindAll(asteroid => asteroid.Size == AsteroidSize.LARGE).Count;

            if(count < GameConstants.ASTEROID_MAX_COUNT_LARGE && _asteroids.Count < GameConstants.ASTEROID_MAX_COUNT)
            {
                Vector2 velocity = MathHelper.GetRandomVelocity(GameConstants.ASTEROID_MIN_SPEED, GameConstants.ASTEROID_MAX_SPEED);
                _asteroids.Add(new Asteroid(_asteroidSpriteStrip,
                                   AsteroidSize.LARGE,
                                   MathHelper.GetRandomPositionBasedOnVelocity(velocity),
                                   velocity,
                                   _bounceSound
                               ));
                GenerateNewAsteroids();
            }
        }
        #endregion

        #region Memory Cleaning Methods
        private void ClearFinishedExplosions()
        {
            if (_explosion != null && _explosion.IsFinished)
            {
                _menu.Gamestate = GameState.GAMEOVER;
                _explosion = null;
            }
        }

        private void ClearOffScreenProjectiles()
        {
            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                if (!_projectiles[i].IsActive)
                {
                    _projectiles.RemoveAt(i);
                }
            }
        }

        private void ClearDestroyedAsteroids()
        {
            for (int i =  _asteroids.Count - 1; i >= 0; i--)
            {
                if (!_asteroids[i].IsActive)
                {
                    if(_asteroids[i].Size != AsteroidSize.SMALL)    GenerateNewAsteroidsOnDestruction(_asteroids[i]);
                    _asteroids.RemoveAt(i);
                }
            }
        }
        #endregion

        private void CheckShipHealth()
        {
            if(_ship.Health <= 0)
            {
                _asteroids.Clear();
                _projectiles.Clear();
                _ship.IsAlive = false;
                _explosion = new Explosion(_explosionSpriteStrip, (int)_ship.Position.X, (int)_ship.Position.Y, _explosionSound);
                _ship.Reset();
            }  
        }

        public static void SetRestart()
        {
            _restart = true;
            _updateLeaderboard = true;
        }

        public static void SetStart(bool value)
        {
            _canStart = value;
            _updateLeaderboard = true;
        }

        public static Texture2D GetProjectileSprite()
        {
            return _projectileShipSprite;
            
        }

        public static void AddProjectile(Projectile projectile)
        {
            _projectiles.Add(projectile);
        }

        private void PlayAgain()
        {
            if(_restart)
            {
                
                _ship.IsAlive = true;
                _score.Reset();
                _restart = false;
            }
        }

        private void StartGame()
        {
            _menu.Gamestate = GameState.PLAYING;
            _ship.IsAlive = true;
            _ship.Health = 100;
            _canStart = false;
        }
    }
}
