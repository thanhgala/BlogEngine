using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using FrameworkCore.Infrastructure.Common;

namespace FrameworkCore.Infrastructure.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ImageFileLengthAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var file = (IFormFile) value;
                var exts = new List<string> { ".png", ".jpg" };
                var ext = Path.GetExtension(file.FileName).ToLower();

                if (!exts.Contains(ext))
                {
                    return new ValidationResult(string.Format("You must upload JPG, PNG, GIF file under 5MB.", validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }
    }
}
