#if !UNITY_6000_3_OR_NEWER
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RealtimeCSG
{
    public static partial class ObjectIdExtensions
    {
        public static int Id(this Object obj)
        {
            if (IsNull(obj))
                return 0;

            return obj.GetInstanceID();
        }

        public static int ToNativeId(this Object obj) => Id(obj);

#if UNITY_EDITOR
        public static Object ToObject(this int id) => EditorUtility.InstanceIDToObject(id);
#endif
    }
}
#endif
