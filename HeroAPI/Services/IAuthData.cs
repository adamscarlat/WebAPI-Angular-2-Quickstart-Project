namespace HeroAPI.Services
{
    public interface IAuthData
    {
        void AddToken(TokenStore tokenStoreEntity);
        void AddToken(string token, bool isValidToken);
        TokenStore GetToken(string token);
        int Commit();
    }
}