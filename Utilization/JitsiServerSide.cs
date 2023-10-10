using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace sti_sys_backend.Utilization
{
    public enum PKType
    {
        // PKCS#1 type key
        PKCS1,
        // PKCS#8 type key
        PKCS8
    }

    public class JitsiServerSide
    {
        // Placeholder helper strings
        public const string BEGIN_PKCS1_PRIVATE_KEY = "-----BEGIN RSA PRIVATE KEY-----";
        public const string END_PKCS1_PRIVATE_KEY = "-----END RSA PRIVATE KEY-----";
        public const string BEGIN_PKCS8_PRIVATE_KEY = "-----BEGIN PRIVATE KEY-----";
        public const string END_PKCS8_PRIVATE_KEY = "-----END PRIVATE KEY-----";

        public const double EXP_TIME_DELAY_SEC = 7200;
        public const double NBF_TIME_DELAY_SEC = 10;

        private readonly IDictionary<string, object> userClaims = new Dictionary<string, object>();
        private readonly IDictionary<string, object> featureClaims = new Dictionary<string, object>();
        private readonly JwtPayload payload = new JwtPayload();
        private string apiKey = string.Empty;

        private JitsiServerSide() { }

        public static JitsiServerSide Builder()
        {
            return new JitsiServerSide();
        }

        public JitsiServerSide WithApiKey(string apiKey)
        {
            this.apiKey = apiKey;
            return this;
        }

        public JitsiServerSide WithUserAvatar(string url)
        {
            userClaims.Add("avatar", url);
            return this;
        }

        public JitsiServerSide WithModerator(bool isModerator)
        {
            userClaims.Add("moderator", isModerator ? "true" : "false");
            return this;
        }

        public JitsiServerSide WithUserName(string userName)
        {
            userClaims.Add("name", userName);
            return this;
        }

        public JitsiServerSide WithUserEmail(string userEmail)
        {
            userClaims.Add("email", userEmail);
            return this;
        }

        public JitsiServerSide WithLiveStreamingEnabled(bool isEnabled)
        {
            featureClaims.Add("livestreaming", isEnabled ? "true" : "false");
            return this;
        }

        public JitsiServerSide WithRecordingEnabled(bool isEnabled)
        {
            featureClaims.Add("recording", isEnabled ? "true" : "false");
            return this;
        }

        public JitsiServerSide WithOutboundCallEnabled(bool isEnabled)
        {
            featureClaims.Add("outbound-call", isEnabled ? "true" : "false");
            return this;
        }

        public JitsiServerSide WithTranscriptionEnabled(bool isEnabled)
        {
            featureClaims.Add("transcription", isEnabled ? "true" : "false");
            return this;
        }

        public JitsiServerSide WithExpTime(DateTime expTime)
        {
            payload.Add("exp", new DateTimeOffset(expTime).ToUnixTimeSeconds());
            return this;
        }

        public JitsiServerSide WithNbfTime(DateTime nbfTime)
        {
            payload.Add("nbf", new DateTimeOffset(nbfTime).ToUnixTimeSeconds());
            return this;
        }

        public JitsiServerSide WithRoomName(string roomName)
        {
            if (payload.ContainsKey("room"))
            {
                payload["room"] = roomName;
            }
            else
            {
                payload.Add("room", roomName);
            }
            return this;
        }

        public JitsiServerSide WithAppID(string appId)
        {
            payload.Add("sub", appId);
            return this;
        }
        public JitsiServerSide WithAudienceAndIssuer()
        {
            payload.Add("aud", "jitsi");
            payload.Add("iss", "chat");
            return this;
        }
        public JitsiServerSide WithUserId(string id)
        {
            userClaims.Add("id", id);
            return this;
        }

        public JitsiServerSide WithDefaults()
        {
            return WithExpTime(DateTime.UtcNow.AddSeconds(EXP_TIME_DELAY_SEC))
                .WithNbfTime(DateTime.UtcNow.AddSeconds(-NBF_TIME_DELAY_SEC))
                .WithAppID("vpaas-magic-cookie-d912c09dfba74cf2b05fe117f76fafd1")
                .WithAudienceAndIssuer()
                .WithLiveStreamingEnabled(true)
                .WithRecordingEnabled(true)
                .WithOutboundCallEnabled(true)
                .WithTranscriptionEnabled(true)
                .WithModerator(true)
                .WithRoomName("*")
                .WithUserId(Guid.NewGuid().ToString());
        }

        public string SignWith(RSA privateKey)
        {
            var rsaSecurityKey = new RsaSecurityKey(privateKey);
            var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(rsaSecurityKey);
            jwk.KeyId = apiKey;

            var context = new Dictionary<string, object>();
            context.Add("user", userClaims);
            context.Add("features", featureClaims);
            payload.Add("context", context);

            var cred = new SigningCredentials(jwk, SecurityAlgorithms.RsaSha256);
            var secToken = new JwtSecurityToken(new JwtHeader(cred), this.payload);
            var jwtHandler = new JwtSecurityTokenHandler();

            return jwtHandler.WriteToken(secToken);
        }

        public static RSA ReadPrivateKeyFromFile(string privateKeyFilePath, PKType pkType)
        {
            var rsa = RSA.Create();
            var privateKeyContent = File.ReadAllText(privateKeyFilePath, Encoding.UTF8);
            privateKeyContent = privateKeyContent.Replace(pkType == PKType.PKCS1 ? BEGIN_PKCS1_PRIVATE_KEY : BEGIN_PKCS8_PRIVATE_KEY, "");
            privateKeyContent = privateKeyContent.Replace(pkType == PKType.PKCS1 ? END_PKCS1_PRIVATE_KEY : END_PKCS8_PRIVATE_KEY, "");
            var privateKeyDecoded = Convert.FromBase64String(privateKeyContent);
            if (pkType == PKType.PKCS1)
            {
                rsa.ImportRSAPrivateKey(privateKeyDecoded, out _);
            }
            else
            {
                rsa.ImportPkcs8PrivateKey(privateKeyDecoded, out _);
            }

            return rsa;
        }
    }
}
