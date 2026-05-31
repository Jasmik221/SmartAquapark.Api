using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using SmartAquapark.Application.DTOs;

namespace SmartAquapark.Application.Validators;

public class AssignWristbandDtoValidator : AbstractValidator<AssignWristbandDto>
{
    public AssignWristbandDtoValidator()
    {
        RuleFor(x => x.TicketId)
            .GreaterThan(0);

        RuleFor(x => x.QrCode)
            .NotEmpty()
            .MaximumLength(100);
    }
}