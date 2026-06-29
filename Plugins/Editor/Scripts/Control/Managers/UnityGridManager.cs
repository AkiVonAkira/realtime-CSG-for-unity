using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RealtimeCSG
{
    internal sealed class UnityGridManager
    {
#if UNITY_6000_3_OR_NEWER
        static bool initialized = false;
        static readonly HashSet<SceneView> subscribedSceneViews = new HashSet<SceneView>();

        static void EnsureInitialized()
        {
            if (initialized)
                return;
            initialized = true;
            EditorApplication.update += TrackSceneViewGridSubscriptions;
            TrackSceneViewGridSubscriptions();
        }

        static void TrackSceneViewGridSubscriptions()
        {
            for (int i = 0; i < SceneView.sceneViews.Count; i++)
            {
                var sceneView = SceneView.sceneViews[i] as SceneView;
                if (sceneView == null || subscribedSceneViews.Contains(sceneView))
                    continue;

                sceneView.gridVisibilityChanged += OnGridVisibilityChanged;
                subscribedSceneViews.Add(sceneView);
            }
        }

        static void OnGridVisibilityChanged(bool visible)
        {
            if (!CSGSettings.EnableRealtimeCSG)
                return;
            CSGSettings.GridVisible = visible;
        }

        internal static bool ShowGrid
        {
            get
            {
                EnsureInitialized();
                var sceneView = SceneView.lastActiveSceneView;
                if (sceneView)
                    return sceneView.showGrid;
                return CSGSettings.GridVisible;
            }
            set
            {
                EnsureInitialized();
                for (int i = 0; i < SceneView.sceneViews.Count; i++)
                {
                    var sceneView = SceneView.sceneViews[i] as SceneView;
                    if (sceneView)
                        sceneView.showGrid = value;
                }
            }
        }
#else
        static Type UnityAnnotationUtility;
        static PropertyInfo UnityShowGridProperty;

        static bool initialized = false;
        static bool reflectionSucceeded = false;

        static void InitReflectedData()
        {
            if (initialized)
                return;
            initialized = true;
            reflectionSucceeded = false;

            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            var types = new List<System.Type>();
            foreach (var assembly in assemblies)
            {
                try
                {
                    types.AddRange(assembly.GetTypes());
                }
                catch { }
            }
            UnityAnnotationUtility = types.FirstOrDefault(t =>
                t.FullName == "UnityEditor.AnnotationUtility"
            );

            if (UnityAnnotationUtility != null)
            {
                UnityShowGridProperty = UnityAnnotationUtility.GetProperty(
                    "showGrid",
                    BindingFlags.NonPublic | BindingFlags.Static
                );
                if (UnityShowGridProperty != null)
                    UnityShowGridProperty.SetValue(UnityAnnotationUtility, false, null);
            }
            else
            {
                UnityShowGridProperty = null;
            }

            reflectionSucceeded = UnityShowGridProperty != null;
        }

        internal static bool ShowGrid
        {
            get
            {
                InitReflectedData();
                if (reflectionSucceeded && UnityShowGridProperty != null)
                    return (bool)UnityShowGridProperty.GetValue(UnityAnnotationUtility, null);
                return true;
            }
            set
            {
                InitReflectedData();
                if (reflectionSucceeded && UnityShowGridProperty != null)
                    UnityShowGridProperty.SetValue(UnityAnnotationUtility, value, null);
            }
        }
#endif
    }
}
