using System;

namespace BioInspiredOptimization.Individual
{
    class Agent : Individual, IComparable<Agent>
    {
        public double Error;

        public Agent(double[] pos, double error)
        {
            Coords = new double[pos.Length];
            pos.CopyTo(Coords, 0);

            Error = error;
        }

        public int CompareTo(Agent y)
        {
            return Error.CompareTo(y.Error);
        }

        public double distanceTo(Agent y)
        {
            double total = 0;
            for(int i = 0; i < Coords.Length; i++)
            {
                total += (this.Coords[i] - y.Coords[i]) * (this.Coords[i] - y.Coords[i]);
            }
            return Math.Sqrt(total);
        }
    }
}
