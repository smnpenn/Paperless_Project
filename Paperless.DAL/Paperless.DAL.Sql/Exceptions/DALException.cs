namespace Paperless.DAL.Sql.Exceptions
{
    internal class DALException : Exception
    {
        public DALException(string message) : base(message) { }
        public DALException(string message, Exception innerException) : base(message, innerException) { }
    }
}
