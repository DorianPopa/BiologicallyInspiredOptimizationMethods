using BioInspiredOptimization.Problems;

namespace BioInspiredOptimization.Solvers
{
    public interface ISolver
    {
        public double[] Solve(IProblem problem);
    }
}
