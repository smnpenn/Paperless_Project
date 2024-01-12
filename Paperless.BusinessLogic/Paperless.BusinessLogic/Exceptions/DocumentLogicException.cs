namespace Paperless.BusinessLogic.Exceptions
{
    internal class DocumentLogicException : Exception
    {
        public DocumentLogicException(string message) : base(message) { }
        public DocumentLogicException(string message, Exception innerException) : base(message, innerException) { }
    }
}
