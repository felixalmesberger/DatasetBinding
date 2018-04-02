using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TestApp
{
  public class User
  {
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [Mail]
    public string Mail { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    [WarnForWeakPassword]
    public string Password { get; set; }

  }

  public class MailAttribute : ValidationAttribute
  {
    private static readonly Regex MailRegex = new Regex("^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");

    protected override ValidationResult IsValid(object value, ValidationContext ctx)
    {
      var mail = value as string;

      if(mail == null)
        return ValidationResult.Success;

      return MailRegex.IsMatch(mail)
        ? ValidationResult.Success 
        : new ValidationResult("Not a valid mail", new [] { ctx.MemberName }) ;

    }
  }

  public class WarnForWeakPasswordAttribute : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      var password = value as string;

      if(password == null)
        return ValidationResult.Success;

      if(password.Length < 8)
        return new ValidationResult("Password is weak.");
      else
        return ValidationResult.Success;

    }
  }

}