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
using Palexen.Tools;
using System.Collections.Generic;
using Palexen.Scriptables;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Palexen.Gameplay.UI
{
    [CreateAssetMenu(fileName = "New Text Translator", menuName = "Palexen/UI/Text Translator")]
    public class LangTextContainer : ScriptableObject
    {
        public List<LangText> _conversions;
    }

#if UNITY_EDITOR

    #region MAIN CUSTOM EDITOR

    [CustomEditor(typeof(LangTextContainer))]
    public class LangTextContainerEditor : Editor
    {
        LangTextContainer _ltc;
        SerializedProperty _conversions;

        private void OnEnable()
        {
            _ltc = (LangTextContainer)target;
            _conversions = serializedObject.FindProperty("_conversions");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Text Translator Container</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));
            GUILayout.Box("Configure all translations; if you don't use them all, you can duplicate your default value, " +
                "or simply leave it blank as long as you don't use more than you set.",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 80));

            serializedObject.Update();

            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);

            EditorGUILayout.PropertyField(_conversions);

            serializedObject.ApplyModifiedProperties();
        }
    }

    public class CreateTextTranslatorContainer
    {
#if PALEXEN_UP_TOOLBAR
        [MenuItem("Text Translator/Create Text Translator")]
#else
        [MenuItem("Palexen/Create Text Translator", false, 5)]
#endif
        static void CreateAsset()
        {
            LangTextContainer asset = ScriptableObject.CreateInstance<LangTextContainer>();

            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            string folderPath = setting.scriptablesFolderPath;

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder($"{folderPath}", "Text Translator");
            }

            string assetPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/New Text Translator.asset");

            AssetDatabase.CreateAsset(asset, assetPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;

            Debug.Log($"<color=green>Text Translator created at: </color><color=cyan>{assetPath}</color>");
        }
    }

    #endregion

#endif
}
