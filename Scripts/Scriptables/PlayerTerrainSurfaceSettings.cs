/*
* -----------------------------------------------------------------------------
* Palexen Tools
* © Palexen | Xeen Render & Devward. All rights reserved.
* https://www.palexen.com/

* -----------------------------------------------------------------------------

* Developed by: Palexen & Xeen Render

* Written by: Devward

* This software is provided "as is," without warranties of any kind.

* Use of this script is subject to the terms of the Palexen Tools and other derivative products license.

* Commercial redistribution or redistribution to third parties without authorization is prohibited.

* -----------------------------------------------------------------------------
*/
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;

#if PALEXEN_TOOLS
using Palexen.Tools;
#endif

namespace Palexen.Scriptables
{
    [CreateAssetMenu(fileName = "New Terrain Surface Settings", menuName = "Palexen/Player/Terrain Footsteps Settings")]
    public class PlayerTerrainSurfaceSettings : ScriptableObject
    {
        public List<TerrainSurface> terrainSurfaceSettings;
    }

    #region MAIN CUSTOM EDITOR
#if UNITY_EDITOR
    [CustomEditor(typeof(PlayerTerrainSurfaceSettings))]
    [CanEditMultipleObjects]
    public class PlayerTerrainSurfaceSettingsEditor : Editor
    {
        private SerializedProperty terrainSurfaceSettings;
        private void OnEnable()
        {
            terrainSurfaceSettings = serializedObject.FindProperty("terrainSurfaceSettings");
        }
        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Terrain Surface Settings</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Edit the terrain surface settings for player footsteps. " +
                "Each entry defines a terrain surface type and its associated footstep sounds." +
                "\nMake sure to use the same order here and in your terrain textures",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 90));

            GUILayout.Space(10);

            serializedObject.Update();
            EditorGUILayout.PropertyField(terrainSurfaceSettings, true);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion
}