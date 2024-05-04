namespace Argo.CA.Api.Controllers;

using Application.Companies.Commands.CreateCompany;
using Application.Companies.Commands.UpdateCompany;
using Application.Companies.Queries.GetCompanyDetails;
using Application.Companies.Queries.GetCompanyList;
using AutoMapper;
using Contracts.Companies;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using DotSwashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("companies")]
public class CompaniesController(ISender mediator, IMapper mapper) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(OperationId = "CreateCompany", Summary = "Create company")]
    [SwaggerResponse(201, "The company was created", typeof(CreateCompanyResponse))]
    [SwaggerResponse(400, "The company data is invalid", typeof(ValidationProblemDetails))]
    [SwaggerResponse(401)]

    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCompanyCommand(
            request.Name,
            request.Description,
            request.Street,
            request.City,
            request.PostCode,
            request.CountryCode,
            request.Email,
            request.PhoneNumber);

        var companyId = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            actionName: nameof(GetCompanyDetails),
            routeValues: new { CompanyId = companyId },
            value: new CreateCompanyResponse(companyId));
    }

    [HttpPut]
    [Route("{companyId:guid}")]
    [SwaggerOperation(OperationId = "UpdateCompany", Summary = "Update company")]
    [SwaggerResponse(204)]
    [SwaggerResponse(400, "The company data is invalid", typeof(ValidationProblemDetails))]
    [SwaggerResponse(404, "The company was not found", typeof(ProblemDetails))]
    [SwaggerResponse(401)]
    public async Task<IActionResult> UpdateCompany(Guid companyId, [FromBody] UpdateCompanyRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCompanyCommand(
            companyId,
            request.Name,
            request.Description,
            request.Street,
            request.City,
            request.PostCode,
            request.CountryCode,
            request.Email,
            request.PhoneNumber);

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpGet]
    [SwaggerOperation(OperationId = "GetCompanyList", Summary = "Get company list")]
    [SwaggerResponse(200, "Success", typeof(GetCompanyListResponse))]
    [SwaggerResponse(401)]
    public async Task<IActionResult> GetCompanyList([FromQuery] GetCompanyListRequest request, CancellationToken cancellationToken)
    {
        var command = new GetCompanyListQuery(
            request.PageNumber,
            request.PageSize);

        var result = await mediator.Send(command, cancellationToken);

        return Ok(new GetCompanyListResponse(
            mapper.Map<List<CompanyListItem>>(result.Items),
            result.PageNumber,
            result.TotalPages,
            result.TotalCount));
    }

    [HttpGet]
    [Route("{companyId:guid}")]
    [SwaggerOperation(OperationId = "GetCompanyDetails", Summary = "Get company details")]
    [SwaggerResponse(200, "Success", typeof(GetCompanyDetailsResponse))]
    [SwaggerResponse(404, "The company was not found", typeof(ProblemDetails))]
    [SwaggerResponse(401)]
    public async Task<IActionResult> GetCompanyDetails(Guid companyId, CancellationToken cancellationToken)
    {
        var command = new GetCompanyDetailsQuery(companyId);

        var result = await mediator.Send(command, cancellationToken);

        return Ok(mapper.Map<GetCompanyDetailsResponse>(result));
    }
}