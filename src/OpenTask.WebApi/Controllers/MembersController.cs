using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenTask.Application.Base;
using OpenTask.Application.Members.GetMember;
using OpenTask.Application.Members.ListMembers;

namespace OpenTask.WebApi.Controllers
{
    [Route("api/Organization/{organizationId}/[controller]")]
    public class MembersController : BaseController
    {
        private readonly ILogger<OrganizationController> _logger;

        public MembersController(IMediator mediator, ILogger<OrganizationController> logger)
            : base(mediator)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ListMembersResponse> ListMembers(long organizationId)
        {
            return await RequestAsync<ListMembersRequest, ListMembersResponse>(new ListMembersRequest
            {
                OrganizationId = organizationId
            });
        }

        [HttpGet("{accountId}")]
        public async Task<MemberResponse> Member(long organizationId, long accountId)
        {
            return await RequestAsync<MemberRequest, MemberResponse>(new MemberRequest
            {
                AccountId = accountId,
                OrganizationId = organizationId
            });
        }
    }
}
