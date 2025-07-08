namespace Both.Services;

public interface IUserService
{
    Task<UserProfile> GetOrCreateAsync(long telegramUserId);
    Task UpdateAsync(UserProfile user);
}
