using LpakBL.Controller;
using LpakBL.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfClient.Controls
{
    public partial class CommentControl : UserControl
    {
        public CommentControl()
        {
            InitializeComponent();
        }

        private void OpenNewWindowButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeCustomerWindow changeCustomerWindow = new ChangeCustomerWindow();
            changeCustomerWindow.DataContext = this.DataContext;
            changeCustomerWindow.Show();
        }

        private async void TextBoxCommentLostFocus(object sender, RoutedEventArgs e)
        {
            var customerId = Guid.TryParse(CustomerIdTextBlock.Text, out Guid id)?id:throw new Exception("Ошибка id");
            var custuomerController = new CustomerController();
            Customer customer = await custuomerController.GetAsync(customerId);
            customer.Comment = CommentCustomerTextBox.Text;
            await custuomerController.UpdateAsync(customer);
            WindowManager.UpdateWindow();
        }
    }
}