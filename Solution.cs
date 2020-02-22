namespace TSP
{
    public class Solution
    {
        public int PopulationSize { get; set; }

        public int Elitism { get; set; }

        public double Fitest { get; set; }

        public double MutationRate { get; set; }

        public override string ToString()
        {
            return $"{PopulationSize};{MutationRate};{Elitism};{Fitest}".Replace(",", ".");
        }
    }
}