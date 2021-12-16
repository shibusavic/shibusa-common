using System.Text;

string maleNameFileName = Path.Combine(Path.GetTempPath(), "male-names.txt");
string femaleNameFileName = Path.Combine(Path.GetTempPath(), "female-names.txt");

var maleNameStream = File.Create(maleNameFileName);
var femaleNameStream = File.Create(femaleNameFileName);

string[] lines = File.ReadAllLines("data\\yob2018.txt");

foreach (string line in lines)
{
    string[] split = line.Split(',');

    byte[] buffer = Encoding.UTF8.GetBytes(split[0] + Environment.NewLine);
    if (split[1] == "M")
    {
        await maleNameStream.WriteAsync(buffer);
    }
    else if (split[1] == "F")
    {
        await femaleNameStream.WriteAsync(buffer);
    }
    else
    {
        Console.WriteLine($"Unexpected gender found: {split[1]}");
    }
}

await maleNameStream.FlushAsync();
await femaleNameStream.FlushAsync();

maleNameStream.Close();
femaleNameStream.Close();

Console.WriteLine($"File {maleNameFileName} created.");
Console.WriteLine($"File {femaleNameFileName} created.");