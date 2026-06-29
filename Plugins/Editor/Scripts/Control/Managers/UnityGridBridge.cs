#if UNITY_6000_3_OR_NEWER
using UnityEditor;
using UnityEngine;

namespace RealtimeCSG
{
    internal static class UnityGridBridge
    {
        struct SnapStateSnapshot
        {
            public Vector3 gridPosition;
            public Quaternion gridRotation;
            public Vector3 gridSize;
            public Vector3 move;
            public float rotate;
            public float scale;
            public bool gridSnapEnabled;
            public bool snapEnabled;
        }

        static bool hasSnapshot;
        static SnapStateSnapshot snapshot;

        internal static void CaptureUnitySnapState()
        {
            snapshot = new SnapStateSnapshot
            {
                gridPosition = EditorSnapSettings.gridPosition,
                gridRotation = EditorSnapSettings.gridRotation,
                gridSize = EditorSnapSettings.gridSize,
                move = EditorSnapSettings.move,
                rotate = EditorSnapSettings.rotate,
                scale = EditorSnapSettings.scale,
                gridSnapEnabled = EditorSnapSettings.gridSnapEnabled,
                snapEnabled = EditorSnapSettings.snapEnabled,
            };
            hasSnapshot = true;
        }

        internal static void RestoreUnitySnapState()
        {
            if (!hasSnapshot)
                return;

            EditorSnapSettings.gridPosition = snapshot.gridPosition;
            EditorSnapSettings.gridRotation = snapshot.gridRotation;
            EditorSnapSettings.gridSize = snapshot.gridSize;
            EditorSnapSettings.move = snapshot.move;
            EditorSnapSettings.rotate = snapshot.rotate;
            EditorSnapSettings.scale = snapshot.scale;
            EditorSnapSettings.gridSnapEnabled = snapshot.gridSnapEnabled;
            EditorSnapSettings.snapEnabled = snapshot.snapEnabled;
            hasSnapshot = false;
        }

        internal static void SyncFromCSG(Camera camera)
        {
            if (!CSGSettings.EnableRealtimeCSG)
                return;

            if (camera)
                CSGGrid.UpdateGridOrientation(camera);

            var snapVector = CSGSettings.SnapVector;
            EditorSnapSettings.gridSize = snapVector;
            EditorSnapSettings.move = snapVector;
            EditorSnapSettings.rotate = CSGSettings.SnapRotation;
            EditorSnapSettings.scale = CSGSettings.SnapScale;

            if (CSGGrid.ForceGrid)
            {
                EditorSnapSettings.gridPosition = CSGGrid.ForcedGridCenter;
                EditorSnapSettings.gridRotation = CSGGrid.ForcedGridRotation;
            }
            else if (camera)
            {
                EditorSnapSettings.gridPosition = CSGGrid.CurrentWorkGridCenter;
                EditorSnapSettings.gridRotation = CSGGrid.CurrentWorkGridRotation;
            }

            UnityGridManager.ShowGrid = CSGSettings.GridVisible;
        }
    }
}
#endif
