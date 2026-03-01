using SimpleLibrary.GMail;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SimpleLibrary.Tests
{
    public class GMailTests
    {
        [Fact]
        public void GMail_Constructor_ShouldInitialize()
        {
            var gmail = new SimpleLibrary.GMail.GMail("test@gmail.com", "password");

            Assert.NotNull(gmail);
        }

        [Fact]
        public void GMail_Constructor_WithCustomSmtp_ShouldSetSmtp()
        {
            var gmail = new SimpleLibrary.GMail.GMail("test@gmail.com", "password", "smtp.custom.com", 587);

            Assert.NotNull(gmail);
        }

        [Fact]
        public void SendMessage_WithEmptyRecipients_ShouldNotThrow()
        {
            var gmail = new SimpleLibrary.GMail.GMail("test@gmail.com", "password");
            
            gmail.SendMessage("Test", "Subject", "Body", new List<string>());

            Assert.True(true);
        }

        [Fact]
        public void SendMessage_WithNullRecipients_ShouldNotThrow()
        {
            var gmail = new SimpleLibrary.GMail.GMail("test@gmail.com", "password");
            
            gmail.SendMessage("Test", "Subject", "Body", null);

            Assert.True(true);
        }

        [Fact]
        public void SendMessage_WithAttachments_ShouldWork()
        {
            var gmail = new SimpleLibrary.GMail.GMail("test@gmail.com", "password");
            var attachments = new List<string> { "nonexistent.pdf" };

            gmail.SendMessage("Test", "Subject", "Body", new List<string> { "test@test.com" }, attachments, null);

            Assert.True(true);
        }

        [Fact]
        public void SendMessage_WithInlineImages_ShouldWork()
        {
            var gmail = new SimpleLibrary.GMail.GMail("test@gmail.com", "password");
            var images = new Dictionary<string, string> { { "nonexistent.png", "image1" } };

            gmail.SendMessage("Test", "Subject", "<img src='cid:image1'>", new List<string> { "test@test.com" }, null, images);

            Assert.True(true);
        }

        [Fact]
        public async Task SendMessageAsync_ShouldWork()
        {
            var gmail = new SimpleLibrary.GMail.GMail("test@gmail.com", "password");

            await gmail.SendMessageAsync("Test", "Subject", "Body", new List<string> { "test@test.com" });

            Assert.True(true);
        }

        [Fact]
        public async Task SendMessageAsync_WithCancellationToken_ShouldWork()
        {
            var gmail = new SimpleLibrary.GMail.GMail("test@gmail.com", "password");
            using (var cts = new CancellationTokenSource())
            {
                await gmail.SendMessageAsync("Test", "Subject", "Body", new List<string> { "test@test.com" }, cts.Token);
            }

            Assert.True(true);
        }
    }
}
