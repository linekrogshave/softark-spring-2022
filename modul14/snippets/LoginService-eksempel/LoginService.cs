using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

public class LoginService
{
    // TODO: Gem 'users' i database, ikke her i koden 😊
    public record UserRecord(string username, string hashedPassword, string salt, string[] roles);

    private List<UserRecord> userDatabase = new List<UserRecord>();

    public LoginService() {;
        CreateLogin("kristian", "banankage", new string[] {"admin"});
        CreateLogin("kell", "password123", new string[] {"user"});
        CreateLogin("klaus", "secret", new string[] {"user"});
    }

    public UserRecord CreateLogin(string username, string password, string[] roles)
    {
        // lav et 128-bit salt 
        byte[] saltBytes = new byte[128 / 8];
        using (var rngCsp = new RNGCryptoServiceProvider())
        {
            rngCsp.GetNonZeroBytes(saltBytes);
        }
        string salt = Convert.ToBase64String(saltBytes);

        // lav en 256-bit hash med HMACSHA256 - kør den 100.000 gange
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: Convert.FromBase64String(salt),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        var record = new UserRecord(username, hashed, salt, roles);
        userDatabase.Add(record);
        return record;
    }

    public bool ValidateLogin(string username, string password)
    {
        Console.WriteLine("ValidateLogin");
        UserRecord record = userDatabase.Find(user => user.username == username);
        string newHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: Convert.FromBase64String(record.salt),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        if (newHashed != record.hashedPassword) {
            Console.WriteLine($"Login failed for {username}/{password}");
        } else {
            Console.WriteLine($"Login succeded for {username}/{password}");
        }

        return newHashed == record.hashedPassword;
    }
}

