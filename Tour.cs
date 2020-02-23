using System.Collections.Generic;
using System.Linq;

namespace TSP
{
    public class Tour
    {
        // list of cities
        public List<City> t { get; private set; }

        // total distance between the cities
        public double distance { get; private set; }

        // fitness value
        public double fitness { get; private set; }

        // ctor
        public Tour(List<City> l)
        {
            t = l;
            distance = CalcDist();
            fitness = CalcDist();
        }

        // generates new random tour
        public static Tour Random(int n)
        {
            List<City> t = new List<City>();

            for (int i = 0; i < n; ++i)
                t.Add( City.Random() );

            return new Tour(t);
        }

        // shuffle cities in tour
        public Tour Shuffle()
        {
            List<City> tmp = new List<City>(this.t);
            int n = tmp.Count;

            while (n > 1)
            {
                n--;
                int k = Program.r.Next(n + 1);
                City v = tmp[k];
                tmp[k] = tmp[n];
                tmp[n] = v;
            }

            return new Tour(tmp);
        }

        // crossover tour
        public Tour Crossover(Tour m)
        {
            int i = Program.r.Next(0, m.t.Count);
            int j = Program.r.Next(i, m.t.Count);

            // take cities from i index to j
            List<City> s = this.t.GetRange(i, j - i + 1);

            // take rest of the cities
            List<City> ms = m.t.Except(s).ToList();

            // generate list with new cities
            List<City> c = ms.Take(i)
                             .Concat(s)
                             .Concat( ms.Skip(i) )
                             .ToList();
            return new Tour(c);
        }

        // mutate tour
        public Tour Mutate()
        {
            List<City> tmp = new List<City>(this.t);

            // check if should mutate based on mutation ratio
            if (Program.r.NextDouble() < Env.mutRate)
            {
                // swap two cities
                int i = Program.r.Next(0, this.t.Count);
                int j = Program.r.Next(0, this.t.Count);
                City v = tmp[i];
                tmp[i] = tmp[j];
                tmp[j] = v;
            }

            return new Tour(tmp);
        }

        // calculate total distance
        private double CalcDist()
        {
            double total = 0;
            for (int i = 0; i < this.t.Count; ++i)
                total += t[i].DistanceTo( t[ (i + 1) % t.Count ] );

            return total;
        }

        private double CalcFit()
        {
            return 1 / distance;
        }
    }
}