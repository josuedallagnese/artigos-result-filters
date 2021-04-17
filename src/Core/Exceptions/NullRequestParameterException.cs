using System;

namespace ResultFilters.Core.Exceptions
{
    public class NullRequestParameterException : Exception
    {
        public const string NullRequestApiResponse = "{\"hasErrors\":true,\"errors\":[\"Parâmetro nulo informado\"]}";
        public const string ExceptionApiResponse = "{\"hasErrors\":true,\"errors\":[\"Ops! Algo deu errado.\"]}";

        public NullRequestParameterException()
            : base("Parâmetro nulo informado")
        {
        }
    }
}
