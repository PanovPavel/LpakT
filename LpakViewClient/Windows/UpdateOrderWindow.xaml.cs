using System.Windows;
using LpakBL.Model;
using LpakViewClient.ModelView;

namespace LpakViewClient.Windows
{
    public partial class UpdateOrderWindow : Window
    {
        
        
        public UpdateOrderWindow(Order order)
        {
            InitializeComponent();
            this.DataContext = order;
        }
        
        
        public UpdateOrderWindow(OrderViewModel orderViewModel)
        {
            InitializeComponent();
            this.DataContext = orderViewModel;
        }
    }
}