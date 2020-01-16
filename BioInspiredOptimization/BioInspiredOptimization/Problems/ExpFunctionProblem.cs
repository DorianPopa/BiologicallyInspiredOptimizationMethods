using System;

namespace BioInspiredOptimization.Problems
{
    class ExpFunctionProblem : IProblem
    {
        public int Dimension { get; private set; }

        public double MinX { get; private set; }

        public double MaxX { get; private set; }

        private ExpFunctionProblem() { }

        public ExpFunctionProblem(int dimension, double minValue, double maxValue)
        {
            Dimension = dimension;
            MinX = minValue;
            MaxX = maxValue;
        }

        public double ErrorFunction(double[] values)
        {
            double trueMin = -1;
            double result = Compute(values);
            return result - trueMin;
        }
        public double Compute(double[] values)
        {
            double sum = 0;
            foreach (double x in values)
                sum += x * x;
            sum *= -0.5;
            return -1 * Math.Exp(sum);
        }
    }
}
