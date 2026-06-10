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
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if PALEXEN_TOOLS
using Palexen.Tools;
#endif

namespace Palexen.Scriptables
{
    [CreateAssetMenu(fileName = "New Entity Manager", menuName = "Palexen/Entity Manager")]
    public class EntityManager : ScriptableObject
    {
        #region VARIABLES

        public List<QuickPrefab> entities = new();

        #endregion

        #region UNITY METHODS


        #endregion

        #region MECHANICS

        

        #endregion

        #region API

        

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(EntityManager))]
    [CanEditMultipleObjects]
    public class EntityManagerEditor : Editor
    {
        SerializedProperty entities;

        private void OnEnable()
        {
            entities = serializedObject.FindProperty("entities");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Entity Library</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Add your entities here, it will appear in the Entities Overlays" +
                "\nTo draw icons, just copy and paste <color=yellow>emojis!</color> 😃",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 90));

            GUILayout.Space(10);

            serializedObject.Update();

            EditorGUILayout.PropertyField(entities, new GUIContent("Entities"), true);

            serializedObject.ApplyModifiedProperties();
        }
    }

    public class CreateNewEntityManager
    {
        public static EntityManager tempAsset;

#if PALEXEN_UP_TOOLBAR
        [MenuItem("Animator Library/Create New Entity Manager")]
#else
        [MenuItem("Palexen/Create New Entity Manager", false, 1)]
#endif
        public static void CreateAsset()
        {
            EntityManager asset = ScriptableObject.CreateInstance<EntityManager>();

            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            string folderPath = setting.scriptablesFolderPath;

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder($"{folderPath}", "Entity Manager");
            }

            string assetPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/New Entity Manager.asset");

            AssetDatabase.CreateAsset(asset, assetPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
            tempAsset = asset;

            Debug.Log($"<color=green>Entity Manager created at: </color><color=cyan>{assetPath}</color>");
        }

        public void CreateNewEntityManagerAsset()
        {
            CreateAsset();
        }

        public EntityManager GetCurrent()
        {
            return tempAsset;
        }
    }

#endif
}
