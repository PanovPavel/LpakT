using System;
using LpakBL.Model.Exception;

namespace LpakBL.Model
{
    public class StatusOrder
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
        public Guid Id { get; set; }

        public StatusOrder(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public StatusOrder(string name):this(Guid.NewGuid(), name)  
        {
            
        }
        
        
        /*
        
        public static StatusOrder NewWorkCreated()
        {
            return new StatusOrder("Новый");
        }
        public static StatusOrder InOfWorkCreated()
        {
            return new StatusOrder("В работе");
        }

        public static StatusOrder FinishedWorkCreated()
        {
            return new StatusOrder("Выполнен");
        }
        */
        public override string ToString()
        {
            return $"{Id} {Name}";
        }
    }
}