using BioInspiredOptimization.Individual;
using BioInspiredOptimization.Problems;
using System;

namespace BioInspiredOptimization.Solvers
{
    class ParticleSwarming : ISolver
    {
        private int NumberOfParticles;
        private int MaxIterations;
        private double AcceptableError;

        public ParticleSwarming(int numberParticles, int maxIterations, double acceptableError)
        {
            NumberOfParticles = numberParticles;
            MaxIterations = maxIterations;
            AcceptableError = acceptableError;
        }

        public double[] Solve(IProblem problem)
        {
            Random rnd = new Random();
            Particle[] swarm = new Particle[NumberOfParticles];
            double[] bestGlobalPosition = new double[problem.Dimension]; // best solution found by any particle in the swarm
            double bestGlobalError = double.MaxValue;

            // init swarm
            for (int i = 0; i < swarm.Length; i++)
            {
                double[] randomPosition = new double[problem.Dimension];
                for (int j = 0; j < randomPosition.Length; j++)
                    randomPosition[j] = (problem.MaxX - problem.MinX) * rnd.NextDouble() + problem.MinX;

                double error = problem.ErrorFunction(randomPosition);
                double[] randomVelocity = new double[problem.Dimension];

                for (int j = 0; j < randomVelocity.Length; j++)
                {
                    double low = problem.MinX * 0.1;
                    double high = problem.MaxX * 0.1;
                    randomVelocity[j] = (high - low) * rnd.NextDouble() + low;
                }
                swarm[i] = new Particle(randomPosition, error, randomVelocity, randomPosition, error);

                if (swarm[i].Error < bestGlobalError)
                {
                    bestGlobalError = swarm[i].Error;
                    swarm[i].Coords.CopyTo(bestGlobalPosition, 0);
                }
            }

            double inertia = 0.729;         // inertia weight
            double localWeight = 1.49445;   // local weight
            double globalWeight = 1.49445;  // global weight
            double localRandom, globalRandom;                  // local and global weights randomizations
            double probabilityOfDeath = 0.01;
            int currentIteration = 0;
            bool acceptableErrorReached = false;

            double[] newVelocity = new double[problem.Dimension];
            double[] newPosition = new double[problem.Dimension];
            double newError;

            while (currentIteration < MaxIterations && !acceptableErrorReached)
            {
                foreach (Particle currentParticle in swarm)
                {
                    // calculate new velocity (displacement of the particle)
                    for (int i = 0; i < currentParticle.Velocity.Length; i++)
                    {
                        localRandom = rnd.NextDouble();
                        globalRandom = rnd.NextDouble();

                        newVelocity[i] = (inertia * currentParticle.Velocity[i]) +
                            (localWeight * localRandom * (currentParticle.BestPosition[i] - currentParticle.Coords[i])) +
                            (globalWeight * globalRandom * (bestGlobalPosition[i] - currentParticle.Coords[i]));
                    }
                    newVelocity.CopyTo(currentParticle.Velocity, 0);

                    // calculate new position after displacement
                    for (int i = 0; i < currentParticle.Coords.Length; i++)
                    {
                        newPosition[i] = currentParticle.Coords[i] + newVelocity[i];
                        if (newPosition[i] < problem.MinX)      // new position out of min-bound
                            newPosition[i] = problem.MinX;
                        if (newPosition[i] > problem.MaxX)      // new position out of max-bound
                            newPosition[i] = problem.MaxX;
                    }
                    newPosition.CopyTo(currentParticle.Coords, 0);

                    // evaluate the error with the new params
                    newError = problem.ErrorFunction(newPosition);
                    currentParticle.Error = newError;

                    if (newError < currentParticle.BestError)    // is newPosition a local best
                    {
                        newPosition.CopyTo(currentParticle.BestPosition, 0);
                        currentParticle.BestError = newError;
                    }
                    if (newError < bestGlobalError)              // is newPosition a global best
                    {
                        newPosition.CopyTo(bestGlobalPosition, 0);
                        bestGlobalError = newError;
                    }
                    if (currentParticle.Error < AcceptableError)
                    {
                        acceptableErrorReached = true;
                        break;
                    }

                    // particle died so replace it with another one with random starting position
                    double death = rnd.NextDouble();
                    if (death < probabilityOfDeath)
                    {
                        for (int i = 0; i < currentParticle.Coords.Length; i++)
                            currentParticle.Coords[i] = (problem.MaxX - problem.MinX) * rnd.NextDouble() + problem.MinX;
                        currentParticle.Error = problem.ErrorFunction(currentParticle.Coords);
                        currentParticle.Coords.CopyTo(currentParticle.BestPosition, 0);
                        currentParticle.BestError = currentParticle.Error;

                        if (currentParticle.Error < bestGlobalError) // randomly got a new global best 
                        {
                            bestGlobalError = currentParticle.Error;
                            currentParticle.Coords.CopyTo(bestGlobalPosition, 0);
                        }
                        if (currentParticle.Error < AcceptableError)
                        {
                            acceptableErrorReached = true;
                            break;
                        }
                    }
                }
                currentIteration++;
            }

            // reached maxIterations or acceptableError
            Console.WriteLine("Finished");
            for (int i = 0; i < swarm.Length; i++)
            {
                Console.WriteLine("Particle: " + i);
                Console.WriteLine(swarm[i].ToString());     // print the particles
            }

            double[] finalResult = new double[problem.Dimension];
            bestGlobalPosition.CopyTo(finalResult, 0);
            return finalResult;                             // return the best global result
        }
    }
}
