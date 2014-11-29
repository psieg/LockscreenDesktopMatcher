using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace LockscreenDesktopMatcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            BackgroundUpdateWatcher watcher = new BackgroundUpdateWatcher();
            watcher.start();
            Application.Run();
            watcher.stop();
        }
    }
}
