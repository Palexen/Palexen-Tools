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
#if PALEXEN_TOOLS
using Palexen.Tools;
#endif

namespace Palexen.Scriptables
{
#if PALEXEN_TOOLS
    [ScriptDescription("Languages", "Improved Monobehavior")]
#endif
    [CreateAssetMenu(fileName = "Languages", menuName = "Palexen/Languages")]
    public class Languages : ScriptableObject
    {
        #region VARIABLES

        [SerializeField] private string[] languages;

        #endregion

        #region PROPERTIES

        public string[] LanguagesList
        {
            get { return languages; }
            set { languages = value; }
        }

        #endregion

        #region UNITY METHODS



        #endregion

        #region MECHANICS



        #endregion

        #region API



        #endregion
    }

    #region CUSTOM EDITOR
#if UNITY_EDITOR

    [CustomEditor(typeof(Languages))]
    [CanEditMultipleObjects]
    public class LanguagesEditor : Editor
    {
        SerializedProperty languages;

        private void OnEnable()
        {
            languages = serializedObject.FindProperty("languages");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.ScriptTitleColor.ConvertToHex()}>Languages</color>",
                PalexenEditorStyles.CoolTitle(setting.ScriptTitleSize));

            GUILayout.Box("Add your Languages here, it will appear in the <color=green>LangManager</color>",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 90));

            GUILayout.Space(10);

            serializedObject.Update();

            EditorGUILayout.PropertyField(languages, new GUIContent("Languages"), true);

            serializedObject.ApplyModifiedProperties();
        }
    }

    public class CreateNewLanguageAsset
    {
        public static Languages tempAsset;

#if PALEXEN_UP_TOOLBAR
        [MenuItem("Languages/Create New Language")]
#else
        [MenuItem("Palexen/Create New Language", false, 1)]
#endif
        public static void CreateAsset()
        {
            Languages asset = ScriptableObject.CreateInstance<Languages>();

            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            string folderPath = setting.ScriptablesFolderPath;

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder($"{folderPath}", "Languages");
            }

            string assetPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/New Language.asset");

            AssetDatabase.CreateAsset(asset, assetPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
            tempAsset = asset;

            Debug.Log($"<color=green>Language created at: </color><color=cyan>{assetPath}</color>");
        }

        public void CreateNewLanguagesAsset()
        {
            CreateAsset();
        }

        public Languages GetCurrent()
        {
            return tempAsset;
        }
    }

#endif
    #endregion
}
