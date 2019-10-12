using System;
using System.Threading.Tasks;
using System.Web.Http;
using BusinessLogic.Exceptions;
using BusinessLogic.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    /// <summary>
    /// The CustomerController uses the RegisterCustomerUseCase in its Register method.
    /// This method only deals with validation of input data, conversion of the input
    /// to a canonical type that can be consumed by the use case, calling the use case
    /// and finally returning the data or error. We are keeping all web in here. The use
    /// case is unaware of web. That give us flexibility to call the use case from a
    /// non-web context, like a mobile app, a windows forms application or a console app,
    /// </summary>
    public class CustomerController : ApiController
    {
        public IRegisterCustomerUseCase RegisterUseCase { get; private set; }


        public CustomerController(IRegisterCustomerUseCase registerUseCase)
        {
            RegisterUseCase = registerUseCase;
        }


        [HttpPost]
        public async Task<IHttpActionResult> Register(ApiCustomerRegistration customerReg)
        {
            try
            {
                Validate(customerReg);
                var reg = customerReg.ToRegistration();
                var customer = await RegisterUseCase.Register(reg);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        private void Validate(ApiCustomerRegistration customerReg)
        {
            if (customerReg == null)
                throw new MissingCustomerRegistration();
        }

        private IHttpActionResult HandleException(Exception ex)
        {
            if (ex is ClientInputException)
                return BadRequest(ex.Message);
            return InternalServerError();
        }
    }
}
