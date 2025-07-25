﻿using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace MatrixBugtracker.API.Filters
{
    public class AuthorizeApiAttribute : ActionFilterAttribute
    {
        private readonly UserRole[] _roles;

        public AuthorizeApiAttribute()
        {
            _roles = Enum.GetValues<UserRole>().ToArray();
        }

        public AuthorizeApiAttribute(UserRole[] roles)
        {
            _roles = roles;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;
            if (user.Identity?.IsAuthenticated == false)
            {
                context.Result = new UnauthorizedObjectResult(ResponseDTO<object>.Unauthorized());
                return;
            }

            IUserService userService = context.HttpContext.RequestServices.GetService<IUserService>();
            var userIdStr = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (!int.TryParse(userIdStr, out int userId))
            {
                context.Result = new UnauthorizedObjectResult(ResponseDTO<object>.Unauthorized());
                return;
            }

            var userEntity = await userService.GetSingleUserAsync(userId);
            if (userEntity == null)
            {
                context.Result = new UnauthorizedObjectResult(ResponseDTO<object>.Unauthorized("Authenticated user not found or deleted"));
                return;
            }

            if (!_roles.Contains(userEntity.Role))
            {
                context.Result = new ObjectResult(ResponseDTO<object>.Forbidden())
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }

            await next();
        }
    }
}
