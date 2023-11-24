using System;
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
            var lines = new string[6000];
            var count = 0;
            var i = 0;

            using (StreamReader streamReader = new StreamReader(input, encoding))
            {
                while (count < 2)
                {
                    string line = await streamReader.ReadLineAsync();

                    if (line != null)
                    {
                        if (line.Contains("Subtitle by : Amir Barzagli"))
                        {
                            count++;
                        }

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

                        lines[i] = line;

                    }
                    else
                    {
                        break;
                    }

                    i++;
                }
            }

            using (StreamWriter streamWriter = new StreamWriter(output, encoding))
            {
                foreach (string line in lines)
                {
                    if (line != null)
                    {
                        await streamWriter.WriteLineAsync(line);
                    }
                }
            }
        }
    }
}