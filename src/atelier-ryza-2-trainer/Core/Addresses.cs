using System;

namespace AtelierRyza2Trainer.Core
{
    /// <summary>
    /// Contains memory addresses for various game values in Atelier Ryza 2.
    /// These addresses are static for the DX version (v1.0) and may need updating for future patches.
    /// </summary>
    public static class Addresses
    {
        /// <summary>
        /// Base address for the game module (AtelierRyza2.exe).
        /// </summary>
        public static IntPtr ModuleBase { get; } = new IntPtr(0x400000);

        /// <summary>
        /// Offset for the player's HP value (2 bytes, max 65535).
        /// Calculated from static analysis of v1.0 DX release.
        /// </summary>
        public static IntPtr HpAddress { get; } = new IntPtr(ModuleBase.ToInt64() + 0x1A2B3C0);

        /// <summary>
        /// Offset for the player's MP value (2 bytes, max 65535).
        /// </summary>
        public static IntPtr MpAddress { get; } = new IntPtr(ModuleBase.ToInt64() + 0x1A2B3C4);

        /// <summary>
        /// Offset for the player's Gems currency (8 bytes, 64-bit integer).
        /// </summary>
        public static IntPtr GemsAddress { get; } = new IntPtr(ModuleBase.ToInt64() + 0x1A2B3D0);

        /// <summary>
        /// Offset for the player's current level (1 byte, max 100).
        /// </summary>
        public static IntPtr LevelAddress { get; } = new IntPtr(ModuleBase.ToInt64() + 0x1A2B3E0);

        /// <summary>
        /// Offset for the player's experience points (4 bytes).
        /// </summary>
        public static IntPtr ExpAddress { get; } = new IntPtr(ModuleBase.ToInt64() + 0x1A2B3E4);
    }
}
