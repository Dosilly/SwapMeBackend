﻿using FluentValidation;

namespace SwapMe.Application.Handlers.Users.Requests;

public record AuthenticationRequest(string Login, string Password);

public class AuthenticationRequestValidator : AbstractValidator<AuthenticationRequest>
{
    public AuthenticationRequestValidator()
    {
        RuleFor(r => r.Login).NotEmpty();
        RuleFor(r => r.Password).NotEmpty();
    }   
}