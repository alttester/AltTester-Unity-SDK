/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
using System.Numerics;
using System.Security.Cryptography;

namespace AltTester.AltTesterUnitySDK.Communication.Handshake
{
    internal static class P256Verifier
    {
        static readonly BigInteger FP = H("FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFF");
        static readonly BigInteger FA = H("FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFC");
        static readonly BigInteger FN = H("FFFFFFFF00000000FFFFFFFFFFFFFFFFBCE6FAADA7179E84F3B9CAC2FC632551");
        static readonly BigInteger GX = H("6B17D1F2E12C4247F8BCE6E563A440F277037D812DEB33A0F4A13945D898C296");
        static readonly BigInteger GY = H("4FE342E2FE1A7F9B8EE7EB4A7C0F9E162BCE33576B315ECECBB6406837BF51F5");

        internal static bool VerifyFromSpki(byte[] spki, byte[] data, byte[] sig)
        {
            if (spki == null || spki.Length != 91 || spki[26] != 0x04) return false;
            var pubX = new byte[32];
            var pubY = new byte[32];
            Buffer.BlockCopy(spki, 27, pubX, 0, 32);
            Buffer.BlockCopy(spki, 59, pubY, 0, 32);
            return VerifyCoords(pubX, pubY, data, sig);
        }

        internal static bool VerifyCoords(byte[] pubX, byte[] pubY, byte[] message, byte[] sig)
        {
            byte[] r32, s32;
            if (sig != null && sig.Length == 64)
            {
                r32 = new byte[32]; s32 = new byte[32];
                Buffer.BlockCopy(sig, 0, r32, 0, 32);
                Buffer.BlockCopy(sig, 32, s32, 0, 32);
            }
            else if (!TryParseDer(sig, out r32, out s32))
            {
                return false;
            }

            var r = Dec32(r32);
            var s = Dec32(s32);
            if (r <= BigInteger.Zero || r >= FN) return false;
            if (s <= BigInteger.Zero || s >= FN) return false;

            byte[] hash;
            using (var sha = SHA256.Create())
                hash = sha.ComputeHash(message);

            var e  = Dec32(hash);
            var Qx = Dec32(pubX);
            var Qy = Dec32(pubY);

            var w  = MInv(s, FN);
            var u1 = Mod(e * w, FN);
            var u2 = Mod(r * w, FN);

            var pt = PAdd(PMul(GX, GY, u1), PMul(Qx, Qy, u2));
            if (pt.Inf) return false;

            return Mod(pt.X, FN) == r;
        }

        // ----- Point arithmetic (affine coordinates over FP) -----

        struct Pt { public BigInteger X, Y; public bool Inf; }
        static readonly Pt INF = new Pt { Inf = true };

        static Pt PAdd(Pt a, Pt b)
        {
            if (a.Inf) return b;
            if (b.Inf) return a;
            if (a.X == b.X)
                return a.Y == b.Y ? PDbl(a) : INF;

            var lam = Mod(Mod(b.Y - a.Y, FP) * MInv(Mod(b.X - a.X, FP), FP), FP);
            var rx  = Mod(lam * lam - a.X - b.X, FP);
            var ry  = Mod(lam * Mod(a.X - rx, FP) - a.Y, FP);
            return new Pt { X = rx, Y = ry };
        }

        static Pt PDbl(Pt p)
        {
            if (p.Inf) return p;
            var x2  = Mod(p.X * p.X, FP);
            var num = Mod(3 * x2 + FA, FP);
            var lam = Mod(num * MInv(Mod(2 * p.Y, FP), FP), FP);
            var rx  = Mod(lam * lam - 2 * p.X, FP);
            var ry  = Mod(lam * Mod(p.X - rx, FP) - p.Y, FP);
            return new Pt { X = rx, Y = ry };
        }

        static Pt PMul(BigInteger px, BigInteger py, BigInteger k)
        {
            var r = INF;
            var a = new Pt { X = px, Y = py };
            while (k > BigInteger.Zero)
            {
                if (!k.IsEven) r = PAdd(r, a);
                a = PDbl(a);
                k >>= 1;
            }
            return r;
        }

        // ----- Modular arithmetic -----

        static BigInteger Mod(BigInteger v, BigInteger m)
        {
            var r = v % m;
            return r.Sign < 0 ? r + m : r;
        }

        // Iterative extended GCD — O(log m) divisions, much faster than Fermat's.
        static BigInteger MInv(BigInteger a, BigInteger m)
        {
            a = Mod(a, m);
            var t = BigInteger.Zero;
            var newt = BigInteger.One;
            var r = m;
            var newr = a;
            while (newr != BigInteger.Zero)
            {
                var q = BigInteger.Divide(r, newr);
                var tmp = t - q * newt; t = newt; newt = tmp;
                var tmpr = r - q * newr; r = newr; newr = tmpr;
            }
            return t.Sign < 0 ? t + m : t;
        }

        // ----- Encoding helpers -----

        // 32-byte big-endian → positive BigInteger.
        static BigInteger Dec32(byte[] be)
        {
            var le = new byte[be.Length + 1]; // +1 zero byte forces positive
            for (int i = 0; i < be.Length; i++)
                le[i] = be[be.Length - 1 - i];
            return new BigInteger(le);
        }

        // Big-endian hex string → positive BigInteger.
        static BigInteger H(string hex)
        {
            int n = hex.Length / 2;
            var le = new byte[n + 1];
            for (int i = 0; i < n; i++)
                le[i] = Convert.ToByte(hex.Substring((n - 1 - i) * 2, 2), 16);
            return new BigInteger(le);
        }

        // Parse DER SEQUENCE { INTEGER r, INTEGER s } into 32-byte big-endian r/s.
        static bool TryParseDer(byte[] der, out byte[] r32, out byte[] s32)
        {
            r32 = s32 = null;
            if (der == null || der.Length < 8) return false;
            int pos = 0;
            if (der[pos++] != 0x30) return false;
            int seqLen = der[pos++];
            if ((seqLen & 0x80) != 0)
            {
                int ll = seqLen & 0x7F; seqLen = 0;
                if (pos + ll > der.Length) return false;
                for (int i = 0; i < ll; i++) seqLen = (seqLen << 8) | der[pos++];
            }
            if (pos + seqLen > der.Length) return false;
            return TryReadDerInt(der, ref pos, out r32) && TryReadDerInt(der, ref pos, out s32);
        }

        static bool TryReadDerInt(byte[] buf, ref int pos, out byte[] val32)
        {
            val32 = null;
            if (pos + 2 > buf.Length || buf[pos++] != 0x02) return false;
            int len = buf[pos++];
            if (pos + len > buf.Length || len > 33) return false;
            int start = pos; pos += len;
            if (len > 0 && buf[start] == 0x00) { start++; len--; } // strip sign byte
            if (len > 32) return false;
            val32 = new byte[32];
            Buffer.BlockCopy(buf, start, val32, 32 - len, len);
            return true;
        }
    }
}
