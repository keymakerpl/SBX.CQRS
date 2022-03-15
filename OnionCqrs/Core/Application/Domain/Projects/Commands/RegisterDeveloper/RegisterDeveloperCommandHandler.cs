using Application.Common;
using CSharpFunctionalExtensions;
using Domain.Projects;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Domain.Projects.Commands.RegisterDeveloper
{
    public class RegisterDeveloperCommandHandler : ICommandHandler<RegisterDeveloperCommand, Result<DeveloperDto>>
    {
        private readonly IDeveloperRepository developerRepository;

        public RegisterDeveloperCommandHandler(IDeveloperRepository developerRepository)
        {
            this.developerRepository = developerRepository;
        }

        public async Task<Result<DeveloperDto>> Handle(RegisterDeveloperCommand request, CancellationToken cancellationToken) =>
            await Developer.CreateDeveloper(request.FirstName,
                                            request.LastName,
                                            request.CodeLinesPerHour,
                                            WorkTime.FromHours(request.WorkTime))
                     .Bind(async entity => await developerRepository.SaveAsync(entity, cancellationToken)
                     .Map(() => new DeveloperDto
                     {
                         Id = entity.Id,
                         FirstName = entity.FirstName,
                         LastName = entity.LastName,
                         CodeLinesPerHour = entity.CodeLinesPerHour,
                         HoursPerDay = entity.WorkTime.HoursPerDay,
                         ProjectId = entity.Project?.Id
                     }));
    }
}
