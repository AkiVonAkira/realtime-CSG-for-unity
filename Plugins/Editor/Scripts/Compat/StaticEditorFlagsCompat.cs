using UnityEditor;

namespace RealtimeCSG
{
    internal static class StaticEditorFlagsCompat
    {
#pragma warning disable 0618
        public static readonly StaticEditorFlags NavigationStatic =
            StaticEditorFlags.NavigationStatic;

        public static readonly StaticEditorFlags OffMeshLinkGeneration =
            StaticEditorFlags.OffMeshLinkGeneration;
#pragma warning restore 0618
    }
}
