using MediatR;
using OpenTask.Application.Base.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTask.Application.Orgs.CreateOrganization
{
    public class CreateOrganizationRequest : ICommand<CreateOrganizationResponse>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public long? UserId { get; set; }
    }
}
