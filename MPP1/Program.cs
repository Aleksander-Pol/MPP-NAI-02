using System.Globalization;

namespace MPP1;

class Program
{
    static void Main(string[] args)
    {
        string text = File.ReadAllText("iris.txt");
        
        string[] lines = text.Split('\n');
        double[][] iris_vals = new double[lines.Length][];
        

        for (int i = 1; i < lines.Length; i++)
        {
            string[] tempArr= lines[i].Split(',');
            

            iris_vals[i] = new double[4];
            for (int j = 0; j < 4; j++)
            {
                iris_vals[i][j] = double.Parse(tempArr[j], CultureInfo.InvariantCulture);
                Console.WriteLine(iris_vals[i][j]);
            }
            
        }
    }
}

public static class Calssifier
{
    private static List<int>[] iris_values;



    public static void FormatString(string text)
    {
        string[] lines = text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            double[] tempArr= lines[i].Split(',').Select(double.Parse).ToArray();
            Console.WriteLine(tempArr[i]);
        }
    }
    
    
    public static double Distance(double[] a,  double[] b)
    {
        if (a.Length == b.Length)
        {
            double dist = 0.0;

            for (int i = 0; i < a.Length; i++)
                dist += Math.Pow(Math.Abs(a[i] - b[i]), 2);

            return Math.Sqrt(dist);
        }
        else
            return -1.0;
    }
}