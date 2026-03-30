namespace BrandInspector.Services;
public interface IAuthService 
{
    Task<string> Login(string username, string password, CancellationToken cancellationToken);

    Task SignUp(string username, string password, CancellationToken cancellationToken);

}
