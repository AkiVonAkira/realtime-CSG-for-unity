using UnityEngine;

namespace RealtimeCSG
{
    internal static class ObjectFindExtensions
    {
        public static Transform FindAnyTransform()
        {
#if UNITY_2022_2_OR_NEWER
            return Object.FindAnyObjectByType<Transform>();
#else
            return Object.FindObjectOfType<Transform>();
#endif
        }

        public static Transform[] FindAllTransforms()
        {
#if UNITY_6000_0_OR_NEWER
            return Object.FindObjectsByType<Transform>();
#elif UNITY_2022_2_OR_NEWER
#pragma warning disable 0618
            return Object.FindObjectsByType<Transform>(FindObjectsSortMode.None);
#pragma warning restore 0618
#else
            return Object.FindObjectsOfType<Transform>();
#endif
        }
    }
}
