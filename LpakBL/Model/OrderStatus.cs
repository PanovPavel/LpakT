using LpakBL.Model.Exception;

namespace LpakBL.Model
{
    public class OrderStatus
    {
        
        private string _name;
        public string Name
        {
            get=>_name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new IncorrectLongOrNullException(nameof(Name), $"Invalid long or value null value={value}");
                _name = value;
            }
        }

        public OrderStatus(string name)
        {
            Name = name;
        }

        public static OrderStatus NewWorkCreated()
        {
            return new OrderStatus("Новый");
        }
        public static OrderStatus InOfWorkCreated()
        {
            return new OrderStatus("В работе");
        }

        public static OrderStatus FinishedWorkCreated()
        {
            return new OrderStatus("Выполнен");
        }
        
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}