using System;
using System.ComponentModel;
using System.Windows;
using LpakBL.Controller;
using LpakBL.Model;
using LpakViewClient.ModelView;

namespace LpakViewClient
{
    public partial class ChangeCustomerWindow
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
            Closing += NoSaveUpdateCustomer_OnCloseWindow;
            
        }

        private async void UpdateCustomer_OnClick(object sender, RoutedEventArgs e)
        {
            Guid customerId = Guid.Parse(CustomerId.Text);
            string customerName = NameCustomer.Text;
            string taxNumber = TaxNumber.Text;
            FieldOfBusiness fildOfBusinessName = new FieldOfBusiness(FieldOfBusinessName.Text);
            string comment = Comment.Text;

            try
            {


                CustomerController customerController = new CustomerController();
                Customer oldCustomer = await customerController.GetAsync(Guid.Parse(CustomerId.Text));
                Customer newModifiedCustomer = new Customer(customerId, customerName, taxNumber, comment,
                    fildOfBusinessName, oldCustomer.Orders);
                await customerController.UpdateAsync(newModifiedCustomer);
                this.Close();
            }
            catch (Exception ex) when (HandlerException.IsHandledException(ex))
            {
                new ErrorWindow(ex.Message).Show();
            }
        }
        private async void NoSaveUpdateCustomer_OnCloseWindow(object sender, CancelEventArgs e)
        {
            Customer oldCustomer = await new CustomerController().GetAsync(Guid.Parse(CustomerId.Text));
            NameCustomer.Text = oldCustomer.Name;
            TaxNumber.Text = oldCustomer.TaxNumber;
            Comment.Text = oldCustomer.Comment;
            FieldOfBusinessName.Text = oldCustomer.FieldOfBusiness.Name;
        }
        
    }
}