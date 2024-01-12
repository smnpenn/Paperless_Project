namespace Paperless.BusinessLogic.Exceptions
{
    public class DocumentLogicException : Exception
    {
        public DocumentLogicException(string message) : base(message) { }
        public DocumentLogicException(string message, Exception innerException) : base(message, innerException) { }
    }
}
