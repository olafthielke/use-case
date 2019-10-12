using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Exceptions;
using BusinessLogic.Interfaces;

namespace BusinessLogic.UseCases
{
    /// <summary>
    /// RegisterCustomerUseCase class. Demonstrates how simple a use case
    /// class is and how it delegates responsibility to other services, 
    /// here the ICustomerRepository.
    /// The public Register method is very simple at the highest level. 
    /// </summary>
    public class RegisterCustomerUseCase : IRegisterCustomerUseCase
    {
        public ICustomerRepository Repository { get; private set; }


        public RegisterCustomerUseCase(ICustomerRepository repository)
        {
            Repository = repository;
        }


        public async Task<Customer> Register(CustomerRegistration registration)
        {
            await Validate(registration);
            var customer = registration.ToCustomer();
            await Repository.SaveCustomer(customer);
            return customer;
        }


        private async Task Validate(CustomerRegistration registration)
        {
            if (registration == null)
                throw new MissingCustomerRegistration();
            registration.Validate();
            var existCust = await Repository.GetCustomer(registration.EmailAddress);
            if (existCust != null)
                throw new DuplicateCustomerEmailAddress(registration.EmailAddress);
        }
    }
}
