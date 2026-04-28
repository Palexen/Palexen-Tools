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
using UnityEngine;
using Palexen.Gameplay;

namespace Palexen.Tools
{
    public class AsyncResourcesLoaderEditor : EditorWindow
    {
        private AsyncResourcesLoader asyncLoader;
        private Vector2 scrollPosition;

        [MenuItem("Palexen/Window/Async Resources Loader Editor")]
        public static void ShowWindow()
        {
            AsyncResourcesLoaderEditor window = GetWindow<AsyncResourcesLoaderEditor>();

            Texture icon = EditorGUIUtility.isProSkin ? AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.palexen.tools/Editor Default Resources/Palexen_window_Icon.png")
                : AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.palexen.tools/Editor Default Resources/Palexen_window_Icon_2.png");
            window.titleContent = new GUIContent("Async Game Manager Window", icon);

            GetWindow(typeof(AsyncResourcesLoaderEditor));
        }

        private void OnEnable()
        {
            asyncLoader = FindAnyObjectByType<AsyncResourcesLoader>();
        }

        private void OnGUI()
        {
            GUIStyle style = new();
            style.normal.textColor = Color.cyan;
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 18;
            style.fontStyle = FontStyle.Bold;

            if (asyncLoader == null)
            {
                EditorGUILayout.LabelField("No se encontró AsyncResourcesLoader en la escena.");
                return;
            }

            EditorGUILayout.LabelField("Gameplay Resources:", style);

            EditorGUILayout.Space();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < asyncLoader.gameplayResources.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                asyncLoader.gameplayResources[i] = (GameObject)EditorGUILayout.ObjectField(asyncLoader.gameplayResources[i], typeof(GameObject), false);
                bool isActive = EditorGUILayout.Toggle(asyncLoader.gameplayResources[i].activeSelf, GUILayout.Width(20));
                if (isActive != asyncLoader.gameplayResources[i].activeSelf)
                {
                    asyncLoader.gameplayResources[i].SetActive(isActive);
                }

                if (GUILayout.Button("Activate"))
                {
                    asyncLoader.gameplayResources[i].SetActive(true);
                }

                if (GUILayout.Button("Deactivate"))
                {
                    asyncLoader.gameplayResources[i].SetActive(false);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
#endif