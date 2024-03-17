using System.Windows;
using LpakBL.Model;
using LpakViewClient.ModelView;

namespace LpakViewClient
{
    public partial class ChangeCustomerWindow : Window
    {
        public ChangeCustomerWindow(Customer customer)
        {
            InitializeComponent();
            DataContext = customer;
        }
        
        public ChangeCustomerWindow(CustomerViewModel customerViewModel)
        {
            InitializeComponent();
            DataContext = customerViewModel;
        }
    }
}