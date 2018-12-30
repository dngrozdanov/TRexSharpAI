using System.Runtime.InteropServices;

namespace TRexSharp.Constants
{
    public static class Helpers
    {
        [DllImport("User32.dll")]
        public static extern bool SetCursorPos(int X, int Y);
    }
}