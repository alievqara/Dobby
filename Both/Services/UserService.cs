using Both.Data;
using Both.Models;
using Microsoft.EntityFrameworkCore;

namespace Both.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfile> GetOrCreateAsync(long userId)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user != null)
            return user;

        // Yeni kullanıcı oluştur
        user = new UserProfile
        {
            TelegramUserId = userId,
            Step = 0
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task UpdateAsync(UserProfile user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
