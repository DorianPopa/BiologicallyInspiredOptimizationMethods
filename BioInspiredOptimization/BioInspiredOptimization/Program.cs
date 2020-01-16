using BioInspiredOptimization.Problems;
using BioInspiredOptimization.Solvers;
using System;

namespace BioInspiredOptimization
{
    class Program
    {
        static void Main(string[] args)
        {
            IProblem currentProblem = new ExpFunctionProblem(5, -5, 5);

            int numberOfParticles = 25;
            int maxIterations = 2000;
            double acceptableError = 10e-8d;
            ISolver particleSwarmingAlgorithm = new ParticleSwarming(numberOfParticles, maxIterations, acceptableError);
            ISolver cuckooSearchAlgorithm = new CuckooSearch(numberOfParticles, maxIterations, acceptableError, 25, 3, 0.01);

            double[] result = SolveProblem(currentProblem, particleSwarmingAlgorithm);
            //double[] result = SolveProblem(currentProblem, cuckooSearchAlgorithm);
            double finalError = currentProblem.ErrorFunction(result);

            PrintResult(result, finalError);
        }

        public static double[] SolveProblem(IProblem problem, ISolver solver)
        {
            return solver.Solve(problem);
        }

        public static void PrintResult(double[] result, double finalError)
        {
            Console.WriteLine("Best position/solution found:");
            for (int i = 0; i < result.Length; ++i)
            {
                Console.Write("x" + i + " = ");
                Console.WriteLine(result[i].ToString("F10") + " ");
            }
            Console.WriteLine("");
            Console.Write("Final best error = ");
            Console.WriteLine(finalError.ToString("F10"));

            Console.ReadLine();
        }
    }
}
