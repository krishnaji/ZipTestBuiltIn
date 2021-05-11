using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;

namespace ZipTestBuiltIn
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputfile = @"F:\testfile.dat";
            string outputfile = @"F:\outfile.zip";

            int readSize = 5;
            readSize *= 1024 * 1024;

            int compression = 1;
            BuiltIn(inputfile, outputfile, readSize, compression);
        }
        private static void BuiltIn(string inputFile, string outputFile, int readSize, int compression)
        {
            System.Byte[] buffer = new System.Byte[readSize];

            DateTime start = DateTime.Now;
            double kbIncrement = readSize / 1000;
            double kbWritten = 0;

            Stopwatch readWatch = new Stopwatch();
            Stopwatch writeWatch = new Stopwatch();

            using (FileStream fsOut = new FileStream(outputFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None, readSize))
            using (var zipArchive = new ZipArchive(fsOut, ZipArchiveMode.Create))
            {
                ZipArchiveEntry entry = zipArchive.CreateEntry(Path.GetFileName(inputFile), CompressionLevel.Optimal);
                using (Stream zipStream = entry.Open())
                using (FileStream inputStream = File.OpenRead(inputFile))
                {
                    int bytesRead = 0;
                    do
                    {
                        readWatch.Start();
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        readWatch.Stop();

                        writeWatch.Start();
                        zipStream.Write(buffer, 0, bytesRead);
                        writeWatch.Stop();

                        kbWritten += kbIncrement;
                        TimeSpan sofar = DateTime.Now - start;
                        Console.WriteLine("{0} - {1} - {2} - {3} - {4}",
                            kbWritten.ToString("N0"), sofar.ToString(), (kbWritten / sofar.TotalSeconds).ToString("N0")
                            , readWatch.Elapsed.ToString(), writeWatch.Elapsed.ToString());

                    } while (bytesRead > 0);
                }
            }
        }



    }
}
