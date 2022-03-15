using System;

namespace Application.Domain.Projects
{
    public class ProjectDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int DevelopersLimit { get; set; }
        public int CodeLinesToWrite { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeadLine { get; set; }
    }
}
