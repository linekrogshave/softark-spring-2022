using Newtonsoft.Json.Linq;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using JWT.Exceptions;

public class TokenService {
    private const string secret = "æblekage";

    public string GenerateToken(string username, string[] roles)
    {
        var payload = new Dictionary<string, object>
        {
            { "username", username },
            { "roles", roles}, 
            { "exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds()}
        };
        
        IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
        IJsonSerializer serializer = new JsonNetSerializer();
        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

        var token = encoder.Encode(payload, secret);
        Console.WriteLine(token);

        return token;
    }

    private string ParseToken(string token) {
        try
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
            
            var json = decoder.Decode(token, secret, verify: true);
            Console.WriteLine("Parsing JWT: " + json);
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

    public string[] GetRoles(string token) {
        var json = ParseToken(token);
        Console.WriteLine(json);
        dynamic stuff = JObject.Parse(json);
        return stuff.roles.ToObject<string[]>();
    }

    public string GetUsername(string token) {
        var json = ParseToken(token);
        Console.WriteLine(json);
        dynamic stuff = JObject.Parse(json);
        return stuff.username;
    }
}

