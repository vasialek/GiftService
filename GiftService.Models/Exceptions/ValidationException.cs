using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models.Exceptions
{
    public enum ValidationErrors
    {
        TooLong = 600,
        TooShort,
        RuleViolation,
        NotSelected,
        NotEqual,
        IncorrectType,
        Empty
    }

    public class ValidationException : Exception
    {
        public ValidationError Error { get; private set; }

        public ValidationException(string msg, ValidationErrors errCode)
            : base(msg)
        {
            Error = new ValidationError(errCode, msg);
        }

        public ValidationException(string msg, ValidationError error)
            : base(msg)
        {
            Error = error;
        }
    }

    public class ValidationListException : Exception
    {
        public IEnumerable<ValidationError> Errors { get; private set; }

        public ValidationListException(string msg)
            : this(msg, null)
        {
        }

        public ValidationListException(string msg, IEnumerable<ValidationError> errors)
            : base(msg)
        {
            Errors = errors;
        }
    }

    public class ValidationError
    {
        public ValidationErrors ErrCode { get; set; }
        public string ErrMessage { get; set; }
        public GsErrorCodes SubErrCode { get; set; }

        /// <summary>
        /// Set form field name if any
        /// </summary>
        public string FieldName { get; set; }

        public ValidationError(ValidationErrors errCode, string errMessage)
        : this(errCode, errMessage, "")
        {
        }

        public ValidationError(ValidationErrors errCode, string errMessage, GsErrorCodes subErrCode)
        : this(errCode, errMessage, subErrCode, "")
        {
        }

        public ValidationError(ValidationErrors errCode, string errMessage, string fieldName)
        : this(errCode, errMessage, GsErrorCodes.None, fieldName)
        {
        }

        public ValidationError(ValidationErrors errCode, string errMessage, GsErrorCodes subErrCode, string fieldName)
        {
            ErrCode = errCode;
            ErrMessage = errMessage;
            SubErrCode = subErrCode;
            FieldName = fieldName;
        }

    }
}
