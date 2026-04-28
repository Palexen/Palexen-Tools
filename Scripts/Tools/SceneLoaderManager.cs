/*
* -----------------------------------------------------------------------------
* Palexen Tools
* © 2023 Palexen | Xeen Render & Devward. All rights reserved.
* https://www.palexen.com/

* -----------------------------------------------------------------------------

* Developed by: Palexen & Xeen Render

* Written by: Devward

* This software is provided "as is," without warranties of any kind.

* Use of this script is subject to the terms of the Palexen Tools and other derivative products license.

* Commercial redistribution or redistribution to third parties without authorization is prohibited.

* -----------------------------------------------------------------------------
*/
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Palexen.Tools
{
    public class SceneLoaderEditor : EditorWindow
    {
        [MenuItem("Palexen/Window/Scene Loader")]
        public static void ShowWindow()
        {
            SceneLoaderEditor window = GetWindow<SceneLoaderEditor>();

            Texture icon = EditorGUIUtility.isProSkin ? AssetDatabase.LoadAssetAtPath<Texture>
                ("Packages/com.palexen.tools/Editor Default Resources/Palexen_window_Icon.png") : 
                AssetDatabase.LoadAssetAtPath<Texture>
                ("Packages/com.palexen.tools/Editor Default Resources/Palexen_window_Icon_2.png");

            window.titleContent = new GUIContent("Scene Loader", icon);

            GetWindow(typeof(SceneLoaderEditor));
        }

        private void OnGUI()
        {
            GUIStyle style = new();
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.cyan;
            style.fontSize = 20;
            style.fontStyle = FontStyle.Bold;

            EditorGUILayout.LabelField("Scenes in Build Settings:", style);

            EditorGUILayout.Space();

            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (GUILayout.Button(scene.path))
                {
                    string scenePath = scene.path;
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                }
            }
        }
    }
}
#endif