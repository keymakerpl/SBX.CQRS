namespace Application.Domain.Projects.Commands.RegisterDeveloper
{
    public class RegisterDeveloperRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double CodeLinesPerHour { get; set; }
        public double WorkTime { get; set; }
    }
}
