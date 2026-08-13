public static class PasswordHasher
{
    public static string HashPassword(string passsword)
    {
        return BCrypt.Net.BCrypt.HashPassword(passsword);
    }
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}