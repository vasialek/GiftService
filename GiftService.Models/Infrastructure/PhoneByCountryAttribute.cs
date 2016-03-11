using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GiftService.Models.Infrastructure
{
    public class PhoneByCountryAttribute : ValidationAttribute
    {
        private string _countryCode = "lt";

        public PhoneByCountryAttribute(string countryCode)
            : base("Incorrect phone number: {0}")
        {
            _countryCode = countryCode;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool isOk = true;

            if (value == null)
            {
                return ValidationResult.Success;
                //return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            string phone = value.ToString();
            if (phone.Length < 1)
            {
                return ValidationResult.Success;//(FormatErrorMessage(validationContext.DisplayName));
            }

            int skipSymbols = 0;
            if (IsValidStart(phone, out skipSymbols) == false)
            {
                isOk = false;
            }
            else
            {
                phone = phone.Substring(skipSymbols);
                isOk = IsValidPhone(phone);
            }

            if (isOk == false)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }

        private bool IsValidPhone(string phone)
        {
            bool isOk = true;

            return isOk;
        }

        private bool IsValidStart(string phone, out int skipSymbols)
        {
            if (Char.IsDigit(phone[0]))
            {
                skipSymbols = 1;
                return true;
            }

            if (phone[0] == '+')
            {
                skipSymbols = 1;
                return true;
            }

            skipSymbols = 0;
            return false;
        }
    }
}