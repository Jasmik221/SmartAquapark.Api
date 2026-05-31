using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;
using SmartAquapark.Application.DTOs;

namespace SmartAquapark.Application.Validators;

public class GateExitDtoValidator : AbstractValidator<GateExitDto>
{
    public GateExitDtoValidator()
    {
        RuleFor(x => x.QrCode)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ZoneId)
            .GreaterThan(0);
    }
}