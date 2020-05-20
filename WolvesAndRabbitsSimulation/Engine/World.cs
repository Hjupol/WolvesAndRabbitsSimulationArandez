using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace WolvesAndRabbitsSimulation.Engine
{
    class World
    {
        private Random rnd = new Random();

        private const int width = 255;
        private const int height = 255;
        private Size size = new Size(width, height);
        //private GameObject[] objects = new GameObject[0];

        private HashSet<GameObject> objects = new HashSet<GameObject>();
        private List<GameObject>[,] spatialObjects = new List<GameObject>[(width/2)+1,(height/2)+1];
        private int cellSize = 2;
        private Pen pen = new Pen(Color.White);

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
            //objects = objects.Concat(new GameObject[] { obj }).ToArray();
            objects.Add(obj);
            
            GetBucketAt(obj.Position).Add(obj);
        }

        public void Remove(GameObject obj)
        {
            //objects = objects.Where(o => o != obj).ToArray();
            objects.Remove(obj);
            GetBucketAt(obj.Position).Remove(obj);
        }

        public List<GameObject> GetBucketAt(Point pos)
        {
            int convertedPosX = (int)(pos.X / cellSize);
            int ConvertedPosY = (int)(pos.Y / cellSize);
            var bucket = spatialObjects[convertedPosX ,ConvertedPosY];
            if (bucket == null)
            {
                bucket = new List<GameObject>();
                spatialObjects[convertedPosX, ConvertedPosY]=bucket;
            }
            return bucket;
        }

        public List<GameObject> GetBucketAt(int x,int y)
        {
            if (x > 0 && y > 0 && x < 256 && y < 256)
            {
                int convertedPosX = (int)(x / cellSize);
                int ConvertedPosY = (int)(y / cellSize);
                var bucket = spatialObjects[convertedPosX, ConvertedPosY];
                if (bucket == null)
                {
                    bucket = new List<GameObject>();
                    spatialObjects[convertedPosX, ConvertedPosY] = bucket;
                }
                return bucket;
            }
            else
            {
                return null;
            }
        }

        public virtual void Update()
        {
            foreach (GameObject obj in GameObjects)
            {
                var oldPosition = obj.Position;

                obj.UpdateOn(this);
                obj.Position = PositiveMod(obj.Position, size);

                if (oldPosition != obj.Position)
                {
                    GetBucketAt(oldPosition).Remove(obj);
                    GetBucketAt(obj.Position).Add(obj);
                }
            }
        }

        public virtual void DrawOn(Graphics graphics)
        {
            foreach (GameObject obj in GameObjects)
            {
                pen.Color = obj.Color;
                graphics.FillRectangle(pen.Brush, obj.Bounds);
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
            return GetBucketAt(pos);
            //return GameObjects.Where(each =>
            //{
            //    Rectangle bounds = each.Bounds;
            //    PointF center = new PointF((bounds.Left + bounds.Right - 1) / 2.0f,
            //                               (bounds.Top + bounds.Bottom - 1) / 2.0f);
            //    return Dist(pos, center) <= bounds.Width / 2.0f
            //        && Dist(pos, center) <= bounds.Height / 2.0f;
            //});
        }

        public List<GameObject> ObjectsCloseTo(Point pos)
        {
            List<GameObject> objectsClose = new List<GameObject>();
            for(int x = -1; x < 2; x++)
            {
                for(int y = -1; y < 2; y++)
                {
                    objectsClose.AddRange(CloseObj(pos, GetBucketAt(pos.X + x, pos.Y + y)));
                }
            }
            return objectsClose;

        }
        
        public List<GameObject> CloseObj(Point position,List<GameObject> bucketObjects)
        {
            List<GameObject> objectsCloseList = new List<GameObject>();
            if (bucketObjects != null)
            {
                foreach (GameObject g in bucketObjects)
                {
                    if (Dist(position, g.Position) <= g.Bounds.Width / 2.0f
                        && Dist(position, g.Position) <= g.Bounds.Height / 2.0f)
                    {
                        objectsCloseList.Add(g);
                    }
                }
            }
            return objectsCloseList;
        }
           
    }
}
