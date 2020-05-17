using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolvesAndRabbitsSimulation.Engine
{
    class World
    {
        private Random rnd = new Random();

        private const int width = 255;
        private const int height = 255;
        private Size size = new Size(width, height);
        private GameObject[] objects = new GameObject[0];

        public IEnumerable<GameObject> GameObjects
        {
            get
            {
                return objects.ToArray();
            }
        }

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public float Random()
        {
            return (float)rnd.NextDouble();
        }

        public Point RandomPoint()
        {
            return new Point(rnd.Next(width), rnd.Next(height));
        }

        public int Random(int min, int max)
        {
            return rnd.Next(min, max);
        }

        public void Add(GameObject obj)
        {
            objects = objects.Concat(new GameObject[] { obj }).ToArray();
        }

        public void Remove(GameObject obj)
        {
            objects = objects.Where(o => o != obj).ToArray();
        }

        public virtual void Update()
        {
            foreach (GameObject obj in GameObjects)
            {
                obj.UpdateOn(this);
                obj.Position = PositiveMod(obj.Position, size);
            }
        }

        public virtual void DrawOn(Graphics graphics)
        {
            foreach (GameObject obj in GameObjects)
            {
                graphics.FillRectangle(new Pen(obj.Color).Brush, obj.Bounds);
            }
        }

        // http://stackoverflow.com/a/10065670/4357302
        private static int PositiveMod(int a, int n)
        {
            int result = a % n;
            if ((a < 0 && n > 0) || (a > 0 && n < 0))
                result += n;
            return result;
        }
        private static Point PositiveMod(Point p, Size s)
        {
            return new Point(PositiveMod(p.X, s.Width), PositiveMod(p.Y, s.Height));
        }

        public double Dist(PointF a, PointF b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public IEnumerable<GameObject> ObjectsAt(Point pos)
        {
            return GameObjects.Where(each =>
            {
                Rectangle bounds = each.Bounds;
                PointF center = new PointF((bounds.Left + bounds.Right - 1) / 2.0f,
                                           (bounds.Top + bounds.Bottom - 1) / 2.0f);
                return Dist(pos, center) <= bounds.Width / 2.0f
                    && Dist(pos, center) <= bounds.Height / 2.0f;
            });
        }
    }
}
