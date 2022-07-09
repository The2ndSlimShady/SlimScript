using System.Text;
using System;

namespace SlimScript;

internal class PreProcessor
{
    public static string[] Process(string[] source)
    {
        List<string> processedSource = new();

        bool isString = false;
        StringBuilder token = new();

        for (int i = 0; i < source.Length; i++)
        {
            string element = source[i];

            if (element.StartsWith("--"))
                continue;

            for (int j = 0; j < element.Length; j++)
            {
                char item = element[j];

                if (isString)
                {
                    token.Append(item);

                    if (item == '"')
                        isString = false;
                }
                else if (item == ' ')
                {
                    string val = token.ToString();

                    if (!string.IsNullOrEmpty(val))
                    {
                        processedSource.Add(val);
                        token = new();
                    }
                }
                else
                {
                    if (item == '-' && element[j + 1] == '-')
                        break;
                    if (item == '"')
                        isString = true;

                    token.Append(item);
                }
            }

            if (token.Length != 0)
                processedSource.Add(token.ToString());

            processedSource.Add("EOL");
            isString = false;
            token = new();
        }

#if DEBUG
        StringBuilder b = new();
        bool isIndent = true;
        foreach (var item in processedSource)
        {
            if (item != "EOL")
            {
                b.Append($" {item}");
                isIndent = false;
            }
            else
            {
                b.Append(Environment.NewLine);
                isIndent = true;
            }
        }

        File.WriteAllText("post_process.ss", b.ToString());
#endif

        return processedSource.ToArray();
    }
}
