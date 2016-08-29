using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System;
using System.IO;
using static Common.Common;

namespace TarFiles
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
            string archiveFileName = destinationFolder + "\\TarArchive.tar.gz";
            using (Stream targetStream = new GZipOutputStream(File.Create(archiveFileName))) // Could use 'BZip2OutputStream' or 'DeflaterOutputStream'
            {
                using (TarArchive tarArchive = TarArchive.CreateOutputTarArchive(targetStream, TarBuffer.DefaultBlockFactor))
                {
                    Array.ForEach(files, delegate (FileInfo file)
                    {
                        TarEntry entry = TarEntry.CreateEntryFromFile(file.FullName);
                        tarArchive.WriteEntry(entry, true);
                    });
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
                using (Stream srcStream = new GZipInputStream(zipFile.OpenRead()))
                {
                    using (TarArchive tarArchive = TarArchive.CreateInputTarArchive(srcStream, TarBuffer.DefaultBlockFactor))
                    {
                        tarArchive.ExtractContents(destinationFolder);
                    }
                }
            }
            PrintFiles("UNPACKED FILES", destinationFolder);

        }
    }
}
