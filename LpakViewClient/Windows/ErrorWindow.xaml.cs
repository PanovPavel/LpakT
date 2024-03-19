using System.Windows;

namespace LpakViewClient
{
    public partial class ErrorWindow
    {
        public ErrorWindow()
        {
            InitializeComponent();
        }
        public ErrorWindow(string message)
        {
            InitializeComponent();
            MessageTextBlock.Text = message;
        }
    }
}