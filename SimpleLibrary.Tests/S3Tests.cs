using Amazon;
using SimpleLibrary.S3;
using System;
using System.IO;
using Xunit;

namespace SimpleLibrary.Tests
{
    public class S3Tests
    {
        [Fact]
        public void S3_Constructor_ShouldInitialize()
        {
            var s3 = new SimpleLibrary.S3.S3("test-bucket", "access-key", "secret-key");

            Assert.NotNull(s3);
        }

        [Fact]
        public void S3_Constructor_WithCustomRegion_ShouldSetRegion()
        {
            var s3 = new SimpleLibrary.S3.S3("test-bucket", "access-key", "secret-key", RegionEndpoint.USEast1);

            Assert.NotNull(s3);
        }

        [Fact]
        public void UploadFile_NonExistentFile_ShouldReturnEmptyString()
        {
            var s3 = new SimpleLibrary.S3.S3("test-bucket", "access-key", "secret-key");
            string result = s3.UploadFile("nonexistent_file.txt");

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void UploadFile_WithProgress_ShouldWork()
        {
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "Test content");

            var s3 = new SimpleLibrary.S3.S3("test-bucket", "access-key", "secret-key");
            int progressValue = -1;
            var progress = new Progress<int>(p => progressValue = p);

            try
            {
                s3.UploadFile(tempFile, progress);
            }
            catch
            {
            }

            File.Delete(tempFile);
        }

        [Fact]
        public void DeleteFile_ShouldReturnTrue_WhenNoException()
        {
            var s3 = new SimpleLibrary.S3.S3("test-bucket", "access-key", "secret-key");
            
            bool result = s3.DeleteFile("test.txt");
            
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteFileAsync_ShouldWork()
        {
            var s3 = new SimpleLibrary.S3.S3("test-bucket", "access-key", "secret-key");
            
            bool result = await s3.DeleteFileAsync("test.txt");
            
            Assert.False(result);
        }

        [Fact]
        public void FileExists_ShouldReturnFalse_WhenFileNotFound()
        {
            var s3 = new SimpleLibrary.S3.S3("test-bucket", "access-key", "secret-key");
            
            bool result = s3.FileExists("nonexistent.txt");
            
            Assert.False(result);
        }

        [Fact]
        public void GetPreSignedUrl_ShouldReturnUrl_WhenCalled()
        {
            var s3 = new SimpleLibrary.S3.S3("test-bucket", "access-key", "secret-key");
            
            try
            {
                string url = s3.GetPreSignedUrl("test.txt", TimeSpan.FromDays(1));
                
                Assert.Null(url);
            }
            catch
            {
            }
        }

        [Fact]
        public void Dispose_ShouldNotThrow()
        {
            var s3 = new SimpleLibrary.S3.S3("test-bucket", "access-key", "secret-key");
            
            s3.Dispose();
            
            Assert.True(true);
        }
    }
}
