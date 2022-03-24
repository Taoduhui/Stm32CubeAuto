using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stm32AutoComplete
{
    internal class Hook
    {
    }
    public class KeyPressSender
    {

        [StructLayout(LayoutKind.Sequential)]
        public class KeyBoardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        //事件输出
        public event KeyEventHandler KeyUpEvent;
        public event KeyEventHandler KeyDownEvent;
        //委托 
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);
        static int hHook = 0;
        public const int WH_KEYBOARD_LL = 13;
        //LowLevel键盘截获，如果是WH_KEYBOARD＝2，并不能对系统键盘截取，Acrobat Reader会在你截取之前获得键盘。 
        static HookProc KeyBoardHookProcedure;

        //设置钩子 
        [DllImport("user32.dll")]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        //抽掉钩子 
        public static extern bool UnhookWindowsHookEx(int idHook);
        [DllImport("user32.dll")]
        //调用下一个钩子 
        public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);
        [DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);

        public void Hook_Start()
        {
            if (hHook == 0)
            {
                KeyBoardHookProcedure = new HookProc(KeyBoardHookProc);
                hHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyBoardHookProcedure,
                        GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
                //如果设置钩子失败. 
                if (hHook == 0)
                {
                    Hook_Clear();
                }
            }
        }

        /// <summary>
        /// 取消钩子事件
        /// </summary>
        public void Hook_Clear()
        {
            bool retKeyboard = true;
            if (hHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hHook);
                hHook = 0;
            }
        }

        public int KeyBoardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                KeyBoardHookStruct kbh = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
                Keys KeyData = (Keys)Enum.Parse(typeof(Keys), kbh.vkCode.ToString());
                // 键盘抬起
                if (KeyUpEvent != null)/* && wParam == WM_KEYUP)*/
                {
                    //Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    //if (keyData == Keys.Up)
                    //{
                    if (kbh.flags == 0)
                    {
                        KeyEventArgs e = new KeyEventArgs(KeyData);//绑定事件
                        KeyDownEvent(this, e);
                    }
                    else
                    {
                        KeyEventArgs e = new KeyEventArgs(KeyData);//绑定事件
                        KeyUpEvent(this, e);
                    }
                    //MessageBox.Show("捕捉到了按键释放");
                    //}
                }
                #region 数据操作
                //switch (k)
                //{
                //    case Keys.F2:
                //        if (kbh.flags == 0)
                //        {
                //             

                //        }
                //        else if (kbh.flags == 128)
                //        {
                //            //放开后做什么事
                //        }
                //        return 1;
                //}
                #endregion
            }
            return CallNextHookEx(hHook, nCode, wParam, lParam);
        }

    }

}
