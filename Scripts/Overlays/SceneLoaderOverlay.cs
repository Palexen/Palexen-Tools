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

            tooltip = "Load a scene from Build Settings.";

            dropdownClicked += ShowSceneMenu;
        }

        void ShowSceneMenu()
        {
            var menu = new GenericMenu();

            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

            foreach (var scene in scenes)
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);
                menu.AddItem(new GUIContent(sceneName), false, () => LoadScene(scene.path));
            }

            menu.ShowAsContext();
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
        SceneLoaderOverlay() : base(SceneLoaderToolbar.id) { }
    }
}
#endif
#endif