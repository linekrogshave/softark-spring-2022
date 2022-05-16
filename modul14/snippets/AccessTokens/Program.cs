using Newtonsoft.Json.Linq;

// Se dokumentation her: https://github.com/jwt-dotnet/jwt
// Installeres med 'dotnet add package JWT'
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using JWT.Exceptions;

var tokenService = new TokenService();
var token = tokenService.GenerateToken("Kristian");
Console.WriteLine(token);
Console.WriteLine(tokenService.IsTokenValid(token));
Console.WriteLine(tokenService.ParseToken(token));
Console.WriteLine(tokenService.GetRole(token));
Console.WriteLine(tokenService.GetExp(token));

public class TokenService {
    // Meget hemmelig nøgle som ikke burde ligge i koden:
    private const string secretKey = "banankage";

    public string GenerateToken(string username)
    {
        var payload = new Dictionary<string, object>
        {
            { "Username", username },
            { "Role", "Admin"}, // TODO: Don't make everyone Admin 😊
            { "exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds()}
        };
        
        IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // Hash-funktionen
        IJsonSerializer serializer = new JsonNetSerializer(); // Laver om til JSON
        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder(); // Laver URL-encoding
        IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder); // Selve encoderen

        // Encode en ny token som indeholder payload
        var token = encoder.Encode(payload, secretKey);
        return token;
    }

    public string ParseToken(string token) {
        try
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
            
            var json = decoder.Decode(token, secretKey, verify: true);
            return json;
        }
        catch (TokenExpiredException)
        {
            Console.WriteLine("Token has expired");
            return null!;
        }
        catch (SignatureVerificationException)
        {
            Console.WriteLine("Token has invalid signature");
            return null!;
        }
    }

    public bool IsTokenValid(string token) {
        return !String.IsNullOrEmpty(ParseToken(token));
    }

    public string GetRole(string token) {
        var json = ParseToken(token);
        dynamic stuff = JObject.Parse(json);
        string role = stuff.Role;
        return role;
    }

    public DateTime GetExp(string token) {
        var json = ParseToken(token);
        dynamic stuff = JObject.Parse(json);
        DateTime exp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(stuff.exp.ToString())).LocalDateTime;
        return exp;
    }
}

