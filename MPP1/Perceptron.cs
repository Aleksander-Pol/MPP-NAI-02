using System.Globalization;

namespace MPP1;

public class Perceptron (double[] weightVector, double bias)
{
    private double[] WeightVector{get;} = weightVector;
    public double Bias { get; set; } = bias;

    public int Classify(double[] x)
    {
        double sum = 0.0;
        if (x.Length != WeightVector.Length)
        {
            Console.WriteLine("Różne długości - program się nie wykona");
            return 67;
        }
        else
        {
            for (int i = 0; i < x.Length; i++)
                sum +=x[i] * WeightVector[i];
            
            return sum > Bias? 1:0;
        }
    }

    public void Learn(double[] x, int d, double a, double b)
    {
        int y = Classify(x);
        int pom = d - y;
        double subPartWeightVector = pom * a;
        double subPartTheta = pom * b;

        for (int i = 0; i < x.Length; i++)
            WeightVector[i] += x[i] * subPartWeightVector;
        
        Bias -= subPartTheta;
    }

    public override string ToString()
    {
        foreach (var VARIABLE in WeightVector)
        {
            Console.Write(VARIABLE + " ");
        }

        return " -> " +  Bias.ToString(CultureInfo.InvariantCulture);
    }
}