using CarDealerAPI.Contexts;
using CarDealerAPI.DTOS;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.Extensions.Validators
{
    public class RegisterDtoValidator : AbstractValidator<UserCreateDTO>
    {
        public RegisterDtoValidator(DealerDbContext dealerDbContext)
        {
            RuleFor(e => e.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(e => e.Email)
                .Custom((email, context) =>
                {
                    var checkEmail = dealerDbContext.Users.Any(u => u.Email == email);
                    if (checkEmail)
                    {
                        context.AddFailure("Email :", "Please insert another email this is taken");
                    }
                });

            RuleFor(p => p.Password).MinimumLength(6).MaximumLength(150);

            RuleFor(p => p.ConfirmPassword).Equal(p => p.Password);
 
        }
    }
}
