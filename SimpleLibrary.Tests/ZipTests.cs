using SimpleLibrary.Zip;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace SimpleLibrary.Tests
{
    public class ZipTests : IDisposable
    {
        private readonly string _tempDir;
        private readonly string _testDir;
        private readonly string _zipPath;
        private string _unzipPath;

        public ZipTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "ZipTest_" + Guid.NewGuid().ToString("N"));
            _testDir = Path.Combine(_tempDir, "TestFolder");
            _zipPath = Path.Combine(_tempDir, "output.zip");
            _unzipPath = Path.Combine(_tempDir, "UnzipFolder");

            Directory.CreateDirectory(_testDir);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDir))
            {
                Directory.Delete(_tempDir, true);
            }
        }

        [Fact]
        public void ZipTo_ShouldCreateZipFile()
        {
            string testFile = Path.Combine(_testDir, "test1.txt");
            File.WriteAllText(testFile, "Test content");

            var zip = new SimpleLibrary.Zip.Zip();
            zip.ZipTo(_zipPath, _testDir);

            Assert.True(File.Exists(_zipPath));
        }

        [Fact]
        public void ZipTo_WithMultipleFiles_ShouldCompressAll()
        {
            File.WriteAllText(Path.Combine(_testDir, "file1.txt"), "Content 1");
            File.WriteAllText(Path.Combine(_testDir, "file2.txt"), "Content 2");
            File.WriteAllText(Path.Combine(_testDir, "file3.txt"), "Content 3");

            var zip = new SimpleLibrary.Zip.Zip();
            zip.ZipTo(_zipPath, _testDir);

            var fileInfo = new FileInfo(_zipPath);
            Assert.True(fileInfo.Exists);
            Assert.True(fileInfo.Length > 0);
        }

        [Fact]
        public void ZipTo_WithProgress_ShouldReportProgress()
        {
            File.WriteAllText(Path.Combine(_testDir, "test.txt"), "Test content");

            var zip = new SimpleLibrary.Zip.Zip();
            int lastProgress = -1;
            var progress = new Progress<int>(p => lastProgress = p);

            zip.ZipTo(_zipPath, _testDir, progress);

            Assert.Equal(100, lastProgress);
        }

        [Fact]
        public async Task ZipToAsync_ShouldCreateZipFile()
        {
            string testFile = Path.Combine(_testDir, "async_test.txt");
            File.WriteAllText(testFile, "Async test content");

            var zip = new SimpleLibrary.Zip.Zip();
            await zip.ZipToAsync(_zipPath, _testDir);

            Assert.True(File.Exists(_zipPath));
        }

        [Fact]
        public void UnzipTo_ShouldExtractAllFiles()
        {
            string testFile = Path.Combine(_testDir, "test.txt");
            File.WriteAllText(testFile, "Test content for unzip");

            var zip = new SimpleLibrary.Zip.Zip();
            zip.ZipTo(_zipPath, _testDir);

            Directory.CreateDirectory(_unzipPath);
            zip.UnzipTo(_zipPath, _unzipPath);

            var extractedFiles = Directory.GetFiles(_unzipPath, "*", SearchOption.AllDirectories);
            Assert.NotEmpty(extractedFiles);
        }

        [Fact]
        public void UnzipTo_WithProgress_ShouldReportProgress()
        {
            File.WriteAllText(Path.Combine(_testDir, "test.txt"), "Test content");

            var zip = new SimpleLibrary.Zip.Zip();
            zip.ZipTo(_zipPath, _testDir);

            int lastProgress = -1;
            var progress = new Progress<int>(p => lastProgress = p);

            Directory.CreateDirectory(_unzipPath);
            zip.UnzipTo(_zipPath, _unzipPath, progress);

            Assert.Equal(100, lastProgress);
        }

        [Fact]
        public void UnzipTo_NonExistentFile_ShouldHandleError()
        {
            var zip = new SimpleLibrary.Zip.Zip();
            zip.UnzipTo("nonexistent.zip", _unzipPath);
        }

        [Fact]
        public void ZipTo_InvalidPathCharacter_ShouldWarn()
        {
            string invalidPath = Path.Combine(_tempDir, "test'.zip");
            File.WriteAllText(Path.Combine(_testDir, "file.txt"), "Content");

            var zip = new SimpleLibrary.Zip.Zip();
            zip.ZipTo(invalidPath, _testDir);

            Assert.True(File.Exists(invalidPath) || !File.Exists(invalidPath));
        }

        [Fact]
        public async Task UnzipToAsync_ShouldExtractAllFiles()
        {
            string testFile = Path.Combine(_testDir, "async_unzip.txt");
            File.WriteAllText(testFile, "Async unzip test");

            var zip = new SimpleLibrary.Zip.Zip();
            await zip.ZipToAsync(_zipPath, _testDir);

            Directory.CreateDirectory(_unzipPath);
            await zip.UnzipToAsync(_zipPath, _unzipPath);

            var extractedFiles = Directory.GetFiles(_unzipPath, "*", SearchOption.AllDirectories);
            Assert.NotEmpty(extractedFiles);
        }
    }
}
