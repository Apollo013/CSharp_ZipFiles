using System;
using System.IO;
using System.IO.Compression;
using static Common.Common;

namespace GZipFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            PackFiles();
            UnPackFiles();
        }

        private static void PackFiles()
        {
            /*---------------------------------------------------------------------------
             * Make sure that the source directory exists
            ---------------------------------------------------------------------------*/
            var sourceFolder = Directory.GetCurrentDirectory() + "\\Files_To_Pack";
            if (!DirectoryExists(sourceFolder) || Directory.GetFiles(sourceFolder).Length < 1)
            {
                CreateFilesToZip(sourceFolder);// Create arbitrary files to zip
            }

            /*---------------------------------------------------------------------------
             * Make sure the destination directory exists
            ---------------------------------------------------------------------------*/
            var destinationFolder = Directory.GetCurrentDirectory() + "\\Packed_Files";
            if (DirectoryExists(destinationFolder))
            {
                DeleteFilesFromDirectory(destinationFolder);// Remove any existing zip files
            }

            /*---------------------------------------------------------------------------
             * Grab ALL files TO ZIP
            ---------------------------------------------------------------------------*/
            DirectoryInfo sourceFolderInfo = new DirectoryInfo(sourceFolder);
            FileInfo[] files = sourceFolderInfo.GetFiles();
            PrintFiles("FILES TO PACK", sourceFolder);

            /*---------------------------------------------------------------------------
             * Zip em'
            ---------------------------------------------------------------------------*/
            foreach (var fileToZip in files)
            {
                // This will be the zip file
                var zipFile = new FileInfo($"{destinationFolder}\\{fileToZip.Name}.gz");

                // Read the source file
                using (FileStream srcStream = fileToZip.OpenRead())
                {
                    // Create the zip file
                    using (FileStream destStream = zipFile.Create())
                    {
                        // Copy src to zip and compress
                        using (GZipStream zipStream = new GZipStream(destStream, CompressionMode.Compress))
                        {
                            try
                            {
                                srcStream.CopyTo(zipStream);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
            PrintFiles("ZIPPED FILES", destinationFolder);
        }

        private static void UnPackFiles()
        {
            /*---------------------------------------------------------------------------
             * Make sure that the source directory exists
            ---------------------------------------------------------------------------*/
            var sourceFolder = Directory.GetCurrentDirectory() + "\\Packed_Files";
            if (!DirectoryExists(sourceFolder) || Directory.GetFiles(sourceFolder).Length <= 0)
            {
                throw new Exception("No Files to unzip");
            }

            /*---------------------------------------------------------------------------
             * Make sure the destination directory exists
            ---------------------------------------------------------------------------*/
            var destinationFolder = Directory.GetCurrentDirectory() + "\\UnPacked_Files";
            if (DirectoryExists(destinationFolder))
            {
                DeleteFilesFromDirectory(destinationFolder);// Remove any existing files
            }

            /*---------------------------------------------------------------------------
             * Grab all Zip files
            ---------------------------------------------------------------------------*/
            DirectoryInfo sourceFolderInfo = new DirectoryInfo(sourceFolder);
            FileInfo[] zipFiles = sourceFolderInfo.GetFiles();
            PrintFiles("ZIP FILES TO UNPACK", sourceFolder);

            /*---------------------------------------------------------------------------
             * Unzip em'
            ---------------------------------------------------------------------------*/
            foreach (var zipFile in zipFiles)
            {
                using (FileStream srcStream = zipFile.OpenRead())
                {
                    // Remove the '.gz' from name to arrive at original name
                    var origName = zipFile.Name.Substring(0, zipFile.Name.Length - 3);
                    var destName = $"{destinationFolder}\\{origName}";

                    using (FileStream destStream = File.Create(destName))
                    {
                        using (GZipStream unzipStream = new GZipStream(srcStream, CompressionMode.Decompress))
                        {
                            try
                            {
                                unzipStream.CopyTo(destStream);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
            PrintFiles("UNPACKED FILES", destinationFolder);
        }
    }
}
