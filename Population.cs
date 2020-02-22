using System.Collections.Generic;
using System.Linq;

namespace TSP
{
    public class Population
    {

        // Member variables
        public List<Tour> p { get; private set; }
        public double maxFit { get; private set; }

        // ctor
        public Population(List<Tour> l)
        {
            this.p = l;
            this.maxFit = CalcMaxFit();
        }

        // Functionality
        public static Population Randomized(Tour t, int n)
        {
            List<Tour> tmp = new List<Tour>();

            for (int i = 0; i < n; ++i)
                tmp.Add( t.Shuffle() );

            return new Population(tmp);
        }

        private double CalcMaxFit()
        {
            return p.Max( t => t.fitness );
        }

        public Tour Select()
        {
            while (true)
            {
                int i = Program.r.Next(0, Env.popSize);

                if (Program.r.NextDouble() < p[i].fitness / maxFit)
                    return new Tour(p[i].t);
            }
        }

        public Population GenNewPop(int n)
        {
            List<Tour> p = new List<Tour>();

            for (int i = 0; i < n; ++i)
            {
                Tour t = Select().Crossover( Select() );

                foreach (City c in t.t)
                    t = t.Mutate();

                p.Add(t);
            }

            return new Population(p);
        }

        public Population Elite(int n)
        {
            List<Tour> best = new List<Tour>();
            Population tmp = new Population(p);

            for (int i = 0; i < n; ++i)
            {
                best.Add( tmp.FindBest() );
                tmp = new Population( tmp.p.Except(best).ToList() );
            }

            return new Population(best);
        }

        public Tour FindBest()
        {
            foreach (Tour t in this.p)
            {
                if (t.fitness == this.maxFit)
                    return t;
            }

            // Should never happen, it's here to shut up the compiler
            return null;
        }

        public Population Evolve()
        {
            Population best = Elite(Env.elitism);
            Population np = GenNewPop(Env.popSize - Env.elitism);
            return new Population( best.p.Concat(np.p).ToList() );
        }
    }
}