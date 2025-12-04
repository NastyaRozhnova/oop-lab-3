using System;

namespace DeliveryApp
{
    public class Customer
    {
        public int Id { get; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string DefaultAddress { get; set; }
        public Customer(int id, string name, string phone, string defaultAddress)
        {
            Id = id;

            if (name == null)
                throw new ArgumentNullException(nameof(name));
            Name = name; 

            if (phone == null)
                throw new ArgumentNullException(nameof(phone));
            Phone = phone; 

            if (defaultAddress == null)
                throw new ArgumentNullException(nameof(defaultAddress));
            DefaultAddress = defaultAddress; 
        }

    }
}