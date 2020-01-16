using System;
using System.Collections.Generic;
using System.Text;

namespace BioInspiredOptimization.Problems
{
    public interface IProblem
    {
        int Dimension { get; }
        double MinX { get; }
        double MaxX { get; }

        public double ErrorFunction(double[] values);
        public double Compute(double[] values);
    }
}
