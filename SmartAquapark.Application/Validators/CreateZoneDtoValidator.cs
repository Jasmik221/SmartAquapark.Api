using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;
using SmartAquapark.Application.DTOs;

namespace SmartAquapark.Application.Validators;

public class CreateZoneDtoValidator : AbstractValidator<CreateZoneDto>
{
    public CreateZoneDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.CapacityLimit)
            .GreaterThan(0);
    }
}