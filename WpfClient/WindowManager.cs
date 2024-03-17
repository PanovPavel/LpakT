using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient
{
    public static class WindowManager
    {
        public static Queue<MainWindow> MainWindowInstance { get; set; } = new Queue<MainWindow>();

        public static void UpdateWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Left = WindowManager.MainWindowInstance.Peek().Left;
            mainWindow.Top = WindowManager.MainWindowInstance.Peek().Top;
            mainWindow.Show();
            WindowManager.MainWindowInstance.Dequeue().Close();
            
        }
    }
}
