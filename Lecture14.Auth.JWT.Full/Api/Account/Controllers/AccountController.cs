using Lecture14.Auth.JWT.Full.Api.Account.Contract;
using Lecture14.Auth.JWT.Full.Api.Account.Services;
using Lecture14.Auth.JWT.Full.Api.Account.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lecture14.Auth.JWT.Full.Api.Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(
        IUserService userService,
        ITokenService tokenService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(CredentialsRequest credentials, CancellationToken cancellationToken)
        {
            var result = await userService.Register(credentials, cancellationToken);

            if (!result.Succeeded)
            {
                return BadRequest(new { result.Error });
            }

            return Ok(ToResponse(result.Tokens!));
        }

        [HttpPost("token/create")]
        public async Task<IActionResult> Login(CredentialsRequest credentials, CancellationToken cancellationToken)
        {
            var result = await userService.Login(credentials, cancellationToken);

            if (!result.Succeeded)
            {
                return Unauthorized(new { result.Error });
            }

            return Ok(ToResponse(result.Tokens!));
        }

        [HttpPost("token/refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var tokens = await tokenService.RefreshAsync(request.RefreshToken, cancellationToken);

            if (tokens is null)
            {
                return Unauthorized(new { Error = "Invalid refresh token." });
            }

            return Ok(ToResponse(tokens));
        }

        [HttpPost("token/revoke")]
        public async Task<IActionResult> Revoke(RevokeRefreshTokenRequest request, CancellationToken cancellationToken)
        {
            await tokenService.RevokeAsync(request.RefreshToken,cancellationToken);

            return NoContent();
        }

        private static TokenPairResponse ToResponse(TokenPair tokens)
        {
            return new TokenPairResponse
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                AccessTokenExpiresAt = tokens.AccessTokenExpiresAt,
                RefreshTokenExpiresAt = tokens.RefreshTokenExpiresAt
            };
        }
    }
}