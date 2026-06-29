#if UNITY_2021_3_OR_NEWER
using System.Linq;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

namespace RealtimeCSG
{
    [Overlay(typeof(SceneView), displayName: "Realtime CSG", id: _id, defaultDisplay: true
#if UNITY_2022_3_OR_NEWER
        ,
        defaultLayout = Layout.VerticalToolbar
#endif
    )]
    internal class EditorModeOverlay : ToolbarOverlay
    {
        public const string _id = "RealtimeCSG";

        public EditorModeOverlay()
            : base(
                CSGActivateToggleButton._id,
                PlaceEditorModeButton._id,
                GenerateEditorModeButton._id,
                EditEditorModeButton._id,
                ClipEditorModeButton._id,
                SurfaceEditorModeButton._id
            ) { }
    }

    [EditorToolbarElement(_id, typeof(SceneView))]
    internal class CSGActivateToggleButton : EditorToolbarToggle
    {
        public const string _id = EditorModeOverlay._id + "/CSGActivateToggle";

        public CSGActivateToggleButton()
        {
            tooltip = "Toggle Realtime CSG";

            onIcon = CSGOverlayIconUtility.Load("CSG_Icon.png");
            offIcon = CSGOverlayIconUtility.Load("CSG_Icon_off.png");

            this.RegisterValueChangedCallback(x => OnClicked());
            CSGSettings.OnRealtimeCSGEnabledChanged += OnRealtimeCSGEnabledChanged;

            value = CSGSettings.EnableRealtimeCSG;
        }

        void OnClicked()
        {
            CSGSettings.SetRealtimeCSGEnabled(value);
            OnRealtimeCSGEnabledChanged(value);
        }

        void OnRealtimeCSGEnabledChanged(bool isEnabled)
        {
            value = isEnabled;
            if (parent == null)
                return;

            foreach (EditorModeButton button in parent.Query<EditorModeButton>().ToList())
                button.SetEnabled(isEnabled);
        }
    }

    internal class EditorModeButton : EditorToolbarToggle
    {
        readonly ToolEditMode mode;

        public EditorModeButton(string iconName, ToolEditMode editMode)
        {
            mode = editMode;

            CSG_GUIStyleUtility.InitializeEditModeTexts();
            ToolTip tt = CSG_GUIStyleUtility.brushEditModeTooltips[(int)mode];
            tooltip = $"{tt.TitleString()}\n{tt.ContentsString()}\n{tt.KeyString()}";

            icon = CSGOverlayIconUtility.Load(iconName);

            this.RegisterValueChangedCallback(x => OnClicked());

            value = EditModeManager.EditMode == mode;

            EditModeManager.OnEditModeChanged += OnEditModeChanged;

            SetEnabled(CSGSettings.EnableRealtimeCSG);
        }

        void OnEditModeChanged(ToolEditMode editMode) => SetValueWithoutNotify(editMode == mode);

        void OnClicked()
        {
            if (!value)
                value = true;
            if (value)
                CSGSettings.SetRealtimeCSGEnabled(true);
            EditModeManager.EditMode = mode;
        }
    }

    [EditorToolbarElement(_id, typeof(SceneView))]
    internal class PlaceEditorModeButton : EditorModeButton
    {
        public const string _id = EditorModeOverlay._id + "/Place";

        public PlaceEditorModeButton()
            : base("Place.png", ToolEditMode.Place) { }
    }

    [EditorToolbarElement(_id, typeof(SceneView))]
    internal class GenerateEditorModeButton : EditorModeButton
    {
        public const string _id = EditorModeOverlay._id + "/Generate";

        public GenerateEditorModeButton()
            : base("Generate.png", ToolEditMode.Generate) { }
    }

    [EditorToolbarElement(_id, typeof(SceneView))]
    internal class EditEditorModeButton : EditorModeButton
    {
        public const string _id = EditorModeOverlay._id + "/Edit";

        public EditEditorModeButton()
            : base("Edit.png", ToolEditMode.Edit) { }
    }

    [EditorToolbarElement(_id, typeof(SceneView))]
    internal class ClipEditorModeButton : EditorModeButton
    {
        public const string _id = EditorModeOverlay._id + "/Clip";

        public ClipEditorModeButton()
            : base("Clip.png", ToolEditMode.Clip) { }
    }

    [EditorToolbarElement(_id, typeof(SceneView))]
    internal class SurfaceEditorModeButton : EditorModeButton
    {
        public const string _id = EditorModeOverlay._id + "/Surfaces";

        public SurfaceEditorModeButton()
            : base("Surface.png", ToolEditMode.Surfaces) { }
    }

    [InitializeOnLoad]
    static class CSGEditorModeOverlayBootstrap
    {
        static CSGEditorModeOverlayBootstrap()
        {
            EditorApplication.delayCall += EnsureOverlayOnSceneViews;
        }

        static void EnsureOverlayOnSceneViews()
        {
            for (int i = 0; i < SceneView.sceneViews.Count; i++)
            {
                var sceneView = SceneView.sceneViews[i] as SceneView;
                if (sceneView?.overlayCanvas == null)
                    continue;

                foreach (var overlay in sceneView.overlayCanvas.overlays)
                {
                    if (overlay != null && overlay.id == EditorModeOverlay._id)
                        overlay.displayed = true;
                }
            }

            SceneView.RepaintAll();
        }
    }
}
#endif
