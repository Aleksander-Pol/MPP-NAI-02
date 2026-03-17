using System.Globalization;
using System.Security.AccessControl;

namespace MPP1;

class Program
{
    static void Main(string[] args)
    {
        Classifier classifier = new Classifier("iris.txt");
        classifier.trainData();

    }
}

public class Classifier
{
    private  double[][] _irisVals = null!;
    private  string[] _irisNames = null!;
    private string baseFileName { get; set; }

    public Classifier(string baseFileName)
    {
        this.baseFileName = baseFileName;
    }
    
    public  string LoadBaseFile()
    {
        return File.ReadAllText(baseFileName);
    }
    
    public void FormatFile()
    {
        string text = LoadBaseFile();
        string[] lines = text.Split('\n');
        _irisVals = new double[lines.Length-1][];
        _irisNames = new string[lines.Length-1];
        
        for (int i = 1; i < lines.Length; i++)
        {
            string[] tempArr= lines[i].Split(',');
                        

            _irisVals[i-1] = new double[4];
            for (int j = 0; j < 4; j++)
            {
                _irisVals[i-1][j] = double.Parse(tempArr[j], CultureInfo.InvariantCulture);
            }
            
            
            _irisNames[i-1] = tempArr[4].Trim();
        }
        
    }
    
    
    public  double Distance(double[] a,  double[] b)
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

    public  void ClassifyVector(double[] newVector)
    {
        Console.WriteLine(Knn(10,  newVector));
    }

    public  void ClassifyFile(string fileName)
    {
        string text = File.ReadAllText(fileName);
        var lines = text.Split("\r\n");
        var userVals  = new double[lines.Length][];
        
        for (int i = 0; i < lines.Length; i++)
        {
            string[] tempArr= lines[i].Split(',');
            
            userVals[i] = new double[4];
            for (int j = 0; j < 4; j++)
                userVals[i][j] = double.Parse(tempArr[j], CultureInfo.InvariantCulture);

            Console.WriteLine(Knn(10, userVals[i]));
                
        }
        

    }

    public  string Knn(int k, double[] newVector)
    {
        double[] distances = new double[_irisNames.Length];
        Dictionary<string, int> irisResArr = new Dictionary<string, int>()
        {
            { "Iris-setosa", 0},
            { "Iris-versicolor", 0},
            { "Iris-virginica", 0},
        };

        for (int i = 0; i < distances.Length; i++)
        {
            distances[i] = Distance(_irisVals[i], newVector);
            for (int j = 0; j < i; j++)
            {
                if (distances[i] < distances[j])
                {
                    (distances[i], distances[j]) = (distances[j], distances[i]);
                    (_irisNames[i], _irisNames[j]) =  (_irisNames[j], _irisNames[i]);
                    (_irisVals[i], _irisVals[j]) =  (_irisVals[j], _irisVals[i]);
                }
            }
        }
        
        for (int i = 0; i < k; i++)
            irisResArr[_irisNames[i]]++;

        var maxKvp = irisResArr.MaxBy(x => x.Value);
        
        return maxKvp.Key;
    }

    public void trainData()
    {
        FormatFile();

        List<double[]> trainingSetVals = new List<double[]>();
        List<string> trainingSetNames = new List<string>();
        List<double[]> testSetVals = new List<double[]>();
        List<string> testSetNames = new List<string>();

        for (int i = 0; i < _irisNames.Length; i++)
        {
            if (i % 50 < 30)
            {
                trainingSetVals.Add(_irisVals[i]);
                trainingSetNames.Add(_irisNames[i]);
            }
            else
            {
                testSetVals.Add(_irisVals[i]);
                testSetNames.Add(_irisNames[i]);
            }
        }
    }
        
}