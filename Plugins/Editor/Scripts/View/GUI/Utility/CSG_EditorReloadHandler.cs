using UnityEditor;

namespace RealtimeCSG
{
	[InitializeOnLoad]
	static class CSG_EditorReloadHandler
	{
		static CSG_EditorReloadHandler()
		{
			AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
		}

		static void OnBeforeAssemblyReload()
		{
			CSG_GUIStyleUtility.ResetCachedStyles();
			EditModeSelectionGUI.ResetCachedGuiStyles();
			SceneViewBottomBarGUI.ResetCachedGuiStyles();
			SceneViewInfoGUI.ResetCachedGuiStyles();
			CSGModelComponentInspectorGUI.ResetCachedGuiStyles();
		}
	}
}
