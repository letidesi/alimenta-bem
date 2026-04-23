using Microsoft.IdentityModel.Tokens;
using AlimentaBem.Src.Modules.User.Repository;

namespace AlimentaBem.Src.Providers.Crypto;

public interface ICryptoProvider
{
    string generateAccessToken(User user);
    string generateRefreshToken(User user);
    Task<TokenValidationResult> validateRefreshToken(string token);
}