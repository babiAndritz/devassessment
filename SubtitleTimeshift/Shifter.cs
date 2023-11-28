using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleTimeshift
{
    public class Shifter
    {
        async static public Task Shift(Stream input, Stream output, TimeSpan timeSpan, Encoding encoding,
            int bufferSize = 1024, bool leaveOpen = false)
        {
            List<string> lines = new List<string>();

            using (StreamReader streamReader = new StreamReader(input, encoding))
            using (StreamWriter streamWriter = new StreamWriter(output, encoding))
            {

                var line = default(string);

                while (null != (line = await streamReader.ReadLineAsync()))
                {
                    if (line.Contains("-->"))
                    {
                        var subs = line.Split('>');
                        var firstTime = subs[0].Replace("-", "").Trim();
                        var secondTime = subs[1].TrimStart();

                        var newFirstTime = TimeSpan.Parse(firstTime);
                        newFirstTime += timeSpan;
                        var formatedFirstTime = newFirstTime.ToString(@"hh\:mm\:ss\.fff");
                        line = line.Replace(firstTime, formatedFirstTime);

                        var newSecondTime = TimeSpan.Parse(secondTime);
                        newSecondTime += timeSpan;
                        var formatedSecondTime = newSecondTime.ToString(@"hh\:mm\:ss\.fff");
                        line = line.Replace(secondTime, formatedSecondTime);

                    }

                    lines.Add(line);
                }

                foreach (string l in lines)
                {
                    if (l != null)
                    {
                        await streamWriter.WriteLineAsync(l);
                    }
                }
            }

        }
    }
}