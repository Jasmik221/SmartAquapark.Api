using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;
using SmartAquapark.Application.DTOs;

namespace SmartAquapark.Application.Validators;

public class CreateTicketDtoValidator : AbstractValidator<CreateTicketDto>
{
    public CreateTicketDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.NumberOfPeople)
            .GreaterThan(0);

        RuleFor(x => x.ValidUntil)
            .GreaterThan(DateTime.UtcNow);
    }
}