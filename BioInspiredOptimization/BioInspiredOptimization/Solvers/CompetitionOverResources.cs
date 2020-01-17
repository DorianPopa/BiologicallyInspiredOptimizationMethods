using System;
using BioInspiredOptimization.Individual;
using BioInspiredOptimization.Problems;

namespace BioInspiredOptimization.Solvers
{
    class CompetitionOverResources : ISolver
    {
        int PopulationSize;
        int GroupCount;
        int MaxIterations;
        double AcceptableError;

        public CompetitionOverResources(int populationSize, int groupCount, int maxIterations, double acceptableError)
        {
            PopulationSize = populationSize;
            GroupCount = groupCount;
            MaxIterations = maxIterations;
            AcceptableError = acceptableError;
        }

        public double[] Solve(IProblem problem)
        {
            Random rnd = new Random();
            Agent[] population = new Agent[PopulationSize];
            double[] bestGlobalPosition = new double[problem.Dimension];

            Array.Sort(population);
            Group[] groups = new Group[GroupCount];
            for(int i = 0; i < GroupCount; i++)
            {
                double[] randomPosition = new double[problem.Dimension];
                for (int j = 0; j < randomPosition.Length; j++)
                    randomPosition[j] = (problem.MaxX - problem.MinX) * rnd.NextDouble() + problem.MinX;

                double error = problem.ErrorFunction(randomPosition);

                groups[i] = new Group(new Agent(randomPosition, error), PopulationSize / GroupCount);
            }

            int currentIteration = 0;
            while(currentIteration < MaxIterations)
            {
                for (int i = 0; i < GroupCount; i++)
                {
                    double minDistance = double.MaxValue;

                    double minError = double.MaxValue;
                    double minError2 = double.MaxValue;

                    double[] minCoords = new double[problem.Dimension];
                    double[] minCoords2 = new double[problem.Dimension];

                    for (int j = 0; j < GroupCount; j++)
                    {
                        if (i != j)
                        {
                            double distance = groups[i].Head.distanceTo(groups[j].Head);
                            if (minDistance > distance)
                            {
                                minDistance = distance;
                            }
                        }
                    }

                    for (int j = 1; j < groups[i].GroupSize; j++)
                    {
                        double[] randomPosition = new double[problem.Dimension];
                        for (int k = 0; k < randomPosition.Length; k++)
                            randomPosition[k] = 2 * minDistance * rnd.NextDouble() - minDistance + groups[i].Head.Coords[k];

                        double error = problem.ErrorFunction(randomPosition);

                        if (error < minError)
                        {
                            minError2 = minError;
                            minCoords.CopyTo(minCoords2, 0);

                            minError = error;
                            randomPosition.CopyTo(minCoords, 0);
                        }
                        else if (error < minError2)
                        {
                            minError2 = error;
                            randomPosition.CopyTo(minCoords2, 0);
                        }
                    }

                    if (minError < groups[i].Head.Error)
                    {
                        if (minError2 < groups[i].Head.Error)
                        {
                            minCoords2.CopyTo(groups[i].Tail.Coords, 0);
                            groups[i].Tail.Error = minError2;
                        }
                        else
                        {
                            groups[i].Head.Coords.CopyTo(groups[i].Tail.Coords, 0);
                            groups[i].Tail.Error = groups[i].Head.Error;
                        }

                        minCoords.CopyTo(groups[i].Head.Coords, 0);
                        groups[i].Head.Error = minError;
                    }
                    else
                    {
                        minCoords.CopyTo(groups[i].Tail.Coords, 0);
                        groups[i].Tail.Error = minError;
                    }
                }

                Array.Sort(groups);
                groups[0].Head.Coords.CopyTo(bestGlobalPosition, 0);
                if (groups[0].Head.Error < AcceptableError)
                    break;

                int removedAgents = groups[GroupCount - 1].GroupSize * 4 / 10;
                groups[GroupCount - 1].GroupSize -= removedAgents;
                if (groups[GroupCount - 1].GroupSize < 2)
                {
                    groups[0].Tail.Coords.CopyTo(groups[GroupCount - 1].Head.Coords, 0);
                    groups[GroupCount - 1].Head.Error = groups[0].Tail.Error;

                    groups[0].GroupSize = groups[0].GroupSize / 2 + groups[0].GroupSize % 2;
                    groups[GroupCount - 1].GroupSize = groups[0].GroupSize /= 2;
                }
                else
                {
                    groups[0].GroupSize += removedAgents;
                }
                currentIteration++;
            }
            
            return bestGlobalPosition;
        }
    }
}
