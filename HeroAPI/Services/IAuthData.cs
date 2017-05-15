using System.Threading.Tasks;

namespace HeroAPI.Services
{
    /// <summary>
    /// Repository for auth related actions
    /// </summary>
    public interface IAuthData
    {
        /// <summary>
        /// Add token to TokenStore
        /// </summary>
        /// <param name="tokenStoreEntity"></param>
        Task AddToken(TokenStore tokenStoreEntity);
        Task AddToken(string token, bool isValidToken);
        Task<TokenStore> GetToken(string token);
        Task<int> Commit();
    }
}