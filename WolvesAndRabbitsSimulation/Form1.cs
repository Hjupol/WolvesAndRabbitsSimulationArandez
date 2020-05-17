using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WolvesAndRabbitsSimulation.Engine;
using WolvesAndRabbitsSimulation.Simulation;

namespace WolvesAndRabbitsSimulation
{
    public partial class Form1 : Form
    {
        private const int scale = 3;
        private World world = new World();
        private long frameCount = 0;
        private long frameTime = 0;
        private string fileName;
        private Stopwatch stopwatch = new Stopwatch();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            stopwatch.Start();
            ChooseFileName();
            InitializeWorld();
            SetStyle(ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint,
                true);
        }

        private void ChooseFileName()
        {
            int index = 0;
            do
            {
                fileName = string.Format("rabbits.{0}.{1}.csv", DateTime.Now.ToString("yyyyMMdd"), ++index);
            }
            while (File.Exists(fileName));
        }

        private void InitializeWorld()
        {
            FillWithGrass();
            SpawnSomeRabbits();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.ScaleTransform(scale, scale);
            world.DrawOn(e.Graphics);
        }
                
        private void updateTimer_Tick(object sender, EventArgs e)
        {
            Step();
        }

        private void Step()
        {
            ClientSize = new Size(world.Width * scale, world.Height * scale);
            long begin = stopwatch.ElapsedMilliseconds;
            world.Update();
            Refresh();
            long end = stopwatch.ElapsedMilliseconds;
            RegisterFrameTime(end - begin);

            Text = string.Format("Frames: {3}, Objects: {0}, Average FPS: {1:00}, Current FPS: {2:00}",
                world.GameObjects.Count(),
                1000.0 / (frameTime / frameCount),
                1000.0 / (end - begin),
                frameCount);

            File.AppendAllText(fileName, string.Format("{0}\n", world.GameObjects.Where(o => o is Rabbit).Count()));
        }

        private void RegisterFrameTime(long time)
        {
            frameTime += time;
            frameCount++;
        }

        private void lifeSpawner_Tick(object sender, EventArgs e)
        {
            if (world.GameObjects.Where(o => o is Rabbit).Count() == 0)
            {
                SpawnSomeRabbits();
            }
        }

        private void FillWithGrass()
        {
            for (int x = 0; x < world.Width; x += Grass.PATCH_SIZE)
            {
                for (int y = 0; y < world.Height; y += Grass.PATCH_SIZE)
                {
                    Grass grass = new Grass();
                    grass.Position = new Point(x, y);
                    grass.Growth = world.Random(0, 50);
                    world.Add(grass);
                }
            }
        }

        private void SpawnSomeRabbits()
        {
            for (int i = 0; i < 50; i++)
            {
                Rabbit rabbit = new Rabbit();
                rabbit.Rotation = world.Random() * Math.PI * 2;
                rabbit.Position = world.RandomPoint();
                if (world.Random(1, 10) < 5)
                {
                    rabbit.Position = new Point(0, rabbit.Position.Y);
                }
                else
                {
                    rabbit.Position = new Point(rabbit.Position.X, 0);
                }
                world.Add(rabbit);
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            Step();
        }
    }
}
