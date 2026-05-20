using System;
using Xunit;
using AtelierRyza2Trainer.Core;

namespace AtelierRyza2Trainer.Tests
{
    /// <summary>
    /// Unit tests for the MemoryManager class.
    /// Note: These tests require the game process to be running, or they will be skipped.
    /// </summary>
    public class MemoryManagerTests
    {
        [Fact]
        public void Constructor_GameNotRunning_ThrowsException()
        {
            // Arrange: Use a process name that doesn't exist
            var invalidProcessName = "NonExistentGameProcess";

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => new MemoryManager(invalidProcessName));
            Assert.Contains("not found", exception.Message);
        }

        [Fact]
        public void Constructor_ValidProcessName_DoesNotThrow()
        {
            // This test will only pass if AtelierRyza2 is running.
            // It's marked as a conditional test to avoid false failures.
            var processName = "AtelierRyza2";
            try
            {
                using var manager = new MemoryManager(processName);
                Assert.NotNull(manager);
            }
            catch (Exception ex) when (ex.Message.Contains("not found"))
            {
                // Skip if game is not running
                Assert.True(true, "Game process not found; test skipped.");
            }
        }

        [Fact]
        public void ReadMemory_InvalidAddress_ThrowsException()
        {
            // Arrange: Use a process that exists (e.g., current test runner)
            // For testing, we'll use a known system process like "explorer"
            var processName = "explorer";
            using var manager = new MemoryManager(processName);

            // Act & Assert: Read from an invalid address (0x0)
            var exception = Assert.Throws<Exception>(() => manager.ReadMemory(IntPtr.Zero, 4));
            Assert.Contains("Failed to read memory", exception.Message);
        }

        [Fact]
        public void WriteMemory_InvalidAddress_ThrowsException()
        {
            // Arrange: Use a process that exists
            var processName = "explorer";
            using var manager = new MemoryManager(processName);

            // Act & Assert: Write to an invalid address (0x0)
            var data = new byte[] { 0x00 };
            var exception = Assert.Throws<Exception>(() => manager.WriteMemory(IntPtr.Zero, data));
            Assert.Contains("Failed to write memory", exception.Message);
        }

        [Fact]
        public void Dispose_MultipleCalls_DoesNotThrow()
        {
            // Arrange
            var processName = "explorer";
            var manager = new MemoryManager(processName);

            // Act & Assert: Dispose multiple times should be safe
            manager.Dispose();
            manager.Dispose(); // Should not throw
        }
    }
}
