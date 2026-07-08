using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.ViewModels
{
    internal class CustomersManagement
    {
        public void DeleteCustomer(Customer customer)
        {
            try
            {
                using FuminiHotelManagementContext db = new FuminiHotelManagementContext();
                db.Customers.Remove(customer);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void UpdateCustomer(Customer customer)
        {
            try
            {
                using FuminiHotelManagementContext db = new FuminiHotelManagementContext();
                db.Customers.Update(customer);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void InsertCustomer(Customer customer)
        {
            try
            {
                using FuminiHotelManagementContext db = new FuminiHotelManagementContext();
                db.Customers.Add(customer);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public List<Customer> GetCustomer()
        {
            List<Customer> customer = new List<Customer>();
            try
            {
                using FuminiHotelManagementContext db = new FuminiHotelManagementContext();
                customer = db.Customers.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return customer;
        }
        public Customer GetCustomerByEmailAndPassword(string email, string password)
        {
            Customer customer = new Customer();
            try
            {
                using FuminiHotelManagementContext db = new FuminiHotelManagementContext();
                customer = db.Customers.FirstOrDefault(c => c.EmailAddress == email && c.Password == password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return customer;
        }


    }
}
