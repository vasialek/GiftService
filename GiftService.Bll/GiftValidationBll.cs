using GiftService.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{
    public class GiftValidationBll
    {
        private IConfigurationBll _configurationBll = null;
        private IValidationBll _validationBll = null;

        public GiftValidationBll(IValidationBll validationBll, IConfigurationBll configurationBll)
        {
            if (configurationBll == null)
            {
                throw new ArgumentNullException("configurationBll");
            }
            if (validationBll == null)
            {
                throw new ArgumentNullException("validationBll");
            }

            _configurationBll = configurationBll;
            _validationBll = validationBll;
        }

        public void EnsureMakeCouponGiftIsValid(string text, string friendEmail)
        {
            List<ValidationError> errors = new List<ValidationError>();
            try
            {
                var ms = _configurationBll.Get();
                _validationBll.EnsureTextLength(text, ms.MinGiftCongratulationsLength, ms.MaxGiftCongratulationsLength);
            }
            catch (ValidationException vex)
            {
                vex.Error.FieldName = "text";
                errors.Add(vex.Error);
            }
            catch (ArgumentNullException anex)
            {
                errors.Add(new ValidationError(ValidationErrors.Empty, anex.Message, "text"));
            }
            catch (Exception ex)
            {
                errors.Add(new ValidationError(ValidationErrors.RuleViolation, ex.Message, "text"));
            }

            _validationBll.IsEmailValid(friendEmail, true, errors);
            foreach (var e in errors.Where(x => x.FieldName != "text"))
            {
                e.FieldName = "friendEmail";
            }

            if (errors.Count > 0)
            {
                throw new ValidationListException("Incorrect coupon to gift parameters", errors);
            }
        }

    }
}
