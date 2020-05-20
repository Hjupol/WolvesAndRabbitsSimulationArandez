using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using WolvesAndRabbitsSimulation.Engine;

namespace WolvesAndRabbitsSimulation.Simulation
{
    class Wolf:GameObject
    {
        private int DEATH_AGE = 3000;
        private int ADULTHOOD = 500;
        private int FOOD_TO_BREED = 1000;
        private int FOOD_CONSUMPTION = 2;
        private int MAX_CHILDREN = 10;
        private double BREED_PROBABILITY = 0.7;
        private double DEATH_PROBABILITY = 0.5;

        private int age = 0;
        private int food = 2000;

        public Wolf()
        {
            Color = Color.Black;
        }

        public override void UpdateOn(World forest)
        {
            EatSomeRabbit(forest);
            Breed(forest);
            GrowOld(forest);
            ConsumeFood(forest);
            Wander(forest);
        }

        private void EatSomeRabbit(World forest)
        {
            //IEnumerable<Grass> grass = forest.ObjectsAt(Position).Cast<Grass>();

            if (forest.ObjectsCloseTo(Position).Any(o => o is Rabbit && o != this))
            {
                Rabbit rabbit = forest.ObjectsAt(Position).Select(o => o as Rabbit).First(o => o != null);
                forest.Remove(rabbit);
                int amount = FOOD_CONSUMPTION * 100;
                food += amount;
            }
        }

        private void Breed(World forest)
        {
            if (age < ADULTHOOD || food < FOOD_TO_BREED) return;
            if (forest.ObjectsCloseTo(Position).Any(o => o is Wolf && o != this))
            {
                for (int i = 0; i < MAX_CHILDREN; i++)
                {
                    if (forest.Random(1, 10) <= 10 * BREED_PROBABILITY)
                    {
                        Wolf wolf = new Wolf();
                        wolf.Position = Position;
                        forest.Add(wolf);
                    }
                }
            }
        }

        private void GrowOld(World forest)
        {
            age++;
            if (age >= DEATH_AGE) { Die(forest); }
        }

        private void ConsumeFood(World forest)
        {
            food -= FOOD_CONSUMPTION;
            if (food <= 0) { Die(forest); }
        }

        private void Die(World forest)
        {
            if (forest.Random(1, 10) <= 10 * DEATH_PROBABILITY)
            {
                forest.Remove(this);
            }
        }

        private void Wander(World forest)
        {
            Forward(forest.Random(1, 5));
            Turn(forest.Random(-100, 100));
        }
    }
}

