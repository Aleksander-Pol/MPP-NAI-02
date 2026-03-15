namespace MPP1;

class Program
{
    static void Main(string[] args)
    {
        string text = File.ReadAllText("iris.txt");
        Console.WriteLine(text);
    }
}

class Calssifier
{
    private static double[][] iris;
    
    
    public double Distance(double[] a,  double[] b)
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