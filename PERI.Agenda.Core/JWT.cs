using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PERI.Agenda.Core
{
    public class JWT
    {
        /// <summary>
        /// Generates JWT
        /// </summary>
        /// <param name="id"></param>
        /// <param name="secret"></param>
        /// <returns>JWT string</returns>
        public static string GenerateToken(int id, string secret)
        {
            var add12HoursUtc = DateTime.UtcNow.AddHours(12);
            Int32 unixTimestamp = (Int32)(add12HoursUtc.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var payload = new Dictionary<string, object>
            {
                { "id", id },
                { "exp", unixTimestamp }
            };

            var token = encoder.Encode(payload, secret);

            return token;
        }
    }
}
