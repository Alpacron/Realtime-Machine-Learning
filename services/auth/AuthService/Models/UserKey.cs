namespace AuthService.Models;

public struct UserKey : IEquatable<User>
{
    private readonly int? Id = null;
    private readonly string? Email = null;

    public UserKey(User user)
    {
        Id = user.Id;
        Email = user.Email;
    }

    public UserKey(int id)
    {
        Id = id;
    }

    public UserKey(string email)
    {
        Email = email;
    }

    public UserKey(int id, string email)
    {
        Id = id;
        Email = email;
    }

    public bool Equals(User? user) => user is not null && user.Id == Id && user.Email == Email;
    public bool Equals(UserKey key) => key.Id == Id || key.Email == Email;

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj is User user)
            return Equals(user);
        if (obj is UserKey key)
            return Equals(key);
        return false;
    }
}
