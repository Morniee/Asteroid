using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroid
{
    public struct Circle
    {
        #region Constructor
        public Circle(float x, float y, int radius) : this()
        {
            Center = new Vector2(x, y);
            Radius = radius;
        }
        #endregion

        #region Properties
        public Vector2 Center { get; set; }
        public int Radius { get; private set; }
        #endregion

        #region Public methods
        public bool Intersects(Rectangle r)
        {
            float rW = (r.Right - r.Left) / 2;
            float rH = (r.Bottom - r.Top) / 2;

            float distanceX = Math.Abs(Center.X - (r.Left + rW));
            float distanceY = Math.Abs(Center.Y - (r.Top + rH));

            if(distanceX >= Radius + rW || distanceY >= Radius + rH)
            {
                return false;
            }

            if(distanceX < rW || distanceY < rH)
            {
                return true;
            }

            distanceX -= rW;
            distanceY -= rH;

            if(distanceX * distanceX + distanceY * distanceY < Radius * Radius)
            {
                return true;
            }

            return false;
        }

        public bool Intersects(Circle other)
        {
            return ((other.Center - Center).Length() < (other.Radius - Radius));
        }
        #endregion
    }
}
