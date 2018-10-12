using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PERI.Agenda.BLL
{
    /// <summary>
    /// Custom Validator - Whitespace validator for non-required field
    /// </summary>
    public class NoWhiteSpaceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value.GetType() == typeof(string))
            {
                var str = value as string;
                
                if (String.IsNullOrWhiteSpace(str))
                {
                    return new ValidationResult(validationContext.DisplayName + " value is just whitespace");
                }
            }

            return null;
        }
    }
}
