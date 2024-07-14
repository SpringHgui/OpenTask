using OpenTask.Application.Base.Commands;
using OpenTask.Application.Base.Queries;
using OpenTask.Application.Todos.GetTodoItems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenTask.Application.Projects.CreateProject
{
    public class CreateProjectRequest : ICommand<CreateProjectResponse>
    {
        [JsonIgnore]
        public long OrganizationId { get; set; }

        [JsonIgnore]
        public long CreatedBy { get; set; }

        public string ProjectName { get; set; } = null!;

        public string Describe { get; set; } = null!;
    }
}
