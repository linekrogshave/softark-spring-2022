using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Encodings.Web;

public class JwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private TokenService tokenService;

    public JwtAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        TokenService tokenService
        ) : base(options, logger, encoder, clock)
    {
        this.tokenService = tokenService;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authHeader = Request.Headers["Authorization"].ToString();

        if (Request.Path.Value.StartsWith("/api/users")) {
            return Task.FromResult(AuthenticateResult.NoResult());
        }
        
        if (authHeader != null && authHeader.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();
            if (tokenService.IsTokenValid(token))
            {
                var claims = new List<Claim>();
                Console.WriteLine("Roles:");
                Console.WriteLine(tokenService.GetRoles(token));
                foreach(string role in tokenService.GetRoles(token))
                {
                    claims.Add(new Claim("Role", role));
                }
                var identity = new ClaimsIdentity(claims);
                var claimsPrincipal = new ClaimsPrincipal(identity);
                return Task.FromResult(AuthenticateResult.Success(
                    new AuthenticationTicket(claimsPrincipal, "JwtAuthentication")));
            }
        }
 
        Response.StatusCode = 401;
        return Task.FromResult(AuthenticateResult.Fail("Invalid Token"));  
    }
}