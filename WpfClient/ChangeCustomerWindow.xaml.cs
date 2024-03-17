using LpakBL.Controller;
using LpakBL.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfClient
{
    public partial class ChangeCustomerWindow : Window
    {
        private CustomerController _customerController;
        private System.Guid _customerId;
        
        public ChangeCustomerWindow()
        {
            InitializeComponent();
            _customerController = new CustomerController();
            Closed += (object sender, EventArgs e) =>
            {
                if(sender is ChangeCustomerWindow)
                {
                    WindowManager.UpdateWindow();
                }
            };
        }

        private async void NameLostFocus(object sender, RoutedEventArgs e)
        {
            _customerId = Guid.TryParse(CustomerIdTextBlock.Text, out Guid result) ? result : throw new Exception("Exception idCustomer");
            Customer customer = await _customerController.GetAsync(_customerId);
                await Console.Out.WriteLineAsync(customer.ToString());
                customer.Name = CustomerNameTextBox.Text;
                await _customerController.UpdateAsync(customer);
        }

        private async void TaxNumberLostFocus(object sender, RoutedEventArgs e)
        {
            _customerId = Guid.TryParse(CustomerIdTextBlock.Text, out Guid result) ? result : throw new Exception("Exception idCustomer");
            Customer customer = await _customerController.GetAsync(_customerId);
            customer.TaxNumber = TaxNumberTextBox.Text;
            await _customerController.UpdateAsync(customer);
        }
        private async void FieldOfBusiness(object sender, RoutedEventArgs e)
        {
            _customerId = Guid.TryParse(CustomerIdTextBlock.Text, out Guid result) ? result : throw new Exception("Exception idCustomer");
            Customer customer = await _customerController.GetAsync(_customerId);
            customer.FieldOfBusiness.Name = FieldOfBusinessTextBox.Text;
            await _customerController.UpdateAsync(customer);
        }


    }
}