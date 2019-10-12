using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;

namespace BusinessLogic.Services
{
    /// <summary>
    /// In memory database implementing ICustomerRepository.
    /// Useful for running WebApi and manually testing how 
    /// the RegisterCustomerUseCase class works with an
    /// actual database. 
    /// </summary>
    public class InMemoryCustomerRepository : ICustomerRepository
    {
        private static List<Customer> Customers = new List<Customer>();


        public async Task<Customer> GetCustomer(string emailAddress)
        {
            await Task.CompletedTask;
            return Customers.SingleOrDefault(x => x.EmailAddress == emailAddress);
        }

        public async Task SaveCustomer(Customer customer)
        {
            await Task.CompletedTask;
            Customers.Add(customer);
        }
    }
}
