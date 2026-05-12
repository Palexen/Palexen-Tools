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

    public class CreatePlayerTerrainSurfaceSettingsAsset
    {
#if PALEXEN_UP_TOOLBAR
        [MenuItem("Player Terrain Surface Settings/Create Player Terrain Surface Settings")]
#else
        [MenuItem("Palexen/Create Player Terrain Surface Settings", false, 4)]
#endif
        private static void CreateAsset()
        {
            PlayerTerrainSurfaceSettings asset = ScriptableObject.CreateInstance<PlayerTerrainSurfaceSettings>();

            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            string folderPath = setting.scriptablesFolderPath;

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder($"{folderPath}", "Player Terrain Surface Settings");
            }

            string assetPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/New Player Terrain Surface Settings.asset");

            AssetDatabase.CreateAsset(asset, assetPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;

            Debug.Log($"<color=green>Player Terrain Surface Settings created at: </color><color=cyan>{assetPath}</color>");
        }
    }

#endif
    #endregion
}