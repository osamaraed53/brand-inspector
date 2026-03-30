using BrandInspector.Constants;
using BrandInspector.Data;
using BrandInspector.Exceptions;
using BrandInspector.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BrandInspector.Services;

public class AuthService(IConfiguration configuration, IApplicationDbContext context) : IAuthService
{

    public async Task<string> Login(string username, string password, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            throw new BadRequestException(ErrorMessages.IsRequired);

        var user = await context.Users.SingleOrDefaultAsync(x => x.Username == username, cancellationToken) ?? throw new BadRequestException(ErrorMessages.InvalidCredentials);

        if (!VerifyPasswordHash(password, user.Password))
            throw new BadRequestException(ErrorMessages.InvalidCredentials);

        return GenerateToken(user.Id,username);
    }

    public async Task SignUp(string username, string password, CancellationToken cancellationToken)
    {
        if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))  
            throw new BadRequestException(ErrorMessages.IsRequired);

       if( await context.Users.AnyAsync(e=>e.Username == username, cancellationToken))
            throw new BadRequestException(ErrorMessages.UserExist);

       User user = new() { Password = HashingPassword(password), Username =username };

         await context.Users.AddAsync(user, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }

    private static string HashingPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    private static bool VerifyPasswordHash(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    private string GenerateToken(Guid id , string username)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

        var claims = new List<Claim>
        {
               new(ClaimTypes.NameIdentifier, id.ToString()),
               new(ClaimTypes.Name, username)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryInMinutes"]!)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings["Issuer"],
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

  
}
