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

                        var subs = line.Substring(0, 12);
                        var time = TimeSpan.Parse(subs);

                        time += timeSpan;
                        var formatedTime = time.ToString(@"hh\:mm\:ss\.fff");
                        line = line.Replace(subs, formatedTime);

                        subs = line.Substring(line.LastIndexOf('>') + 2);
                        time = TimeSpan.Parse(subs);

                        time += timeSpan;
                        formatedTime = time.ToString(@"hh\:mm\:ss\.fff");
                        line = line.Replace(subs, formatedTime);

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