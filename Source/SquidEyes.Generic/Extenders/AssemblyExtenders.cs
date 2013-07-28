using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Linq;
using System.IO;

namespace SquidEyes.Generic
{
    public static partial class AssemblyExtenders
    {
        [DllImport("mscoree.dll", CharSet = CharSet.Unicode)]
        private static extern bool StrongNameSignatureVerificationEx(
            string filePath, bool forceVerification, ref bool wasVerified);

        public static T GetAttribute<T>(this Assembly callingAssembly)
            where T : Attribute
        {
            T result = null;

            var configAttributes = Attribute.
                GetCustomAttributes(callingAssembly, typeof(T), false);

            if (!configAttributes.IsNullOrEmpty())
                result = (T)configAttributes[0];

            return result;
        }

        public static bool IsTrusted(this Assembly target, byte[] publicKey)
        {
            return IsStrongNamed(target) && IsSignedWith(target, publicKey);
        }

        private static bool IsSignedWith(this Assembly target, byte[] publicKey)
        {
            if (publicKey != null)
            {
                if (target.GetName().GetPublicKey().SequenceEqual(publicKey))
                    return true;
            }

            return false;
        }

        private static bool IsStrongNamed(this Assembly target)
        {
            bool notForced = false;

            bool verified = StrongNameSignatureVerificationEx(
                target.Location, false, ref notForced);

            return verified;
        }

        public static string GetResourceString(this Assembly assembly, string name)
        {
            string result;

            using (var stream = assembly.GetManifestResourceStream(name))
            {
                var reader = new StreamReader(stream);

                result = reader.ReadToEnd();
            }

            return result;
        }

        public static byte[] GetResourceBytes(this Assembly assembly, string name)
        {
            byte[] bytes;

            using (var stream = assembly.GetManifestResourceStream(name))
            {
                var reader = new BinaryReader(stream);

                bytes = reader.ReadBytes((int)stream.Length);
            }

            return bytes;
        }
    }
}
