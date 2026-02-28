using Autofac;
using ICSharpCode.SharpZipLib.Zip;
using SimpleLibrary.Logger;
using System;
using System.Drawing;
using System.IO;

namespace SimpleLibrary.Zip
{
    public class Zip : PrintLogger
    {
        public Zip(ContainerBuilder builder = null)
        {
            InitLogger(builder);
        }

        /// <summary>
        /// 🔍 檢查並提示 S3 的下載網址內不可包含單引號 (')
        /// </summary>
        /// <param name="filePath">☁️ 準備上傳至 S3 的檔案路徑</param>
        private void CheckS3Path(string filePath)
        {
            string path_ = filePath;
            if (path_.Contains("'") == true)
            {
                // S3 的下載網址不能有 ' 分號
                string error_ = $@"S3 的下載網址不能有 ' 分號 path = {path_}";
                Print(error_, Color.OrangeRed);
            }
        }

        /// <summary>
        /// 🗜️ 將指定的目錄整體壓縮為 zip 檔案
        /// </summary>
        /// <param name="outputZipPath">📁 壓縮完成後要輸出的 zip 檔案路徑</param>
        /// <param name="inputDirectory">📂 準備進行壓縮的來源目錄路徑</param>
        public void ZipTo(string outputZipPath, string inputDirectory)
        {
            // 修正 S3 的下載網址不能有 ' 分號
            CheckS3Path(outputZipPath);

            ZipOutputStream zipStream_ = new ZipOutputStream(File.Create(outputZipPath));
            zipStream_.SetLevel(9);

            ZipFolder(inputDirectory, inputDirectory, zipStream_);

            zipStream_.Finish();
            zipStream_.Close();
        }

        /// <summary>
        /// 🗜️ 執行特定目錄的壓縮作業
        /// </summary>
        /// <param name="rootFolder">📂 指定要壓縮的根目錄 (起始目錄)</param>
        /// <param name="currentFolder">📂 目前正在處理的目錄</param>
        /// <param name="zipStream">📝 ZipOutputStream 的參考實例</param>
        private static void ZipFolder(string rootFolder, string currentFolder, ZipOutputStream zipStream)
        {
            string[] SubFolders_ = Directory.GetDirectories(currentFolder);

            foreach (string Folder in SubFolders_)
            {
                ZipFolder(rootFolder, Folder, zipStream);
            }

            string relativePath_ = currentFolder.Substring(rootFolder.Length) + "/";

            if (relativePath_.Length > 1)
            {
                ZipEntry dirEntry_;

                dirEntry_ = new ZipEntry(relativePath_)
                {
                    DateTime = DateTime.Now
                };
            }

            foreach (string file in Directory.GetFiles(currentFolder))
            {
                AddFileToZip(zipStream, relativePath_, file);
            }
        }

        /// <summary>
        /// 📄 將單一檔案加入至指定的 zip 壓縮檔內
        /// </summary>
        /// <param name="zipStream">📝 ZipOutputStream 的參考實例</param>
        /// <param name="relativePath">🛣️ 檔案在壓縮包內的相對路徑</param>
        /// <param name="file">📄 準備加入的新檔案</param>
        private static void AddFileToZip(ZipOutputStream zipStream, string relativePath, string file)
        {
            byte[] buffer_ = new byte[4096];
            string fileRelativePath_ = (relativePath.Length > 1 ? relativePath : string.Empty) + Path.GetFileName(file);
            ZipEntry entry_ = new ZipEntry(fileRelativePath_)
            {
                DateTime = DateTime.Now
            };
            zipStream.PutNextEntry(entry_);

            using (FileStream fs = File.OpenRead(file))
            {
                int sourceBytes_;

                do
                {
                    sourceBytes_ = fs.Read(buffer_, 0, buffer_.Length);
                    zipStream.Write(buffer_, 0, sourceBytes_);
                } while (sourceBytes_ > 0);
            }
        }
    }
}
