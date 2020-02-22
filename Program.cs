using System;
using System.Collections.Generic;

namespace TSP
{
    public class Program
    {
        public static Random r { get; private set; }

        public static List<Solution> solution = new List<Solution>();

        public static void Main()
        {
            r = new Random();

            Tour dest = Tour.Random(Env.numCities);

            // for (int i = 1; i < 11; i++)
            // {
            //     int popSize = 12 * i;
            //     int elitism = 0.1 * popSize < 1 ? 1 : (int)(0.1 * popSize);
            //     double mutRate = 0.03 * i;
            //     Env.SetupParams(mutRate, Env.elitism, Env.popSize);
            //     Solve(dest);
            // }

            for (int i = 1; i < 11; i++)
            {
                Env.SetupParams(0.02, 45, 60);
                Solve(dest);
            }

            PrintSolution();
        }

        public static void PrintSolution()
        {
            foreach(var solution in solution)
            {
                Console.WriteLine(solution.ToString());
            }
        }

        public static void Solve(Tour dest)
        {
            Population p = Population.Randomized(dest, Env.popSize);

            int gen = 0;
            double maxFit = -1000000;

            while (gen < 100)
            {
                double oldFit = p.maxFit;

                p = p.Evolve();
                if (p.maxFit > oldFit)
                {
                    maxFit = p.maxFit;
                }

                gen++;
            }

            solution.Add(new Solution()
            {
                Elitism = Env.elitism,
                Fitest = maxFit,
                MutationRate = Env.mutRate,
                PopulationSize = Env.popSize
            });
        }
    }

    public static class Env
    {
        public static void SetupEnv(double mutationRate, int _elitism, int _popSize, int _numCities)
        {
            mutRate = mutationRate;
            elitism = _elitism;
            popSize = _popSize;
            numCities = _numCities;
        }

        public static void SetupParams(double mutationRate, int _elitism, int _popSize)
        {
            mutRate = mutationRate;
            elitism = _elitism;
            popSize = _popSize;
        }

        public static double mutRate = 0.03;
        public static int elitism = 6;
        public static int popSize = 60;
        public static int numCities = 40;
    }
}