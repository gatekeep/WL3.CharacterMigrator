/**
 * Wasteland 3 Character Migrator
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@jmp.cx>
 */

using System;
using System.Windows.Forms;

namespace WL3.CharacterMigrator
{
    /// <summary>
    /// 
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    } // public static class Program
} // namespace WL3.CharacterMigrator
