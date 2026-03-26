using System.Globalization;

namespace MPP1;

class Program
{
    static void Main(string[] args)
    {
        Classifier classifier = new Classifier("iris.txt");
        classifier.ReadInput();
    }
}

public class Classifier (string baseFileName)
{
    private double[][] IrisVals { get; set; } = null!;
    private int[] IrisResVals { get; set; }= null!;
    private  string[] _irisNames = null!;
    
    
    private string BaseFileName { get; set; } = baseFileName;
    
    private string LoadBaseFile()
    {
          return File.ReadAllText(BaseFileName);
    }

    public void ReadInput()
    {
        string? input = "";
        FormatFile();
        
        while (input?.ToLower() != "quit")
        {
            Console.WriteLine("\nMENU\n==========================");
            Console.WriteLine("[1] - Classify single vector\t[2] - Classify vectors from file\t[3] - Check accuracy\t[4] - Test perceptron");
            input = Console.ReadLine();
            
            switch (input)
            {
                case "1":
                    Console.WriteLine("Enter vector: ");
                    string? givenVector =  Console.ReadLine();
                    
                    string[]? sepString = givenVector?.Split(',');
                    if (sepString != null)
                    {
                        double[] newVector = new double[sepString.Length];
                    
                        for (int i = 0; i<sepString.Length; i++)
                            newVector[i] = double.Parse(sepString[i], CultureInfo.InvariantCulture);
                    
                        ClassifyVector(newVector);
                    }

                    break;
                
                case "2":
                    Console.WriteLine("Enter file name: ");
                    string? fileName = Console.ReadLine();
                    if (fileName != null) ClassifyFile(fileName);
                    Console.WriteLine("File has been created");
                    break;
                
                case "3":
                    Console.WriteLine("Enter k for training");
                    int k =  int.Parse(Console.ReadLine() ?? string.Empty);
                    TrainData(k);
                    break;
                case "4":
                    Console.WriteLine("Enter number of eras: ");
                    int repetitions = Int32.Parse(Console.ReadLine() ?? string.Empty);
                    Console.WriteLine("Enter weight vector: ");
                    string weightVector = Console.ReadLine();
                    double [] weightVectorArray = weightVector.Split(',').Select(x => double.Parse(x,CultureInfo.InvariantCulture)).ToArray();
                    Console.WriteLine("Enter bias: ");
                    double bias = double.Parse(Console.ReadLine() ?? string.Empty);
                    Perceptron p = new Perceptron(weightVectorArray, bias);
                    Console.WriteLine("Enter alfa: ");
                    double a =  double.Parse(Console.ReadLine() ?? string.Empty, CultureInfo.InvariantCulture);
                    Console.WriteLine("Enter beta: ");
                    double b = double.Parse(Console.ReadLine() ?? string.Empty, CultureInfo.InvariantCulture);
                    
                    TestPerceptronAccuracy(repetitions, p, a, b);
                    break;
                    
            }
        }
    }

    public double[][] FormatPerceptronText()
    {
        string text = LoadBaseFile();
        string[] lines = text.Split('\n');
        IrisVals = new double[lines.Length-1][];
        IrisResVals = new int[lines.Length-1];
        

        for (int i = 1; i < lines.Length; i++)
        {
            string[] tempArr = lines[i].Split(',');
            IrisVals[i-1] = new double[4];

            for (int j = 0; j < 4; j++)
            {
                IrisVals[i-1][j] = double.Parse(tempArr[j], CultureInfo.InvariantCulture);
            }

            IrisResVals[i - 1] = tempArr[4].Trim() == "Iris-setosa" ? 1 : 0;
        }

        return IrisVals;
    }

    
    
    private void FormatFile()
    {
        string text = LoadBaseFile();
        string[] lines = text.Split('\n');
        IrisVals = new double[lines.Length-1][];
        _irisNames = new string[lines.Length-1];
        
        for (int i = 1; i < lines.Length; i++)
        {
            string[] tempArr= lines[i].Split(',');
            
            IrisVals[i-1] = new double[4];
            for (int j = 0; j < 4; j++)
            {
                IrisVals[i-1][j] = double.Parse(tempArr[j], CultureInfo.InvariantCulture);
            }
            _irisNames[i-1] = tempArr[4].Trim();
        }
    }

