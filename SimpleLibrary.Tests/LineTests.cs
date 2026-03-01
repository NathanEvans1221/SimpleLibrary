using SimpleLibrary.Line;
using System.Collections.Generic;
using Xunit;

namespace SimpleLibrary.Tests
{
    public class LineTests
    {
        [Fact]
        public void Line_Constructor_ShouldInitialize()
        {
            var line = new SimpleLibrary.Line.Line("https://api.line.me/v2/bot/message/push", "user-id", "api-key");

            Assert.NotNull(line);
        }

        [Fact]
        public void Notify_ShouldNotThrow()
        {
            var line = new SimpleLibrary.Line.Line("https://api.line.me/v2/bot/message/push", "user-id", "invalid-key");

            line.Notify("Test message");

            Assert.True(true);
        }

        [Fact]
        public void NotifyImage_ShouldWork()
        {
            var line = new SimpleLibrary.Line.Line("https://api.line.me/v2/bot/message/push", "user-id", "invalid-key");

            line.NotifyImage("Test message", "https://example.com/image.jpg");

            Assert.True(true);
        }

        [Fact]
        public void NotifySticker_ShouldWork()
        {
            var line = new SimpleLibrary.Line.Line("https://api.line.me/v2/bot/message/push", "user-id", "invalid-key");

            line.NotifySticker("Test message", "11537", "52002734");

            Assert.True(true);
        }

        [Fact]
        public void NotifyWithActions_ShouldWork()
        {
            var line = new SimpleLibrary.Line.Line("https://api.line.me/v2/bot/message/push", "user-id", "invalid-key");
            var actions = new List<LineAction>
            {
                new LineAction { Type = "message", Label = "Option A", Text = "A" },
                new LineAction { Type = "message", Label = "Option B", Text = "B" }
            };

            line.NotifyWithActions("Choose an option", actions);

            Assert.True(true);
        }

        [Fact]
        public async Task NotifyAsync_ShouldWork()
        {
            var line = new SimpleLibrary.Line.Line("https://api.line.me/v2/bot/message/push", "user-id", "invalid-key");

            await line.NotifyAsync("Async message");

            Assert.True(true);
        }

        [Fact]
        public async Task NotifyImageAsync_ShouldWork()
        {
            var line = new SimpleLibrary.Line.Line("https://api.line.me/v2/bot/message/push", "user-id", "invalid-key");

            await line.NotifyImageAsync("Async image", "https://example.com/image.jpg");

            Assert.True(true);
        }

        [Fact]
        public async Task NotifyStickerAsync_ShouldWork()
        {
            var line = new SimpleLibrary.Line.Line("https://api.line.me/v2/bot/message/push", "user-id", "invalid-key");

            await line.NotifyStickerAsync("Async sticker", "11537", "52002734");

            Assert.True(true);
        }

        [Fact]
        public void LineAction_DefaultValues_ShouldBeSet()
        {
            var action = new LineAction();

            Assert.Equal("message", action.Type);
            Assert.Null(action.Label);
            Assert.Null(action.Text);
            Assert.Null(action.Uri);
        }

        [Fact]
        public void LineAction_CustomValues_ShouldBeSet()
        {
            var action = new LineAction
            {
                Type = "uri",
                Label = "Open URL",
                Text = null,
                Uri = "https://example.com"
            };

            Assert.Equal("uri", action.Type);
            Assert.Equal("Open URL", action.Label);
            Assert.Equal("https://example.com", action.Uri);
        }

        [Fact]
        public void Dispose_ShouldNotThrow()
        {
            var line = new SimpleLibrary.Line.Line("https://api.line.me/v2/bot/message/push", "user-id", "api-key");

            line.Dispose();

            Assert.True(true);
        }
    }
}
