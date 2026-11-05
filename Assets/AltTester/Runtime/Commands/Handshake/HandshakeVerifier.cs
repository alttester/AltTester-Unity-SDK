/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace AltTester.AltTesterUnitySDK.Communication.Handshake
{
    internal static class HandshakeVerifier
    {
        private static readonly (string kid, string spkiBase64)[] _mwPublicKeys = new (string, string)[]
        {
            ("g1", "MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEu6PX2R8ahiSzvmu1yWSVplo30qSv" +
                   "+p8h8AGxKz+h7u60kBsIaodSYAA59Zql3HqpVon0T2r08kRM8xBTjaIHkw=="),
        };

        internal static HandshakeResult Verify(
            HandshakeTokenMessage token,
            string pendingNonce)
        {
            if (token == null || string.IsNullOrEmpty(token.cert) || string.IsNullOrEmpty(token.sig))
                return HandshakeResult.MissingToken;

            SessionCertClaims claims = VerifyCert(token.cert, out HandshakeResult certResult);
            if (certResult != HandshakeResult.Valid)
                return certResult;

            if (!VerifyNonceSig(token.sig, pendingNonce, claims.ephemeralPubKey))
                return HandshakeResult.InvalidNonceSig;


            return HandshakeResult.Valid;
        }

        private static SessionCertClaims VerifyCert(string jws, out HandshakeResult result)
        {
            result = HandshakeResult.InvalidCertSig;

            var parts = jws.Split('.');
            if (parts.Length != 3)
                return null;

            JwsHeader header;
            try
            {
                header = JsonConvert.DeserializeObject<JwsHeader>(Base64UrlDecodeString(parts[0]));
            }
            catch
            {
                return null;
            }

            if (header == null || header.alg != "ES256")
                return null;

            byte[] mwSpki = FindEmbeddedKey(header.kid);
            if (mwSpki == null)
            {
                result = HandshakeResult.UnknownKey;
                return null;
            }

            byte[] signingInput = Encoding.ASCII.GetBytes(parts[0] + "." + parts[1]);
            byte[] sig = Base64UrlDecodeBytes(parts[2]);

            if (!VerifyWithSpki(mwSpki, signingInput, sig))
                return null;

            SessionCertClaims claims;
            try
            {
                // Explicit token reads (not reflection-based POCO population): Unity's
                // IL2CPP/Mono player drops reflection-populated fields on some targets
                // (observed on x86_64: the string fields declared before the first
                // value-type field come back empty), which broke ephemeralPubKey.
                var body = Newtonsoft.Json.Linq.JObject.Parse(Base64UrlDecodeString(parts[1]));
                claims = new SessionCertClaims
                {
                    ephemeralPubKey = (string)body["ephemeralPubKey"],
                    sessionId = (string)body["sessionId"],
                    productId = (string)body["productId"],
                    iat = body["iat"] != null ? (long)body["iat"] : 0,
                    exp = body["exp"] != null ? (long)body["exp"] : 0,
                    kid = (string)body["kid"],
                    aud = (string)body["aud"],
                };
            }
            catch
            {
                result = HandshakeResult.InvalidClaims;
                return null;
            }

            if (claims == null || claims.aud != "alttester-sdk")
            {
                result = HandshakeResult.InvalidClaims;
                return null;
            }

            long nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (nowUnix > claims.exp)
            {
                result = HandshakeResult.ExpiredCert;
                return null;
            }

            result = HandshakeResult.Valid;
            return claims;
        }

        private static bool VerifyNonceSig(string sigBase64Url, string nonce, string ephemeralPubKeyBase64Url)
        {
            if (string.IsNullOrEmpty(ephemeralPubKeyBase64Url) || string.IsNullOrEmpty(nonce))
                return false;
            try
            {
                byte[] spki = Base64UrlDecodeBytes(ephemeralPubKeyBase64Url);
                byte[] nonceBytes = Base64UrlDecodeBytes(nonce);
                byte[] sigBytes = Base64UrlDecodeBytes(sigBase64Url);
                return VerifyWithSpki(spki, nonceBytes, sigBytes);
            }
            catch
            {
                return false;
            }
        }


        private static bool VerifyWithSpki(byte[] spki, byte[] data, byte[] sig)
        {
            // Fast path: the platform's native ECDsa. On some Unity player runtimes
            // (e.g. x86_64 Mono) ImportSubjectPublicKeyInfo is not implemented and
            // throws, and depending on the API compatibility level VerifyData may
            // interpret the ES256 signature (P1363 vs DER) differently, so only a
            // "true" here is authoritative.
            try
            {
                using (var ecdsa = ECDsa.Create())
                {
                    ecdsa.ImportSubjectPublicKeyInfo(spki, out _);
                    if (VerifyEcSignature(ecdsa, data, sig))
                        return true;
                }
            }
            catch { }

            // Authoritative fallback: pure-managed P-256 verifier, independent of the
            // runtime crypto backend and signature format.
            return P256Verifier.VerifyFromSpki(spki, data, sig);
        }

        private static bool VerifyEcSignature(ECDsa ecdsa, byte[] data, byte[] sig)
        {
            try { if (ecdsa.VerifyData(data, sig, HashAlgorithmName.SHA256)) return true; }
            catch { }
            if (sig.Length == 64)
            {
                try { return ecdsa.VerifyData(data, IeeeP1363ToDer(sig), HashAlgorithmName.SHA256); }
                catch { }
            }
            return false;
        }

        private static byte[] FindEmbeddedKey(string kid)
        {
            if (string.IsNullOrEmpty(kid))
                return null;
            foreach (var (k, spkiBase64) in _mwPublicKeys)
            {
                if (k == kid)
                    return Convert.FromBase64String(spkiBase64);
            }
            return null;
        }

        private static byte[] IeeeP1363ToDer(byte[] p1363)
        {
            byte[] r = new byte[32];
            byte[] s = new byte[32];
            Buffer.BlockCopy(p1363, 0, r, 0, 32);
            Buffer.BlockCopy(p1363, 32, s, 0, 32);

            byte[] rDer = PrependZeroIfHighBitSet(r);
            byte[] sDer = PrependZeroIfHighBitSet(s);

            int seqLen = 2 + rDer.Length + 2 + sDer.Length;
            byte[] der = new byte[2 + seqLen];
            int pos = 0;
            der[pos++] = 0x30;
            der[pos++] = (byte)seqLen;
            der[pos++] = 0x02;
            der[pos++] = (byte)rDer.Length;
            Buffer.BlockCopy(rDer, 0, der, pos, rDer.Length); pos += rDer.Length;
            der[pos++] = 0x02;
            der[pos++] = (byte)sDer.Length;
            Buffer.BlockCopy(sDer, 0, der, pos, sDer.Length);
            return der;
        }

        private static byte[] PrependZeroIfHighBitSet(byte[] b)
        {
            if ((b[0] & 0x80) == 0) return b;
            var result = new byte[b.Length + 1];
            result[0] = 0x00;
            Buffer.BlockCopy(b, 0, result, 1, b.Length);
            return result;
        }

        private static string Base64UrlDecodeString(string s)
            => Encoding.UTF8.GetString(Base64UrlDecodeBytes(s));

        private static byte[] Base64UrlDecodeBytes(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            switch (s.Length % 4)
            {
                case 2: s += "=="; break;
                case 3: s += "="; break;
            }
            return Convert.FromBase64String(s);
        }
    }
}
