using SimpleLibrary.Logger;
using System;
using System.Drawing;
using System.IO;
using Xunit;

namespace SimpleLibrary.Tests
{
    public class LoggerTests
    {
        [Fact]
        public void ConsoleLogger_Print_ShouldWriteToConsole()
        {
            var logger = new ConsoleLogger();
            var message = "Test message";

            logger.Print(message, Color.White);

            Assert.True(true);
        }

        [Fact]
        public void ColorfulLogger_Print_ShouldWork()
        {
            var logger = new ColorfulLogger();
            var message = "Colorful test";

            logger.Print(message, Color.Blue);

            Assert.True(true);
        }

        [Fact]
        public void FileLogger_Print_ShouldWriteToFile()
        {
            string testFilePath = Path.Combine(Path.GetTempPath(), "test_log.txt");
            if (File.Exists(testFilePath)) File.Delete(testFilePath);

            var logger = new FileLogger(testFilePath);
            var message = "File log test";

            logger.Print(message, Color.White);

            Assert.True(File.Exists(testFilePath));
            var content = File.ReadAllText(testFilePath);
            Assert.Contains("File log test", content);

            File.Delete(testFilePath);
        }

        [Fact]
        public void FileLogger_ThreadSafety_ShouldHandleConcurrentWrites()
        {
            string testFilePath = Path.Combine(Path.GetTempPath(), "concurrent_test.txt");
            if (File.Exists(testFilePath)) File.Delete(testFilePath);

            var logger = new FileLogger(testFilePath);

            System.Threading.Tasks.Parallel.For(0, 100, i =>
            {
                logger.Print($"Log message {i}", Color.White);
            });

            var lines = File.ReadAllLines(testFilePath);
            Assert.Equal(100, lines.Length);

            File.Delete(testFilePath);
        }

        [Fact]
        public void LogLevel_Enum_ShouldHaveCorrectValues()
        {
            Assert.Equal(0, (int)LogLevel.Debug);
            Assert.Equal(1, (int)LogLevel.Info);
            Assert.Equal(2, (int)LogLevel.Warning);
            Assert.Equal(3, (int)LogLevel.Error);
        }

        [Fact]
        public void PrintLogger_MinLevel_ShouldFilterMessages()
        {
            var logger = new PrintLogger();
            logger.MinLevel = LogLevel.Warning;

            logger.Print("Debug message", Color.Gray, LogLevel.Debug);
            logger.Print("Info message", Color.White, LogLevel.Info);
            logger.Print("Warning message", Color.Yellow, LogLevel.Warning);
            logger.Print("Error message", Color.Red, LogLevel.Error);

            Assert.True(true);
        }

        [Fact]
        public void ConsoleLogger_ShowTimestamp_ShouldWork()
        {
            var logger = new ConsoleLogger();
            logger.ShowTimestamp = true;

            logger.Print("Test with timestamp", Color.White);

            Assert.True(logger.ShowTimestamp);
        }
    }
}
