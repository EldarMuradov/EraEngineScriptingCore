using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EngineLibrary
{
    public static class Debug
    {
        public static void Log(string log) 
        {
            Log_Internal(log);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void Log_Internal(string log);

        public static void LogError(string log)
        {
            LogError_Internal(log);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void LogError_Internal(string log);

        public static void LogWarning(string log)
        {
            LogWarning_Internal(log);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        private extern static void LogWarning_Internal(string log);
    }
}
