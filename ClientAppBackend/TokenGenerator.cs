using Jose;
using Newtonsoft.Json;
using Nexmo.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace ClientAppBackend
{
    public class TokenGenerator
    {

        private const int SECONDS_EXPIRY = 3600;

        public static string GenerateToken(string appId, string privateKeyPath, string subject)
        {
            string privateKey;
            using (var reader = File.OpenText(privateKeyPath)) // file containing RSA PKCS1 private key
                privateKey = reader.ReadToEnd();

            var tokenData = new byte[64];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(tokenData);
            var jwtTokenId = Convert.ToBase64String(tokenData);
            var t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var exp = ((Int32)(t.TotalSeconds + SECONDS_EXPIRY)).ToString(); // Unix timestamp for when the token expires
            var payload = new Dictionary<string, object>
            {
                { "iat", (long) (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds },
                { "jti", Guid.NewGuid() },
                { "sub",subject},
                { "exp", exp},                
                { "acl", new Acls()},                
                { "application_id", appId }
            };

            var rsa = PemParse.DecodePEMKey(privateKey);

            return JWT.Encode(payload, rsa, JwsAlgorithm.RS256);
        }

        public class Acls
        {
            [JsonProperty("paths")]
            public Paths paths { get; set; } = new Paths();
        }

        public class Paths
        {
            [JsonProperty("/*/users/**")]
            public object users { get; set; } = new object();
            [JsonProperty("/*/conversations/**")]
            public object conversations { get; set; } = new object();
            [JsonProperty("/*/sessions/**")]
            public object sessions { get; set; } = new object();
            [JsonProperty("/*/devices/**")]
            public object devices { get; set; } = new object();
            [JsonProperty("/*/image/**")]
            public object image { get; set; } = new object();
            [JsonProperty("/*/media/**")]
            public object media { get; set; } = new object();
            [JsonProperty("/*/applications/**")]
            public object applications { get; set; } = new object();
            [JsonProperty("/*/push/**")]
            public object push { get; set; } = new object();
            [JsonProperty("/*/knocking/**")]
            public object knocking { get; set; } = new object();
        }
    }
}
