using System.Globalization;

namespace MPP1;

class Program
{
    static void Main(string[] args)
    {
        string text = File.ReadAllText("iris.txt");
        double[][] irisVals = Classifier.FormatString(text);

        Classifier.KNN(5, new double[]{3.0, 2.0, 1.0, 0.0});

    }
}

public class Classifier
{
    private static double[][] irisVals;
    private static string[] irisNames;
    
    
    public static double[][] FormatString(string text)
    {
        
        string[] lines = text.Split('\n');
        irisVals = new double[lines.Length-1][];
        irisNames = new string[lines.Length-1];
        
        for (int i = 1; i < lines.Length; i++)
        {
            string[] tempArr= lines[i].Split(',');
                        

            irisVals[i-1] = new double[4];
            for (int j = 0; j < 4; j++)
            {
                irisVals[i-1][j] = double.Parse(tempArr[j], CultureInfo.InvariantCulture);
            }
            
            
            irisNames[i-1] = tempArr[4].Trim();
        }
        
        
        return irisVals;
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


    public static void KNN(int k, double[] newVector)
    {
        double[] distances = new double[irisNames.Length];
        Dictionary<string, int> irisResArr = new Dictionary<string, int>()
        {
            { "Iris-setosa", 0},
            { "Iris-versicolor", 0},
            { "Iris-virginica", 0},
        };

        for (int i = 0; i < distances.Length; i++)
        {
            distances[i] = Distance(irisVals[i], newVector);
            for (int j = 0; j < i; j++)
            {
                if (distances[i] < distances[j])
                {
                    (distances[i], distances[j]) = (distances[j], distances[i]);
                    (irisNames[i], irisNames[j]) =  (irisNames[j], irisNames[i]);
                    (irisVals[i], irisVals[j]) =  (irisVals[j], irisVals[i]);
                }
            }
        }
        
        for (int i = 0; i < k; i++)
            irisResArr[irisNames[i]]++;
        
        
        var maxKVP = irisResArr.MaxBy(x => x.Value);
        Console.WriteLine(maxKVP.Key);
        
    }
}