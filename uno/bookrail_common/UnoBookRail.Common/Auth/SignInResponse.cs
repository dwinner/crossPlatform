namespace UnoBookRail.Common.Auth;

public class SignInResponse
{
    public bool IsSuccessful { get; internal set; }

    public List<string> Messages { get; internal set; } = new();

    public User? UserDetails { get; internal set; }
}