    private double Distance(double[] a,  double[] b)
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

    private void ClassifyVector(double[] newVector)
    {
        Console.WriteLine(Knn(3,  newVector, IrisVals,  _irisNames));
    }

    private void ClassifyFile(string fileName)
    {
        File.WriteAllText("result.txt","");
        string text = File.ReadAllText(fileName);
        Console.WriteLine(text);
        var lines = text.Split("\r\n");
        var userVals  = new double[lines.Length][];
        
        for (int i = 0; i < lines.Length; i++)
        {
            string[] tempArr= lines[i].Split(',');
            
            userVals[i] = new double[4];
            for (int j = 0; j < 4; j++)
                userVals[i][j] = double.Parse(tempArr[j], CultureInfo.InvariantCulture);

            File.AppendAllText("result.txt", Knn(10, userVals[i], IrisVals, _irisNames)+"\n");
                
        }
        

    }

    private string Knn(int k, double[] newVector, double[][] values, string[] names)
    {
        
        var copyNames = names.Clone() as string[];
        double[] distances = new double[names.Length];
        Dictionary<string?, int> irisResMap = new Dictionary<string?, int>()
        {
            { "Iris-setosa", 0},
            { "Iris-versicolor", 0},
            { "Iris-virginica", 0},
        };
        
        for (int i = 0; i < distances.Length; i++)
        {
            distances[i] = Distance(values[i], newVector);
            for (int j = 0; j < i; j++)
            {
                if (distances[i] < distances[j])
                {
                    (distances[i], distances[j]) = (distances[j], distances[i]);
                    if (copyNames != null) (copyNames[i], copyNames[j]) = (copyNames[j], copyNames[i]);
                }
            }
        }
        
        for (int i = 0; i < k; i++)
            irisResMap[copyNames?[i]]++;

        var maxKvp = irisResMap.MaxBy(x => x.Value);
        
        return maxKvp.Key;
    }

    public void TestPerceptronAccuracy(int repetitions, Perceptron p, double a, double b)
    {
        FormatPerceptronText();
        
        List<double[]> trainingSetVals = new List<double[]>();
        List<int> trainingSetResults = new List<int>();
        List<double[]> testSetVals = new List<double[]>();
        List<int> testSetResults = new List<int>();

        int sum = 0;
        
        for (int i = 0; i < IrisVals.Length; i++)
        {
            if (i % 50 < 30)
            {
                trainingSetVals.Add(IrisVals[i]);
                trainingSetResults.Add(IrisResVals[i]);
            }
            else
            {
                testSetVals.Add(IrisVals[i]);
                testSetResults.Add(IrisResVals[i]);
            }
        }
        
        
        for (int i = 0; i < repetitions; i++)
        {
            for (int j = 0; j < trainingSetVals.Count; j++)
                p.Learn(trainingSetVals[j], trainingSetResults[j], a, b);
        }
        
        for (int i = 0; i < testSetVals.Count; i++)
            if (p.Classify(testSetVals[i]) == testSetResults[i]) sum++;
        
        double result = ((double)sum / testSetVals.Count) * 100;
        Console.WriteLine($"Udało się zdobyć - {result}%");
    }

    private void TrainData(int k)
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
                trainingSetVals.Add(IrisVals[i]);
                trainingSetNames.Add(_irisNames[i]);
            }
            else
            {
                testSetVals.Add(IrisVals[i]);
                testSetNames.Add(_irisNames[i]);
            }
        }

        List<int> resultTable = new List<int>();
        for (int i = 0; i < testSetVals.Count; i++)
        {
            if (Knn(k, testSetVals[i], trainingSetVals.ToArray(), trainingSetNames.ToArray()) == testSetNames[i])
            {
                resultTable.Add(1);
            }
            else
                resultTable.Add(0);
        }
        
        double result = resultTable.Sum()/(double)resultTable.Count * 100;

        Console.WriteLine($"Udało się zdobyć - {result}%");
    }
        
}