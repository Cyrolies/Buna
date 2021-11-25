using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace CyroTechPortal
{
    /// <summary>
    /// Contains sorting helpers for In Memory collections.
    /// </summary>
    public static class CollectionHelper
    {
        public static IOrderedEnumerable<TSource> CustomSort<TSource, TKey>(this IEnumerable<TSource> items, SortingDirection direction, Func<TSource, TKey> keySelector)
        {
            if (direction == SortingDirection.Ascending)
            {
                return items.OrderBy(keySelector);
            }

            return items.OrderByDescending(keySelector);
        }

        public static IOrderedEnumerable<TSource> CustomSort<TSource, TKey>(this IOrderedEnumerable<TSource> items, SortingDirection direction, Func<TSource, TKey> keySelector)
        {
            if (direction == SortingDirection.Ascending)
            {
                return items.ThenBy(keySelector);
            }

            return items.ThenByDescending(keySelector);
        }
    }

    public static class InMemoryCustomersRepository
    {
        private static IList<CustomerModel> GetAllCustomers()
        {
            var customers = new List<CustomerModel>();
            customers.Add(new CustomerModel(firstName: "Justin", lastName: "Michaels", age: 27, phoneNumber: "(123) 555-5555", birthday: Convert.ToDateTime("12/03/1984")));
            customers.Add(new CustomerModel(firstName: "James", lastName: "Halk", age: 21, phoneNumber: "(123) 555-5554", birthday: Convert.ToDateTime("12/01/1990")));
            customers.Add(new CustomerModel(firstName: "Lauren", lastName: "Waddams", age: 22, phoneNumber: "(123) 555-5553", birthday: Convert.ToDateTime("05/09/1990")));
            customers.Add(new CustomerModel(firstName: "Dan", lastName: "Callahan", age: 35, phoneNumber: "(123) 555-5552", birthday: Convert.ToDateTime("12/09/76")));
            customers.Add(new CustomerModel(firstName: "Kevin", lastName: "Kentucky", age: 40, phoneNumber: "(123) 555-5551", birthday: Convert.ToDateTime("12/08/1972")));
            customers.Add(new CustomerModel(firstName: "Mike", lastName: "Peterson", age: 24, phoneNumber: "(123) 555-5550", birthday: Convert.ToDateTime("1/03/1988")));
            customers.Add(new CustomerModel(firstName: "Tom", lastName: "Gun", age: 59, phoneNumber: "(123) 555-5559", birthday: Convert.ToDateTime("1/23/1953")));
            customers.Add(new CustomerModel(firstName: "Erich", lastName: "Milton", age: 54, phoneNumber: "(123) 555-5558", birthday: Convert.ToDateTime("4/03/1958")));
            customers.Add(new CustomerModel(firstName: "Jason", lastName: "Ralph", age: 27, phoneNumber: "(123) 555-5557", birthday: Convert.ToDateTime("11/03/1984")));
            customers.Add(new CustomerModel(firstName: "Jarold", lastName: "Interface", age: 39, phoneNumber: "(123) 555-5556", birthday: Convert.ToDateTime("10/03/1972")));
            customers.Add(new CustomerModel(firstName: "John", lastName: "Thompson21", age: 27, phoneNumber: "(123) 555-5545", birthday: Convert.ToDateTime("12/31/1984")));
            return customers;
        }

        public static IList<CustomerModel> GetCustomers(int startIndex,
            int pageSize,
            ReadOnlyCollection<SortedColumn> sortedColumns,
            out int totalRecordCount,
            out int searchRecordCount, 
            string searchString)
        {
            var customers = GetAllCustomers();

            totalRecordCount = customers.Count;

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                customers = customers.Where(c => c.FirstName.ToLower().Contains(searchString.ToLower())
                    || c.LastName.ToLower().Contains(searchString.ToLower())).ToList();
            }

            searchRecordCount = customers.Count;

            IOrderedEnumerable<CustomerModel> sortedCustomers = null;
            foreach (var sortedColumn in sortedColumns)
            {
                switch (sortedColumn.PropertyName)
                {
                    case "FirstName":
                        sortedCustomers = sortedCustomers == null ? customers.CustomSort(sortedColumn.Direction, cust => cust.FirstName)
                            : sortedCustomers.CustomSort(sortedColumn.Direction, cust => cust.FirstName);
                        break;
                    case "LastName":
                        sortedCustomers = sortedCustomers == null ? customers.CustomSort(sortedColumn.Direction, cust => cust.LastName)
                            : sortedCustomers.CustomSort(sortedColumn.Direction, cust => cust.LastName);
                        break;
                    case "Age":
                        sortedCustomers = sortedCustomers == null ? customers.CustomSort(sortedColumn.Direction, cust => cust.Age)
                            : sortedCustomers.CustomSort(sortedColumn.Direction, cust => cust.Age);
                        break;
                    case "PhoneNumber":
                        sortedCustomers = sortedCustomers == null ? customers.CustomSort(sortedColumn.Direction, cust => cust.PhoneNumber)
                            : sortedCustomers.CustomSort(sortedColumn.Direction, cust => cust.PhoneNumber);
                        break;
                    case "Birthday":
                        sortedCustomers = sortedCustomers == null ? customers.CustomSort(sortedColumn.Direction, cust => cust.Birthday)
                            : sortedCustomers.CustomSort(sortedColumn.Direction, cust => cust.Birthday);
                        break;
                }
            }

            return sortedCustomers.Skip(startIndex).Take(pageSize).ToList();
        }

        public static IList<CustomerModel> GetCustomers(int startIndex,
            int pageSize,
            ReadOnlyCollection<SortedColumn> sortedColumns,
            out int totalRecordCount,
            out int searchRecordCount,
            CustomerSearchModel searchModel)
        {
            var customers = GetAllCustomers();

            totalRecordCount = customers.Count;

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
            {
                customers = customers.Where(c => c.FirstName.ToLower().Contains(searchModel.Name.ToLower())
                    || c.LastName.ToLower().Contains(searchModel.Name.ToLower())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchModel.PhoneNumber))
            {
                customers = customers.Where(c => c.PhoneNumber.Contains(searchModel.PhoneNumber)).ToList();
            }

            if (searchModel.BeginAge.HasValue && searchModel.EndAge.HasValue)
            {
                customers = customers.Where(c => c.Age >= searchModel.BeginAge.Value && c.Age <= searchModel.EndAge.Value).ToList();
            }

            if (searchModel.BeginBirthday.HasValue && searchModel.EndBirthday.HasValue)
            {
                customers = customers.Where(c => c.Birthday >= searchModel.BeginBirthday.Value && c.Birthday <= searchModel.EndBirthday.Value).ToList();
            }

            searchRecordCount = customers.Count;

            IOrderedEnumerable<CustomerModel> sortedCustomers = null;
            foreach (var sortedColumn in sortedColumns)
            {
                switch (sortedColumn.PropertyName)
                {
                    case "FirstName":
                        sortedCustomers = sortedCustomers == null ? customers.CustomSort(sortedColumn.Direction, cust => cust.FirstName)
                            : sortedCustomers.CustomSort(sortedColumn.Direction, cust => cust.FirstName);
                        break;
                    case "LastName":
                        sortedCustomers = sortedCustomers == null ? customers.CustomSort(sortedColumn.Direction, cust => cust.LastName)
                            : sortedCustomers.CustomSort(sortedColumn.Direction, cust => cust.LastName);
                        break;
                    case "Age":
                        sortedCustomers = sortedCustomers == null ? customers.CustomSort(sortedColumn.Direction, cust => cust.Age)
                            : sortedCustomers.CustomSort(sortedColumn.Direction, cust => cust.Age);
                        break;
                    case "PhoneNumber":
                        sortedCustomers = sortedCustomers == null ? customers.CustomSort(sortedColumn.Direction, cust => cust.PhoneNumber)
                            : sortedCustomers.CustomSort(sortedColumn.Direction, cust => cust.PhoneNumber);
                        break;
                    case "Birthday":
                        sortedCustomers = sortedCustomers == null ? customers.CustomSort(sortedColumn.Direction, cust => cust.Birthday)
                            : sortedCustomers.CustomSort(sortedColumn.Direction, cust => cust.Birthday);
                        break;
                }
            }

            return sortedCustomers.Skip(startIndex).Take(pageSize).ToList();
        }
    }
}