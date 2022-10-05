using System.Security.Claims;
using System.Text.Json;

namespace BPSGobalClient.Services
{
    public static class JwtParser
    {
        [System.Diagnostics.DebuggerStepThrough]
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);


            //Fixing!!!!!
            var FixedKVP = FixNameForMicrosft(keyValuePairs);


            ExtractRolesFromJWT(claims, FixedKVP);

            claims.AddRange(FixedKVP.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }


        private static void ExtractRolesFromJWT(List<Claim> claims, Dictionary<string, object> keyValuePairs)
        {
            //keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);


            keyValuePairs.TryGetValue("role", out object roles);

            //keyValuePairs.TryGetValue("role", out object roles2);

            if (roles != null)
            {
                var parsedRoles = roles.ToString().Trim().TrimStart('[').TrimEnd(']').Split(',');
                if (parsedRoles.Length > 1)
                {
                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole.Trim('"')));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsedRoles[0]));
                }
                keyValuePairs.Remove(ClaimTypes.Role);
            }
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }


        /// <summary>
        /// Fixes the name "type" for Microsoft auth.
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        private static Dictionary<string, object> FixNameForMicrosft(Dictionary<string, object> pairs)
        {
            var FixedList = new Dictionary<string, object>();

            foreach(var i in pairs)
            {
                string kk = i.Key;
                object vv = i.Value;

                if (kk == "name")
                {
                    kk = ClaimTypes.Name;
                }
                               
                FixedList.Add(kk,vv);
            }

            return FixedList;
        }
    }
}
