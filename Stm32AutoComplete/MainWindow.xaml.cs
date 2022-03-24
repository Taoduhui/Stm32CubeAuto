using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Stm32AutoComplete
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.KeyEventHandler myKeyEventHandeler = null;//按键钩子
                                                          //private KeyboardHook k_hook = new KeyboardHook();
        private KeyPressSender k_hook = new KeyPressSender();

        List<Keys> ActiveKey = new List<Keys>() { Keys.Q, Keys.W, Keys.E, Keys.R, Keys.T, Keys.Y, Keys.U, Keys.I, Keys.O, Keys.P, Keys.A, Keys.S, Keys.D, Keys.F, Keys.G, Keys.H, Keys.J, Keys.K, Keys.L, Keys.Z, Keys.X, Keys.C, Keys.V, Keys.B, Keys.N, Keys.M,Keys.OemMinus };

        public MainWindow()
        {
            InitializeComponent();
        }


        private void hook_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (ActiveKey.Contains(e.KeyCode))
            {
                MoniAction.Keyboard.Press(Key.LeftAlt);
                MoniAction.Keyboard.Press(Key.OemQuestion);
                MoniAction.Keyboard.Release(Key.OemQuestion);
                MoniAction.Keyboard.Release(Key.LeftAlt);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        bool IsRuning = false;

        private void Indicator_Click(object sender, RoutedEventArgs e)
        {
            if (!IsRuning)
            {
                myKeyEventHandeler = new System.Windows.Forms.KeyEventHandler(hook_KeyUp);
                k_hook.KeyUpEvent += myKeyEventHandeler;//钩住键按下
                k_hook.Hook_Start();//安装键盘钩子
                IsRuning = true;
                Indicator.Content = "关闭自动补全";
            }
            else
            {
                k_hook.Hook_Clear();//卸载键盘钩子
                IsRuning = false;
                Indicator.Content = "激活自动补全";
            }
        }
    }
}
