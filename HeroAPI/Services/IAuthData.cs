namespace HeroAPI.Services
{
    public interface IAuthData
    {
        /// <summary>
        /// Add token to TokenStore
        /// </summary>
        /// <param name="tokenStoreEntity"></param>
        void AddToken(TokenStore tokenStoreEntity);
        void AddToken(string token, bool isValidToken);
        TokenStore GetToken(string token);
        int Commit();
    }
}