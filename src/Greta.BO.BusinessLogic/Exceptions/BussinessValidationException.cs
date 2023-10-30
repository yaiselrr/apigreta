using System;
using System.Collections.Generic;

namespace Greta.BO.BusinessLogic.Exceptions
{
    public class BussinessValidationException : Exception
    {
        public BussinessValidationException(IEnumerable<string> errors) : base("Validation Errors")
        {
            Errors = errors;
        }
        
        public BussinessValidationException(string error) : base("Validation Errors")
        {
            Errors = new List<string>(){error};
        }

        public IEnumerable<string> Errors { get; set; }
    }
}