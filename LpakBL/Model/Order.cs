using System;
using LpakBL.Controller.Exception;
using LpakBL.Model.Exception;

namespace LpakBL.Model
{
    public class Order
    {
        public Order(Guid id, StatusOrder status, Guid customerId, DateTime dateTime, string nameOfWork,
            string descriptionOfWork = "")
        {
            if (id == Guid.Empty) throw new ArgumentException("Order id cannot be empty", nameof(id));
            Id = id;
            Status = status;
            CustomerId = customerId;
            DateTimeCreatedOrder = dateTime;
            NameOfWork = nameOfWork;
            DescriptionOfWork = descriptionOfWork;
        }

        public Order(StatusOrder status, Guid customerId, DateTime dateTime, string nameOfWork,
            string descriptionOfWork = "")
            : this(Guid.NewGuid(), status, customerId, dateTime, nameOfWork, descriptionOfWork)
        {
        }

        public Order(StatusOrder status, Guid customerId, string nameOfWork, string descriptionOfWork = "")
            : this(Guid.NewGuid(), status, customerId, DateTime.Now, nameOfWork, descriptionOfWork)
        {
        }
        private string _nameOfWork, _descriptionOfWork;
        private DateTime _dateTimeCreatedOrder;
        private Guid _customerId;
        private StatusOrder _status;
        public Guid Id { get; }

        public DateTime DateTimeCreatedOrder
        {
            get => _dateTimeCreatedOrder;
            set
            {
                if (value == DateTime.MinValue)
                {
                    throw new InvalidDateException("Date time created order can not be null");
                }

                if (value > DateTime.Today.AddDays(1) || value < DateTime.Parse("01.01.2000"))
                {
                    throw new InvalidDateException($"Date time cannot more than today or less than 01.01.2000 value={value}");
                }

                _dateTimeCreatedOrder = value;
            }
        }

        public string NameOfWork
        {
            get => _nameOfWork;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new IncorrectLongOrNullException(nameof(NameOfWork),
                        $"Invalid long or value null value={value}");
                _nameOfWork = value;
            }
        }

        public string DescriptionOfWork
        {
            get => _descriptionOfWork;
            set => _descriptionOfWork =
                value ?? throw new ArgumentNullException("Argument cannot be null", nameof(DescriptionOfWork));
        }

        public Guid CustomerId
        {
            get => _customerId;
            set
            {
                if (value == Guid.Empty)
                {
                    throw new ArgumentException("Customer id can not be empty", nameof(CustomerId));
                }

                _customerId = value;
            }
        }

        public StatusOrder Status
        {
            get => _status;
            set => _status = value ?? throw new ArgumentNullException(nameof(Status), "Status can not be null");
        }


        
        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(DateTimeCreatedOrder)}: {DateTimeCreatedOrder}, " +
                   $"{nameof(NameOfWork)}: {NameOfWork}, {nameof(DescriptionOfWork)}: {DescriptionOfWork}";
        }
    }
}