using System;
using BioInspiredOptimization.Problems;
using BioInspiredOptimization.Individual;

namespace BioInspiredOptimization.Solvers
{
    class CuckooSearch : ISolver
    {
        private int NumberOfNests;
        private int MaxIterations;
        private double AcceptableError;
        private int ProbabilityOfAbandon;
        private double LevyExponent;
        private double Alpha;

        public CuckooSearch(int numberOfNests, int maxIterations, double acceptableError, int probabilityOfAbandon, double levyExponent, double alpha)
        {
            NumberOfNests = numberOfNests;
            MaxIterations = maxIterations;
            AcceptableError = acceptableError;
            ProbabilityOfAbandon = probabilityOfAbandon;
            LevyExponent = levyExponent;
            Alpha = alpha;
        }

        public double[] Solve(IProblem problem)
        {
            Random rnd = new Random();
            Nest[] hosts = new Nest[NumberOfNests];
            double[] bestGlobalPosition = new double[problem.Dimension];

            // generate initial population of host nests
            for (int i = 0; i < hosts.Length; i++)
            {
                double[] randomPosition = new double[problem.Dimension];
                for (int j = 0; j < randomPosition.Length; j++)
                    randomPosition[j] = (problem.MaxX - problem.MinX) * rnd.NextDouble() + problem.MinX;

                double error = problem.ErrorFunction(randomPosition);

                hosts[i] = new Nest(randomPosition, error);
            }

            
            int currentIteration = 0;

            while (currentIteration < MaxIterations)
            {
                for(int i = 0; i < hosts.Length; i++)
                {
                    double[] newCoords = new double[problem.Dimension];
                    hosts[i].Coords.CopyTo(newCoords, 0);
                    for(int j = 0; j<problem.Dimension; j++)
                    {
                        newCoords[j] += LevyFlight(LevyExponent);
                        if (newCoords[j] < problem.MinX)      // new position out of min-bound
                            newCoords[j] = problem.MinX;
                        if (newCoords[j] > problem.MaxX)      // new position out of max-bound
                            newCoords[j] = problem.MaxX;
                    }
                    double error = problem.ErrorFunction(newCoords);
                    Nest newNest = new Nest(newCoords, error);

                    if(newNest.Error < hosts[i].Error)
                    {
                        newNest.Coords.CopyTo(hosts[i].Coords, 0);
                        hosts[i].Error = newNest.Error;
                    }
                }
                
                Array.Sort(hosts);
                for(int i = hosts.Length - 1; i > 1; i--)
                {
                    int abandon = rnd.Next(0, 100);
                    if (abandon < ProbabilityOfAbandon)
                    {
                        double[] randomPosition = new double[problem.Dimension];
                        for (int j = 0; j < randomPosition.Length; j++)
                            randomPosition[j] = (problem.MaxX - problem.MinX) * rnd.NextDouble() + problem.MinX;

                        double error = problem.ErrorFunction(randomPosition);
                        hosts[i] = new Nest(randomPosition, error);
                    }
                }
                Array.Sort(hosts);

                hosts[0].Coords.CopyTo(bestGlobalPosition, 0);
                if (hosts[0].Error < AcceptableError)
                    break;

                currentIteration++;
            }
            return bestGlobalPosition;
        }

        public double LevyFlight(double param)
        {
            Random rnd = new Random();
            double x = rnd.NextDouble() * (0.5 * Math.PI - -0.5 * Math.PI) + -0.5 * Math.PI;
            double y = -1 * Math.Log(rnd.NextDouble());

            double a = Math.Sin((param - 1.0) * x) / (Math.Pow(Math.Cos(x), (1.0 / (param - 1.0))));
            double b = Math.Pow((Math.Cos((2.0 - param) * x) / y), ((2.0 - param) / (param - 1.0)));

            double z = a * b;
            return z * Alpha;
        }
    }
}
