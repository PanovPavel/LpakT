using System;
using LpakBL.Model.Exception;

namespace LpakBL.Model
{
    public class FieldOfBusiness
    {
        private string _name;
        public Guid Id { get;}

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new IncorrectLongOrNullException(nameof(Name),$"Field of business name cannot be null or empty value={value}");
                _name = value;
            }
        }
        public FieldOfBusiness(string name):this(Guid.NewGuid(),name)
        {
                
        }

        public FieldOfBusiness(Guid id, string name)
        {

            if(id == Guid.Empty) throw new AggregateException($"Field of business id cannot be null or empty value={id}");
            Name = name;
            Id = id;
        }

        public override string ToString()
        {
            return $"{Id}, {Name}";
        }
    }
}