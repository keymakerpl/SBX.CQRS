namespace Application.Domain.Projects
{
    public class DeveloperDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double CodeLinesPerHour { get; set; }
        public double HoursPerDay { get; set; }
        public long? ProjectId { get; set; }
    }
}
