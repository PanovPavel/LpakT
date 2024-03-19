using System.Windows;
using LpakViewClient.ModelView;

namespace LpakViewClient.Windows
{
    public partial class AddCustomerWindow : Window
    {
        public AddCustomerWindow(CustomerViewModel customerViewModel)
        {
            InitializeComponent();
            DataContext = customerViewModel;
        }
    }
}