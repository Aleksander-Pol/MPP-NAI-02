namespace MPP1;

class Program
{
    static void Main(string[] args)
    {
        string text = File.ReadAllText("iris.txt");
        Console.WriteLine(text);
    }
}