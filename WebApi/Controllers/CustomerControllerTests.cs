using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Xunit;
using FluentAssertions;
using BusinessLogic.Entities;
using WebApi.Fakes;
using WebApi.Models;

namespace WebApi.Controllers
{
    /// <summary>
    /// Unit tests for the CustomerController's Register method.
    /// </summary>
    public class CustomerControllerTests
    {
        [Fact]
        public async Task Given_No_CustomerRegistration_When_Call_Register_Then_Return_400_BadRequest()
        {
            var controller = SetupController();
            var result = await controller.Register(null);
            VerifyBadRequestResult(result, "Missing customer registration data.");
        }

        [Theory]
        [MemberData(nameof(GetApiRegistrationsAndRegistrations))]
        public async Task When_Call_Register_Then_Delegate_To_UseCase(ApiCustomerRegistration apiReg,
            CustomerRegistration reg)
        {
            var controller = SetupController();
            await controller.Register(apiReg);
            VerifyCallUseCase(controller, reg);
        }

        [Theory]
        [MemberData(nameof(GetApiRegistrationsAndCustomers))]
        // TODO: Should really return 201 - Created.
        public async Task Given_UseCase_Returns_Customer_When_Call_Register_Then_Return_200_OK(ApiCustomerRegistration apiReg,
            Customer customer)
        {
            var controller = SetupController(customer);
            var result = await controller.Register(apiReg);
            VerifyOkResult(result, customer);
        }

        [Theory]
        [InlineData("Client Input Error")]
        [InlineData("a different error message")]
        public async Task Given_UseCase_Throws_ClientInputException_When_Call_Register_Then_Return_400_BadRequest(string errorMsg)
        {
            // Note: Using DummyClientInputException here rather than, say, MissingFirstName
            // to not give the impression that this test applies only to MissingFirstName; it
            // applies to all ClientInputExceptions.
            var controller = SetupController(new DummyClientInputException(errorMsg));
            var result = await controller.Register(ApiRegoAdamAnt);
            VerifyBadRequestResult(result, errorMsg);
        }

        [Fact]
        public async Task Given_UseCase_Throws_NonSpecific_Exception_When_Call_Register_Then_Return_500_InternalServerError()
        {
            var controller = SetupController(new Exception("unhandled exception"));
            var result = await controller.Register(ApiRegoAdamAnt);
            VerifyInternalServerErrorResult(result);
        }


        private static readonly ApiCustomerRegistration ApiRegoAdamAnt = new ApiCustomerRegistration("Adam", "Ant", "adam@ant.co.uk");
        private static readonly ApiCustomerRegistration ApiRegoBobSmith = new ApiCustomerRegistration("Bob", "Smith", "bob@smith.com");

        private static readonly CustomerRegistration RegoAdamAnt = new CustomerRegistration("Adam", "Ant", "adam@ant.co.uk");
        private static readonly CustomerRegistration RegoBobSmith = new CustomerRegistration("Bob", "Smith", "bob@smith.com");

        private static readonly Customer CustomerAdamAnt = new Customer(Guid.NewGuid(), "Adam", "Ant", "adam@ant.co.uk");
        private static readonly Customer CustomerBobSmith = new Customer(Guid.NewGuid(), "Bob", "Smith", "bob@smith.com");

        public static IEnumerable<object[]> GetApiRegistrationsAndRegistrations()
        {
            yield return new object[] { ApiRegoAdamAnt, RegoAdamAnt };
            yield return new object[] { ApiRegoBobSmith, RegoBobSmith };
        }

        public static IEnumerable<object[]> GetApiRegistrationsAndCustomers()
        {
            yield return new object[] { ApiRegoAdamAnt, CustomerAdamAnt };
            yield return new object[] { ApiRegoBobSmith, CustomerBobSmith };
        }


        private CustomerController SetupController(Customer customer = null)
        {
            var useCase = new MockRegisterCustomerUseCase(customer);
            return new CustomerController(useCase);
        }

        private CustomerController SetupController(Exception exception)
        {
            var useCase = new MockRegisterCustomerUseCase(exception);
            return new CustomerController(useCase);
        }


        private void VerifyCallUseCase(CustomerController controller, CustomerRegistration rego)
        {
            var useCase = (MockRegisterCustomerUseCase)controller.RegisterUseCase;
            useCase.WasRegisterCalled.Should().BeTrue();
            useCase.PassedInRegistration.Should().BeEquivalentTo(rego);
        }

        private void VerifyOkResult<T>(IHttpActionResult result, T t)
        {
            var okResult = result as OkNegotiatedContentResult<T>;
            okResult.Content.Should().BeEquivalentTo(t);
        }

        private void VerifyBadRequestResult(IHttpActionResult result, string message)
        {
            var badRequestResult = result as BadRequestErrorMessageResult;
            badRequestResult.Message.Should().BeEquivalentTo(message);
        }

        private void VerifyInternalServerErrorResult(IHttpActionResult result)
        {
            var internalServerErrorResult = result as InternalServerErrorResult;
            internalServerErrorResult.Should().NotBeNull();
        }
    }
}