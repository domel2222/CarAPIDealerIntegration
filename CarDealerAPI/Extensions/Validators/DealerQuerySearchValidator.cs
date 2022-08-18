using CarDealerAPI.Extensions;
using CarDealerAPI.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.Extensions.Validators
{
    public class DealerQuerySearchValidator : AbstractValidator<DealerQuerySearch>
    {
        private int[] possibleChose = new[] { 5, 10, 15, 20 };

        private string[] possibleColumnSort =
            {
            nameof(Dealer.DealerName),
            nameof(Dealer.Description),
            nameof(Dealer.Category),
            nameof(Dealer.Address.Country)
            };
        public DealerQuerySearchValidator()
        {
            RuleFor(p => p.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(p => p.PageSize).Custom((value, context)
                 =>
             {
                 if (!possibleChose.Contains(value))
                 {

                     context.AddFailure("Pagesize :", $"Page size must be between {string.Join(",", possibleChose)}");
                 }
             });

            RuleFor(r => r.SortBy)
                .Must(v => string.IsNullOrEmpty(v) || possibleColumnSort.Contains(v))
                .WithMessage($"Sort must be on the range {string.Join(",", possibleColumnSort)}");
        }
    }
}
