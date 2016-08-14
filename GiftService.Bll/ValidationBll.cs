using GiftService.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{
    public interface IValidationBll
    {
        bool IsEmailValid(string email, bool isTldRequired, IList<ValidationError> errors);
        void EnsureTextLength(string s, int min, int max);
    }

    class ValidationBll : IValidationBll
    {
        public void EnsureTextLength(string s, int min, int max)
        {
            if (String.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("Text is empty");
            }
            if (s.Length < min)
            {
                throw new ArgumentOutOfRangeException("s", "Text is too short. Required at least " + min);
            }
            if (s.Length < max)
            {
                throw new ArgumentOutOfRangeException("s", "Text is too long. Max is " + max);
            }
        }

        public bool IsEmailValid(string email, bool isTldRequired, IList<ValidationError> errors)
        {
            bool isOk = true;

            if (string.IsNullOrEmpty(email))
            {
                errors.Add(new ValidationError(ValidationErrors.Empty, "E-mail can't be empty"));
                return false;
            }

            if (email.Count(x => x.Equals('@')) != 1)
            {
                errors.Add(new ValidationError(ValidationErrors.RuleViolation, "E-mail should contain one and only one @"));
                isOk = false;
            }

            // First or last positions are prohibited
            if (email.IndexOf('@') == 0)
            {
                errors.Add(new ValidationError(ValidationErrors.TooShort, "E-mail could not starts with @"/*, ErrorStartsWithEta*/));
                isOk = false;
            }

            int p = email.LastIndexOf('@');
            if (p == email.Length - 1)
            {
                errors.Add(new ValidationError(ValidationErrors.TooLong, "E-mail could not ends with @"/*, ErrorEndsWithEta*/));
                isOk = false;
            }

            // Check if @ is followed with dot
            if (p < email.Length - 1 && email[p + 1] == '.')
            {
                errors.Add(new ValidationError(ValidationErrors.RuleViolation, "E-mail must contain some letters after @"/*, ErrorNoSymbolAfterEta*/));
                isOk = false;
            }

            if (isTldRequired)
            {
                bool isTldOk = HasValidTld(email);
                if (isTldOk == false)
                {
                    errors.Add(new ValidationError(ValidationErrors.RuleViolation, "TLD is incorrect"/*, ErrorNoTld*/));
                    isOk = false;
                }
            }

            return isOk;
        }

        protected bool HasValidTld(string email)
        {
            bool isOk = true;

            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email", "E-mail should not be empty to validate TLD");
            }

            string[] ar = email.Split(new char[] { '.' });

            if (ar.Length < 2)
            {
                isOk = false;
            }
            else
            {
                string tld = ar[ar.Length - 1];
                if (tld.Length < 1)
                {
                    isOk = false;
                }
            }

            return isOk;
        }
    }
}
