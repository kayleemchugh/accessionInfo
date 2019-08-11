using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;



public class ServiceUtils
    {
        private static readonly JwtSecurityTokenHandler JwtTokenHandler = new JwtSecurityTokenHandler();


        
        private string serverName {get; set; }
        public string AccessKey { get; }

        public string Endpoint { get; }

        public string url { get; set; } 

          private readonly string _endpoint;

        private string GenerateServerName()
        {
           return $"{Environment.MachineName}_{Guid.NewGuid():N}";
        }

        public ServiceUtils(String serverType, string connectionString, string hubName)
        {
            (Endpoint, AccessKey) = ParseConnectionString(connectionString);
           // client
            if(serverType == "client") 
            {
                url = $"{Endpoint}/client/?hub={hubName}";
            } else
             {
                url = GetBroadcastUrl(hubName);
            }

        }


        private string GetBroadcastUrl(string hubName)
        {
            return $"{GetBaseUrl(hubName)}";
        }

        private string GetBaseUrl(string hubName)
        {
            return $"{Endpoint}/api/v1/hubs/{hubName.ToLower()}";
        }

         public string GenerateAccessToken()
        {
            IEnumerable<Claim> claims = null;
            string userId = GenerateServerName();

            if (userId != null)
            {
                claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                };
            }
            var token = GenerateAccessTokenInternal(url, claims);
            return token;
        }

        public string GenerateAccessTokenInternal(string audience, IEnumerable<Claim> claims)
        {
        
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AccessKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = JwtTokenHandler.CreateJwtSecurityToken(
                issuer: null,
                audience: audience,
                subject: claims == null ? null : new ClaimsIdentity(claims),
                expires: DateTime.UtcNow.AddDays(200),
                signingCredentials: credentials);
            return JwtTokenHandler.WriteToken(token);
        }
        private static readonly char[] PropertySeparator = { ';' };

        private static readonly char[] KeyValueSeparator = { '=' };

         private const string EndpointProperty = "endpoint";
        private const string AccessKeyProperty = "accesskey";

        internal static (string, string) ParseConnectionString(string connectionString)
        {
            var properties = connectionString.Split(PropertySeparator, StringSplitOptions.RemoveEmptyEntries);
            if (properties.Length > 1)
            {
                var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var property in properties)
                {
                    var kvp = property.Split(KeyValueSeparator, 2);
                    if (kvp.Length != 2) continue;

                    var key = kvp[0].Trim();
                    if (dict.ContainsKey(key))
                    {
                        throw new ArgumentException($"Duplicate properties found in connection string: {key}.");
                    }

                    dict.Add(key, kvp[1].Trim());
                }

                if (dict.ContainsKey(EndpointProperty) && dict.ContainsKey(AccessKeyProperty))
                {
                    return (dict[EndpointProperty].TrimEnd('/'), dict[AccessKeyProperty]);
                }
            }

            throw new ArgumentException($"Connection string missing required properties {EndpointProperty} and {AccessKeyProperty}.");
        }

        private void GetClientUrl(string endpoint, string hubName)
        {
            url = $"{endpoint}/client/?hub={hubName}";
        }
    }