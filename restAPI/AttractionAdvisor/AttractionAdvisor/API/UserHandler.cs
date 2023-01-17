using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace AttractionAdvisor.API;
using DataAccess;
using Models;

class UserHandler
{
    private readonly AttractionAdvisorDbContext _context;

    public UserHandler(AttractionAdvisorDbContext context)
    {
        _context = context;
    }

    public async Task<int> Register(User user)
    {
        var userExists = await _context.Users.AnyAsync(
            x => x.UserName == user.UserName);
        if (userExists)
        {
            throw new Exception("User already exists");
        }

        var newUser = new User
        {
            UserName = user.UserName,
            Password = user.Password
        };

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return newUser.Id;
    }

    public async Task<bool> Login(User user)
    {
        // Check if the deserialized user object is valid.
        if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Password))
        {
            return false;
        }

        // Look for a match of credentials in the database.
        var found = await _context.Users.SingleOrDefaultAsync(
            u => u.UserName == user.UserName && u.Password == user.Password);

        return found != null;
    }
}

