using UnityEditor;
using UnityEngine;

namespace RealtimeCSG
{
    internal static class SelectionIdExtensions
    {
#if UNITY_6000_3_OR_NEWER
        public static EntityId[] GetEntityIds() => Selection.entityIds;

        public static void SetEntityIds(EntityId[] ids) => Selection.entityIds = ids;

        public static void SetEntityIdsFromNative(int nativeId)
        {
#pragma warning disable 0618
            Selection.entityIds = new[] { (EntityId)nativeId };
#pragma warning restore 0618
        }
#else
        public static int[] GetEntityIds() => Selection.instanceIDs;

        public static void SetEntityIds(int[] ids) => Selection.instanceIDs = ids;
#endif
    }
}
