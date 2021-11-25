using System;

namespace CyroTechPortal
{
    public class CustomerModel
    {
        public CustomerModel(string firstName,
            string lastName,
            string phoneNumber,
            int age,
            DateTime birthday)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Age = age;
            Birthday = birthday;
        }

        public CustomerModel()
        {
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public int Age { get; set; }

        public DateTime Birthday { get; set; }
    }
}
