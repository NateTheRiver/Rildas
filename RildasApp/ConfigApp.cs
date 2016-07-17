using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RildasApp
{
    public static class ConfigApp
    {
        public static bool silentMode;
        public static int screenWidth;
        public static int screenHeight;

        static ConfigApp()
        {
            screenWidth = Screen.PrimaryScreen.Bounds.Width;
            screenHeight = Screen.PrimaryScreen.Bounds.Height;
            silentMode = config.Default.SilentMode;
        }
    }
}
