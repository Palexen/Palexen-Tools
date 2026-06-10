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
#if UNITY_EDITOR
#if UNITY_2021_1_OR_NEWER
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace Palexen.Overlays
{
    [EditorToolbarElement(id, typeof(SceneView))]
    class SceneLoaderToolbar : EditorToolbarDropdownToggle, IAccessContainerWindow
    {
        public const string id = "SceneLoaderToolbar/DropdownToggle";

        public EditorWindow containerWindow { get; set; }

        SceneLoaderToolbar()
        {
            text = "Load Scene";
            icon = EditorGUIUtility.isProSkin ? AssetDatabase.LoadAssetAtPath<Texture2D>
                ("Packages/com.palexen.tools/Editor Default Resources/Scenes_Icon.png") :
                AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.palexen.tools/Editor Default Resources/Scenes_Icon_2.png");

            tooltip = "Load or add scenes to Build Settings.";

            dropdownClicked += ShowSceneMenu;
        }

        void ShowSceneMenu()
        {
            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("➕ Add this Scene to the current build settings"), false, AddCurrentSceneToBuild);

            menu.AddSeparator("");

            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

            if (scenes.Length == 0)
            {
                menu.AddDisabledItem(new GUIContent("(No active Scenes in build settings, add one or this scene!)"));
            }
            else
            {
                foreach (var scene in scenes)
                {
                    string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);

                    if (string.IsNullOrEmpty(sceneName)) continue;

                    menu.AddItem(new GUIContent(sceneName), false, () => LoadScene(scene.path));
                }
            }

            menu.ShowAsContext();
        }

        void AddCurrentSceneToBuild()
        {
            var activeScene = EditorSceneManager.GetActiveScene();
            string scenePath = activeScene.path;

            if (string.IsNullOrEmpty(scenePath))
            {
                EditorUtility.DisplayDialog("Error", "Save your scene before you add!", "Ok");
                return;
            }

            List<EditorBuildSettingsScene> buildScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

            bool exists = buildScenes.Exists(s => s.path == scenePath);

            if (!exists)
            {
                buildScenes.Add(new EditorBuildSettingsScene(scenePath, true));
                EditorBuildSettings.scenes = buildScenes.ToArray();

                Debug.Log($"[SceneLoader] Scene added to Build Settings: {activeScene.name}");

                EditorApplication.delayCall += ShowSceneMenu;
            }
            else
            {
                EditorUtility.DisplayDialog("Warning", $"The scene '{activeScene.name}' is already in the Build Settings.", "Ok");
            }
        }

        void LoadScene(string scenePath)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }
    }

    [Overlay(typeof(SceneView), "Scene Loader")]
    public class SceneLoaderOverlay : ToolbarOverlay
    {
        SceneLoaderOverlay() : base(SceneLoaderToolbar.id)
        {
            displayName = "Scene Loader";
            this.collapsedIcon = EditorGUIUtility.isProSkin ? AssetDatabase.LoadAssetAtPath<Texture2D>
                ("Packages/com.palexen.tools/Editor Default Resources/Scenes_Icon.png") :
                AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.palexen.tools/Editor Default Resources/Scenes_Icon_2.png");
        }
    }
}
#endif
#endif
