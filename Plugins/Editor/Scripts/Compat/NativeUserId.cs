using UnityEditor;
using UnityEngine;

namespace RealtimeCSG
{
    /// <summary>
    /// Converts between native C++ int user ids and Unity objects. Use only at the native interop boundary.
    /// </summary>
    internal static class NativeUserId
    {
        public static Object ToObject(int nativeUserId)
        {
            if (nativeUserId == 0)
                return null;

            return nativeUserId.ToObject();
        }

        public static int ToNative(Object obj) => obj.ToNativeId();
    }
}
