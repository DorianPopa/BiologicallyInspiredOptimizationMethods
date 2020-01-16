using System;

namespace BioInspiredOptimization.Individual
{
    class Nest : Individual, IComparable<Nest>
    {
        public double Error;
        public Nest(double[] pos, double err)
        {
            Coords = new double[pos.Length];
            pos.CopyTo(Coords, 0);
            Error = err;
        }

        public int CompareTo(Nest y)
        {
            return Error.CompareTo(y.Error);
        }
    }
}
