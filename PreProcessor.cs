using System.Text;
using System;
namespace SlimScript;

internal class PreProcessor
{
    public static string[] Process(string[] source)
    {
        List<string> processedSource = new();

        foreach (string element in source)
        {
            if (element.StartsWith("--"))
                continue;
            else
            {
                foreach (string item in element.Split(" "))
                {
                    if (string.IsNullOrEmpty(item) || string.IsNullOrWhiteSpace(item))
                        continue;
                    else
                       processedSource.Add(item);
                }

                processedSource.Add("EOL");
            }
        }

        return processedSource.ToArray();
    }
}