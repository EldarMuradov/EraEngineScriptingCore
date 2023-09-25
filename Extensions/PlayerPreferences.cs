using EngineLibrary.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EngineLibrary.Extensions
{
    public class NewRange
    {
        public static int GenerateRandonRange(int min, int max)
        {
            int value;
            System.Random random = new System.Random();
            value = random.Next(min, max);
            return value;
        }
    }

    public struct SafeFloat
    {
        private readonly int _value;
        private readonly int _salt;

        public SafeFloat(float value)
        {
            _salt = NewRange.GenerateRandonRange(-20000, 20000);
            int intValue = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
            this._value = intValue ^ _salt;
        }

        public override bool Equals(object obj) => (float)this == (float)obj;

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => ((float)this).ToString();

        public static implicit operator float(SafeFloat safeFloat) =>
            BitConverter.ToSingle(BitConverter.GetBytes(safeFloat._salt ^ safeFloat._value), 0);

        public static implicit operator SafeFloat(float normalFloat) => new SafeFloat(normalFloat);

    }

    public struct SafeInt
    {
        private readonly int _value;
        private readonly int _salt;

        public SafeInt(int value)
        {
            _salt = _salt = NewRange.GenerateRandonRange(-20000, 20000);
            this._value = value ^ _salt;
        }

        public override bool Equals(object obj) => (int)this == (int)obj;

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => ((int)this).ToString();

        public static implicit operator int(SafeInt safeInt) => safeInt._value ^ safeInt._salt;

        public static implicit operator SafeInt(int normalInt) => new SafeInt(normalInt);

    }

    public static class PlayerPreferences
    {
        private const int _salt = 678309397;

        public static void SetInt(string key, int value)
        {
            int salted = value ^ _salt;
            Task.WaitAll(Serializer.SetIntAsync(StringHash(key), salted), Serializer.SetIntAsync(StringHash("_" + key), IntHash(value)));
        }

        public static int GetInt(string key)
        {
            return GetInt(key, 0);
        }

        public static int GetInt(string key, int defaultValue)
        {
            string hashedKey = StringHash(key);
            if (!Serializer.HasKey(hashedKey)) 
                return defaultValue;

            int salted = Serializer.GetIntAsync(hashedKey).GetAwaiter().GetResult();
            int value = salted ^ _salt;

            int loadedHash = Serializer.GetIntAsync(StringHash("_" + key)).GetAwaiter().GetResult();
            if (loadedHash != IntHash(value)) 
                return defaultValue;

            return value;
        }

        public static void SetFloat(string key, float value)
        {
            int intValue = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);

            int salted = intValue ^ _salt;

            Task.WaitAll(Serializer.SetIntAsync(StringHash(key), salted), Serializer.SetIntAsync(StringHash("_" + key), IntHash(intValue)));
        }

        public static float GetFloat(string key)
        {
            return GetFloat(key, 0);
        }

        public static float GetFloat(string key, float defaultValue)
        {
            string hashedKey = StringHash(key);
            if (!Serializer.HasKey(hashedKey)) 
                return defaultValue;

            int salted = Serializer.GetIntAsync(hashedKey).GetAwaiter().GetResult();
            int value = salted ^ _salt;

            int loadedHash = Serializer.GetIntAsync(StringHash("_" + key)).GetAwaiter().GetResult();
            if (loadedHash != IntHash(value)) 
                return defaultValue;

            return BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
        }

        private static int IntHash(int value)
        {
            value = ((value >> 16) ^ value) * 0x45d9f3b;
            value = ((value >> 16) ^ value) * 0x45d9f3b;
            value = (value >> 16) ^ value;
            return value;
        }

        public static string StringHash(string key)
        {
            HashAlgorithm algorithm = SHA256.Create();
            StringBuilder sb = new StringBuilder();

            var bytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(key));
            foreach (byte b in bytes) 
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static void DeleteKey(string key)
        {
            Serializer.DeleteKey(StringHash(key));
            Serializer.DeleteKey(StringHash("_" + key));
        }

        public static bool HasKey(string key)
        {
            string hashedKey = StringHash(key);
            if (!Serializer.HasKey(hashedKey)) 
                return false;

            int salted = Serializer.GetIntAsync(hashedKey).GetAwaiter().GetResult();
            int value = salted ^ _salt;

            int loadedHash = Serializer.GetIntAsync(StringHash("_" + key)).GetAwaiter().GetResult();

            return loadedHash == IntHash(value);
        }
    }
}