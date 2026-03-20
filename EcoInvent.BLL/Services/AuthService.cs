using EcoInvent.DAL.Repositories;

namespace EcoInvent.BLL.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly Logger _logger;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _logger = new Logger();
        }

        public async Task<(bool Success, string Role, string Message)> LoginAsync(string username, string password)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(username.Trim());

                if (user == null)
                    return (false, string.Empty, "Invalid username or password.");

                bool valid = PasswordHasher.VerifyPassword(password, user.PasswordSalt, user.PasswordHash);

                if (!valid)
                    return (false, string.Empty, "Invalid username or password.");

                _logger.LogInfo($"User logged in: {username}");
                return (true, user.Role, "Login successful.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AuthService.LoginAsync");
                return (false, string.Empty, "System error during login.");
            }
        }
    }
}