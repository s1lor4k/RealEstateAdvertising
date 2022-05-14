﻿using System.Security.Authentication;
using System.Security.Claims;
using Application.Services.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class ContextService : IContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<User> _userManager;

    public ContextService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<User> GetCurrentUser()
    {
        var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext?.User.Identity!;

        if (!identity?.IsAuthenticated ?? false) throw new AuthenticationException();

        var userId = identity!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ??
                     throw new AuthenticationException();

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null) throw new AuthenticationException();

        return user;
    }
}