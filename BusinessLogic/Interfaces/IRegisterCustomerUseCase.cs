using System.Threading.Tasks;
using BusinessLogic.Entities;

namespace BusinessLogic.Interfaces
{
    public interface IRegisterCustomerUseCase
    {
        Task<Customer> Register(CustomerRegistration registration);
    }
}
