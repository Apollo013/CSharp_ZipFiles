using System;
using System.IO;

namespace Common
{
    public static class Common
    {
        /// <summary>
        /// Checks that the directoy exists, and creates it if it does not.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static bool DirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Create some arbitrary text files
        /// </summary>
        /// <param name="directory"></param>
        public static void CreateFilesToZip(string directory)
        {
            // File 1
            string[] lines = { "First line", "Second line", "Third line" };
            File.WriteAllLines($"{directory}\\File1.txt", lines);

            // File 2
            string text = "A class is the most powerful data type in C#. Like a structure, a class defines the data and behavior of the data type. ";
            File.WriteAllText($"{directory}\\File2.txt", text);
        }

        /// <summary>
        /// Removes all files from specified directory
        /// </summary>
        /// <param name="directory"></param>
        public static void DeleteFilesFromDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                Array.ForEach(Directory.GetFiles(directory), delegate (string path) { File.Delete(path); });
            }
        }

        /// <summary>
        /// Prints a list of file in directory
        /// </summary>
        /// <param name="directory"></param>
        public static void PrintFiles(string title, string directory)
        {
            string divider = new string('=', 80);
            System.Console.WriteLine(divider);
            System.Console.WriteLine(title);
            System.Console.WriteLine(divider);

            Array.ForEach(Directory.GetFiles(directory, "*", SearchOption.AllDirectories), delegate (string path) { Console.WriteLine(path); });

            System.Console.WriteLine();
        }
    }
}
