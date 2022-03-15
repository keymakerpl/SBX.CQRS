using System;

namespace Application.Domain.Projects.Commands.RegisterProject
{
    public class RegisterProjectRequest
    {
        public string Name { get; set; }
        public int DevelopersLimit { get; set; }
        public int CodeLinesToWrite { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeadLine { get; set; }
    }
}
