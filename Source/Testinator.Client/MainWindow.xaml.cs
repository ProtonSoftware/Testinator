using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace Testinator.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Disable Task Switching Keys

        private IntPtr mPtrHook;
        private LowLevelKeyboardProc mObjKeyboardProcess;
        private ProcessModule mObjCurrentModule;

        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public int mKey;
            public int mScanCode;
            public int mFlags;
            public int mTime;
            public IntPtr mExtra;
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int id, LowLevelKeyboardProc callback, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hook);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hook, int nCode, IntPtr wp, IntPtr lp);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string name);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern short GetAsyncKeyState(int key);

        private IntPtr CaptureKey(int nCode, IntPtr wp, IntPtr lp)
        {
            if (nCode >= 0)
            {
                var objKeyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lp, typeof(KBDLLHOOKSTRUCT));

                // Info about key values
                // https://www.autoitscript.com/forum/topic/85171-limit-keyboard-input/

                var tabKey = 9;
                var ctrlKey = 17;
                var printScreenKey = 44;
                var delKey = 46;
                var leftWin = 91;
                var rightWin = 92;
                var f4Key = 115;

                // Monitor for task switch keys
                // NOTE: mFlags 0x20 is if Alt is pressed
                if (
                    objKeyInfo.mKey == rightWin || 
                    objKeyInfo.mKey == leftWin || 
                    objKeyInfo.mKey == printScreenKey ||
                    objKeyInfo.mKey == tabKey && ((objKeyInfo.mFlags & 0x20) == 0x20) ||
                    objKeyInfo.mKey == ctrlKey && ((objKeyInfo.mFlags & 0x20) == 0x20) && objKeyInfo.mKey == delKey ||
                    ((objKeyInfo.mFlags & 0x20) == 0x20) && objKeyInfo.mKey == f4Key
                   )
                    return (IntPtr)1;
            }

            return CallNextHookEx(mPtrHook, nCode, wp, lp);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Set DataContext to the WindowViewModel to allow binding in xaml
            DataContext = new WindowViewModel(this);
        }

        #endregion

        #region Internal Helpers

        /// <summary>
        /// Prevents this window from allowing alt-tabs / alt-f4 / window buttons etc.
        /// </summary>
        internal void PreventUserEscapeActions()
        {
            mObjCurrentModule = Process.GetCurrentProcess().MainModule;
            mObjKeyboardProcess = new LowLevelKeyboardProc(CaptureKey);
            mPtrHook = SetWindowsHookEx(13, mObjKeyboardProcess, GetModuleHandle(mObjCurrentModule.ModuleName), 0);
        }

        /// <summary>
        /// Re-allows users actions by returning to the initial window state
        /// </summary>
        internal void AllowUserActions()
        {
            mObjCurrentModule = null;
            mObjKeyboardProcess = null;
            mPtrHook = default(IntPtr);
        }

        #endregion
    }
}

