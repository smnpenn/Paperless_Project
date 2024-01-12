namespace Paperless.BusinessLogic.Exceptions
{
    internal class CorrespondentLogicException : Exception
    {
        public CorrespondentLogicException(string message) : base(message) { }
        public CorrespondentLogicException(string message, Exception innerException) : base(message, innerException) { }
    }
}
