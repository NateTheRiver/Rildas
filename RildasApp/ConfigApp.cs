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
        public static bool silentGroupMessages { get { return Global.GetApplicationSettings("silentGroupMessages") == "true"; } }
        public static bool silentPrivateMessages { get { return Global.GetApplicationSettings("silentPrivateMessages") == "true"; } }
        public static bool silentNotifications { get { return Global.GetApplicationSettings("silentNotifications") == "true"; } }
        public static string ConnectionIp { get { return Properties.Settings.Default.IP; } }
        static ConfigApp()
        {
            screenWidth = Screen.PrimaryScreen.Bounds.Width;
            screenHeight = Screen.PrimaryScreen.Bounds.Height;
            silentMode = config.Default.SilentMode;
        }
    }
}
