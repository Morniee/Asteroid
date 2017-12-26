using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroid
{
    public static class MathHelper
    {
        public static int GetRandomPosition(int min, int max)
        {
            return RandomNumberGenerator.Next(min, max);
        } 

        public static Vector2 GetRandomVelocity(float min, float max)
        {
            float speed = min + RandomNumberGenerator.NextFloat(max);
            int angle = RandomNumberGenerator.Next(15, 75);
            int quadrant = RandomNumberGenerator.Next(4);
            
            switch(quadrant)
            {
                case 0:
                    return new Vector2(speed * (float)Math.Cos(DegreesToRadians(angle)), speed * -(float)Math.Sin(DegreesToRadians(angle)));
                case 1:
                    return new Vector2(speed * (float)Math.Cos(DegreesToRadians(angle)), speed * (float)Math.Sin(DegreesToRadians(angle)));
                case 2:
                    return new Vector2(speed * -(float)Math.Cos(DegreesToRadians(angle * 2)), speed * (float)Math.Sin(DegreesToRadians(angle * 2)));
                default:
                    return new Vector2(speed * -(float)Math.Cos(DegreesToRadians(angle * 2)), speed * -(float)Math.Sin(DegreesToRadians(angle * 2)));
            }
        }

        public static Vector2 GetRandomPositionBasedOnVelocity(Vector2 velocity)
        {
            int x, y;

            if(velocity.X > 0 )
            {
                x = GetRandomPosition(0, GameConstants.WINDOW_SPAWN_BORDER_SIZE);
            } else
            {
                x = GetRandomPosition(GameConstants.WINDOW_WIDTH - GameConstants.WINDOW_SPAWN_BORDER_SIZE, GameConstants.WINDOW_WIDTH);
            }

            if (velocity.Y > 0)
            {
                y = GetRandomPosition(0, GameConstants.WINDOW_SPAWN_BORDER_SIZE);
            }
            else
            {
                y = GetRandomPosition(GameConstants.WINDOW_HEIGHT - GameConstants.WINDOW_SPAWN_BORDER_SIZE, GameConstants.WINDOW_HEIGHT);
            }

            return new Vector2(x, y);
        }

        public static float DegreesToRadians(int angle)
        {
            return (float)Math.PI / 180 * angle;
        }

        public static Vector2 CalculateDirection(int angle, float speed)
        {
            float radians = DegreesToRadians(angle);
            float x = (float)Math.Cos(radians) * speed;
            float y = (float)Math.Sin(radians) * speed;
            return new Vector2(x, y);
        }

    }
}
