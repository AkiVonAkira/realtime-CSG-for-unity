#if UNITY_6000_3_OR_NEWER
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RealtimeCSG
{
    public static partial class ObjectIdExtensions
    {
        public static EntityId Id(this Object obj)
        {
            if (IsNull(obj))
                return EntityId.None;

            return obj.GetEntityId();
        }

        public static int ToNativeId(this Object obj)
        {
            if (IsNull(obj))
                return 0;

#pragma warning disable 0618
            return (int)obj.GetEntityId();
#pragma warning restore 0618
        }

#if UNITY_EDITOR
        public static Object ToObject(this EntityId id) => EditorUtility.EntityIdToObject(id);

        public static Object ToObject(this int nativeId)
        {
            if (nativeId == 0)
                return null;

#pragma warning disable 0618
            return EditorUtility.EntityIdToObject(nativeId);
#pragma warning restore 0618
        }
#endif
    }
}
#endif
