using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroid
{
    class GameConstants
    {
        #region Window Support
        public const int WINDOW_WIDTH = 800;
        public const int WINDOW_HEIGHT = 600;
        public const int WINDOW_SPAWN_BORDER_SIZE = 50;
        #endregion

        #region Ship Support
        public const float SHIP_ACCELERATION = 0.01f;
        public const float SHIP_BASE_SPEED = 0.1f;
        public const float SHIP_MAX_SPEED = 0.2f;
        public const int SHIP_FIRING_DELAY = 100;
        public const int SHIP_RADIUS = 22;
        #endregion

        #region Asteroid Support
        public const int ASTEROID_IMAGESPARROW = 4;
        public const float ASTEROID_MIN_SPEED = 0.1f;
        public const float ASTEROID_MAX_SPEED = 0.2f;
        public const int ASTEROID_LARGE_HEALTH = 80;
        public const int ASTEROID_MEDIUM_HEALTH = 40;
        public const int ASTEROID_SMALL_HEALTH = 20;
        public const int ASTEROID_LARGE_DAMAGE = 80;
        public const int ASTEROID_MEDIUM_DAMAGE = 60;
        public const int ASTEROID_SMALL_DAMAGE = 40;
        public const int ASTEROID_DESTROY_SPAWN_COUNT = 2;
        public const int ASTEROID_MAX_COUNT = 4;
        public const int ASTEROID_MAX_COUNT_LARGE = 2;
        #endregion

        #region Projectile Support
        public const float PROJECTILE_SPEED = 0.3f;
        public const int PROJECTILE_DAMAGE = 10;
        #endregion

        #region Explosion Support
        public const int EXPLOSION_DURATION = 10;
        public const int EXPLOSION_AMOUNT_FRAMES = 9;
        public const int EXPLOSION_IMAGE_COLUMNS = 3;
        public const int EXPLOSION_IMAGE_ROWS = 3;
        #endregion

        #region Menu Support
        public const string MENU_ENTER_NAME_COMMAND = "Please enter your name in 3 letters";
        public const int MENU_ENTER_NAME_COMMAND_MARGIN = 150;
        public const int MENU_BUTTON_AREA_X = 50;
        public const int MENU_BUTTON_AREA_Y = 200;
        public const int MENU_BUTTON_MARGIN = 50;
        public const int MENU_LEADERBOARD_Y = 215;
        public const int MENU_LEADERBOARD_MARGIN_X = 200;
        public const int MENU_LEADERBOARD_MARGIN_Y = 50;
        #endregion

        #region Score Support
        public const int SCORE_POSITION_X = 30;
        public const int SCORE_POSITION_Y = 20;
        public const int SCORE_ASTEROID_SMALL = 10;
        public const int SCORE_ASTEROID_MEDIUM = 20;
        public const int SCORE_ASTEROID_LARGE = 40;
        #endregion

        #region Leaderboards Support
        public const string LEADERBOARDS_FILE_NAME = "leaderboard.txt";
        #endregion
    }
}
