namespace BioInspiredOptimization.Individual
{
    class Particle : Individual
    {
        public double Error;
        public double[] Velocity;
        public double[] BestPosition;
        public double BestError;

        public Particle(double[] pos, double err, double[] vel, double[] bestPos, double bestErr)
        {
            Coords = new double[pos.Length];
            pos.CopyTo(Coords, 0);
            Error = err;
            Velocity = new double[vel.Length];
            vel.CopyTo(Velocity, 0);
            BestPosition = new double[bestPos.Length];
            bestPos.CopyTo(BestPosition, 0);
            BestError = bestErr;
        }

        public override string ToString()
        {
            string s = "";
            s += "Position: ";
            for (int i = 0; i < Coords.Length; ++i)
                s += Coords[i].ToString("F10") + " ";
            s += "\n";
            s += "Error = " + Error.ToString("F10") + "\n";
            s += "Velocity: ";
            for (int i = 0; i < Velocity.Length; ++i)
                s += Velocity[i].ToString("F10") + " ";
            s += "\n";
            s += "Best Position: ";
            for (int i = 0; i < BestPosition.Length; ++i)
                s += BestPosition[i].ToString("F10") + " ";
            s += "\n";
            s += "Best Error = " + BestError.ToString("F10") + "\n";
            return s;
        }
    }
}
