/*
    Copyright(C) 2026 Altom Consulting
*/

namespace AltTester.AltTesterUnitySDK.Communication.Handshake
{
    public class HandshakeChallengeMessage
    {
        public string commandName = "handshakeChallenge";
        public string nonce;
        public string appName;
        public string deviceInstanceId;
        public string sdkVersion;
    }

    public class HandshakeTokenMessage
    {
        public string commandName;
        public string cert;
        public string sig;
    }

    public class SessionCertClaims
    {
        public string ephemeralPubKey;
        public string sessionId;
        public string productId;
        public long iat;
        public long exp;
        public string kid;
        public string aud;
    }

    public class JwsHeader
    {
        public string alg;
        public string kid;
    }

    public enum HandshakeResult
    {
        Valid,
        MissingToken,
        UnknownKey,
        InvalidCertSig,
        ExpiredCert,
        InvalidClaims,
        InvalidNonceSig,
    }
}
