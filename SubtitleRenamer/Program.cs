using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SubtitleRenamer
{
    public class Program
    {
        private const bool WriteToConsole = false;

        public static void Main(string[] args)
        {
            try
            {
                var fileInfos = args.Select(x => new FileInfo(x)).ToList();

                PrintArgumentsToConsole(fileInfos);

                FileInfo mediaFile = GetMediaFile(fileInfos);

                if (mediaFile != null)
                {
                    var nonMediaFiles = fileInfos.Except(new List<FileInfo> {mediaFile});

                    RenameFiles(mediaFile, nonMediaFiles);
                }
                else
                {
                    WriteTextToConsole("No media file found");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION:" + Environment.NewLine);
                Console.WriteLine(e);
                Console.ReadKey();
            }

            //Console.ReadKey();
        }

        private static void PrintArgumentsToConsole(List<FileInfo> args)
        {
            foreach (var s in args)
            {
                WriteTextToConsole(s.FullName);
            }
        }

        private static FileInfo GetMediaFile(IEnumerable<FileInfo> fileInfos)
        {
            var mediaFileExtensions = new List<string>
            {
                ".mkv", ".avi", ".mp4", ".mpg", ".mpeg"
            };

            return fileInfos.FirstOrDefault(x => mediaFileExtensions.Contains(x.Extension));
        }

        private static void RenameFiles(FileInfo mediaFile, IEnumerable<FileInfo> nonMediaFiles)
        {
            string mediaFileNameWithoutExtension = Path.GetFileNameWithoutExtension(mediaFile.FullName);

            foreach (var nonMediaFile in nonMediaFiles)
            {
                string directory = nonMediaFile.DirectoryName;
                string newFileName = mediaFileNameWithoutExtension + nonMediaFile.Extension;

                string newFullName = Path.Combine(directory, newFileName);

                WriteTextToConsole(nonMediaFile.FullName + " -> " + newFullName);

                File.Move(nonMediaFile.FullName, newFullName);
            }
        }

        private static void WriteTextToConsole(string text)
        {
            if (WriteToConsole)
            {
                Console.WriteLine(text);
            }
        }
    }
}
