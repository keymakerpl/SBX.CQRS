using Application.Features.CustomerFeatures.Queries;
using Application.Features.CustomerFeatures.Queries.GetCustomers;
using Application.Features.CustomerFeatures.Queries.GetCustomer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Controllers.Base;
using CSharpFunctionalExtensions;
using Domain.Errors;
using Application.Features.CustomerFeatures.Commands.RegisterCustomer;
using WebApi.Customers.Validation;
using Application.Features.CustomerFeatures.Commands;
using Application.Features.CustomerFeatures;

namespace WebApi.Customers.Controllers.V1
{
    /// <summary>
    /// Customers endpoint
    /// </summary>
    [ApiVersion("1.0")]
    [Consumes("application/json")]
    public class CustomerController : BaseController
    {
        /// <summary>
        /// Returns list of customers in database
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of customers</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<CustomerDetailsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomers(int pageNumber, int pageSize) =>
            Ok(await Mediator.Send(new GetCustomersQuery(pageNumber, pageSize)));

        /// <summary>
        /// Get customer by id
        /// </summary>
        /// <param name="customerId">Customer id</param>
        /// <returns>Customer details</returns>
        [HttpGet("{customerId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CustomerDetailsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomer(int customerId) =>
            (await Mediator.Send(new GetCustomerByIdQuery(customerId)))
                           .Match(Some: customer => Ok(customer),
                                  None: () => NotFound(Errors.General.NotFound(nameof(CustomerDetailsDto),
                                                                               customerId)));

        /// <summary>
        /// Registers customer with provided data
        /// </summary>
        /// <param name="request">Customer data to register</param>
        /// <returns>Newly created customer with id</returns>
        [HttpPost()]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerRequest request) => 
            await Result.Success(new RegisterCustomerRequestValidator().Validate(request))
                        .Ensure(validationResult => validationResult.IsValid,
                                validationResult => validationResult.ToString(";"))
                        .Bind(async _ => await RegisterCustomerInternal(request))
                        .Match(onSuccess: createdCustomer => CreatedAtRoute(string.Empty, createdCustomer),
                               onFailure: errors => Error(Errors.Customer.CustomerRegistrationError(errors.Split(";"))));

        private async Task<Result<CustomerDto>> RegisterCustomerInternal(RegisterCustomerRequest request) =>
            await Mediator.Send(
                new RegisterCustomerCommand(request.FirstName,
                                            request.LastName,
                                            request.Email));
    }
}
