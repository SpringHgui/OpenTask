using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenTask.Application.Base;
using OpenTask.Application.Orgs.CreateOrganization;
using OpenTask.Application.Orgs.ListOrg;
using System.ComponentModel.DataAnnotations;

namespace OpenTask.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class OrganizationController : BaseController
    {
        private readonly ILogger<OrganizationController> _logger;

        public OrganizationController(IMediator mediator, ILogger<OrganizationController> logger) : base(mediator)
        {
            _logger = logger;
        }

        [HttpPut]
        public async Task<CreateOrganizationResponse> CreateOrganization(CreateOrganizationRequest request)
        {
            request.UserId = CurrentUser.UserId;
            return await RequestAsync<CreateOrganizationRequest, CreateOrganizationResponse>(request);
        }

        [HttpDelete]
        public IActionResult DeleteOrganization([Range(1, double.MaxValue)] long Id)
        {
            return Content($"deleted");
        }

        [HttpGet]
        public async Task<ListOrgResponse> ListOrganizations()
        {
            return await RequestAsync<ListOrgCommand, ListOrgResponse>(new ListOrgCommand { UserId = CurrentUser.UserId });
        }

    }
}
