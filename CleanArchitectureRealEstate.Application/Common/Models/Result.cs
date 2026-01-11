using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Application.Common.Models
{
    public class Result
    {
        public bool Succeeded { get; }
        public string? Error { get; }

        protected Result(bool succeeded, string? error)
        {
            Succeeded = succeeded;
            Error = error;
        }

        public static Result Success()
            => new(true, null);

        public static Result Failure(string error)
            => new(false, error);
    }
}
