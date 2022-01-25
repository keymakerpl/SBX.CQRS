namespace Application.Utils.Connection
{
    public abstract class ConnectionString
    {
        public ConnectionString(string value) => Value = value;

        public string Value { get; }
    }

    public sealed class QueriesConnectionString : ConnectionString
    {
        public QueriesConnectionString(string value) : base(value)
        {
        }
    }

    public sealed class CommandsConnectionString : ConnectionString
    {
        public CommandsConnectionString(string value) : base(value)
        {
        }
    }
}
