using System;
using System.Threading;
using AtelierRyza2Trainer.Core;

namespace AtelierRyza2Trainer
{
    /// <summary>
    /// Entry point for the Atelier Ryza 2: Lost Legends & the Secret Fairy DX Trainer.
    /// Provides a console-based interface for activating various in-game cheats.
    /// </summary>
    internal class Program
    {
        private static MemoryManager _memoryManager;
        private static bool _running = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Atelier Ryza 2: Lost Legends & the Secret Fairy DX Trainer");
            Console.WriteLine("=============================================");
            Console.WriteLine("Ensure the game is running before using this trainer.");
            Console.WriteLine();

            try
            {
                _memoryManager = new MemoryManager("AtelierRyza2");
                Console.WriteLine("Successfully attached to game process.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to attach to game: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Available cheats:");
            Console.WriteLine("1 - Infinite HP");
            Console.WriteLine("2 - Infinite MP");
            Console.WriteLine("3 - Max Gems");
            Console.WriteLine("4 - Max Level");
            Console.WriteLine("0 - Exit");
            Console.WriteLine();

            while (_running)
            {
                Console.Write("Enter option: ");
                var input = Console.ReadLine();
                ProcessInput(input);
            }

            _memoryManager.Dispose();
            Console.WriteLine("Trainer closed.");
        }

        /// <summary>
        /// Processes user input and applies the selected cheat.
        /// </summary>
        /// <param name="input">The user's input string.</param>
        private static void ProcessInput(string input)
        {
            switch (input)
            {
                case "1":
                    ApplyCheat("Infinite HP", () => _memoryManager.WriteMemory(Addresses.HpAddress, new byte[] { 0xFF, 0xFF }));
                    break;
                case "2":
                    ApplyCheat("Infinite MP", () => _memoryManager.WriteMemory(Addresses.MpAddress, new byte[] { 0xFF, 0xFF }));
                    break;
                case "3":
                    ApplyCheat("Max Gems", () => _memoryManager.WriteMemory(Addresses.GemsAddress, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                    break;
                case "4":
                    ApplyCheat("Max Level", () => _memoryManager.WriteMemory(Addresses.LevelAddress, new byte[] { 0x64 }));
                    break;
                case "0":
                    _running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }

        /// <summary>
        /// Applies a cheat action and displays a confirmation message.
        /// </summary>
        /// <param name="cheatName">The name of the cheat being applied.</param>
        /// <param name="action">The action to perform.</param>
        private static void ApplyCheat(string cheatName, Action action)
        {
            try
            {
                action();
                Console.WriteLine($"{cheatName} activated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to activate {cheatName}: {ex.Message}");
            }
        }
    }
}
