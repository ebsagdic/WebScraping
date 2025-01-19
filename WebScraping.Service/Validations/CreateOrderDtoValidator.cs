using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Core.Dto_s;

namespace WebScraping.Service.Validations
{
    public class CreateOrderDtoValidator:AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.TrackingNo).NotNull().WithMessage(" TrackingNo is required");
            RuleFor(x => x.TrackingNo).NotNull().MinimumLength(13).WithMessage("TrackingNo 13 Karakterden az olamaz");
            RuleFor(x => x.TrackingNo).NotNull().MaximumLength(13).WithMessage("TrackingNo 13 Karakterden fazla olamaz");
            RuleFor(x => x.Status).NotNull().WithMessage(" Situation is required");
        }
    }
}
