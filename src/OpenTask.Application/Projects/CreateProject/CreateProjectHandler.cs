using OpenTask.Application.Base.Commands;
using OpenTask.Domain.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTask.Application.Projects.CreateProject
{
    public class CreateProjectHandler : ICommandHandler<CreateProjectRequest, CreateProjectResponse>
    {
        readonly IProjectRepository projectRepository;

        public CreateProjectHandler(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public Task<CreateProjectResponse> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
        {
            var project = Project.Create(request.OrganizationId, request.ProjectName, request.Describe, request.CreatedBy);

            var id = projectRepository.Save(project);

            project.Id = id;
            return Task.FromResult(new CreateProjectResponse
            {
                Project = project
            });
        }
    }
}
