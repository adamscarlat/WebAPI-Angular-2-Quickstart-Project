namespace HeroAPI.Services
{
    public interface IAuthData
    {
        void AddToken(TokenStore token);
        TokenStore GetToken(string token);
        int Commit();

    }
}