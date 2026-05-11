/*
* -----------------------------------------------------------------------------
* Palexen Tools
* © | Xeen Render & Devward. All rights reserved.
* https://www.palexen.com/

* -----------------------------------------------------------------------------

* Developed by: Palexen & Xeen Render

* Written by: Devward

* This software is provided "as is," without warranties of any kind.

* Use of this script is subject to the terms of the Palexen Tools and other derivative products license.

* Commercial redistribution or redistribution to third parties without authorization is prohibited.

* -----------------------------------------------------------------------------
*/
using Palexen.Tools;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Palexen.Scriptables
{
    [ScriptDescription("Animator Container", "Scriptable")]
    [CreateAssetMenu(fileName = "New Animator Container", menuName = "Palexen/Animation/Animator Library")]
    public class AnimatorLibrary : ScriptableObject
    {
        #region VARIABLES

        [MyHeader("Animator Library")]
        public string _genderOrType = "Gender, Type, Class";
        [FieldColor(FieldPropertyColor.cyan, ShowObjectMessage.errorMessage)] public RuntimeAnimatorController[] _animators;

        #endregion

        #region API

        /// <summary>
        /// Get a random RuntimeAnimatorController from the library.
        /// </summary>
        /// <returns>Get random RuntimeAnimatorController from the library.</returns>
        public RuntimeAnimatorController GetRandomAnimator()
        {
            int i = Random.Range(0, _animators.Length);
            return _animators[i];
        }

        #endregion
    }

    #region MAIN CUSTOM EDITOR
#if UNITY_EDITOR
    [CustomEditor(typeof(AnimatorLibrary))]
    [CanEditMultipleObjects]
    public class AnimatorLibraryEditor : Editor
    {
        private SerializedProperty _genderOrType;
        private SerializedProperty _animators;

        private void OnEnable()
        {
            _genderOrType = serializedObject.FindProperty("_genderOrType");
            _animators = serializedObject.FindProperty("_animators");
        }
        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Animator Library</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Add your animators here, and call them in your scripts, or choose a random one from here using " +
                "the <color=cyan>GetRandomAnimator();</color> method.\r\n\r\nThis is great when you need many NPCs that share the same structure but have different animations.",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 90));

            GUILayout.Space(10);

            serializedObject.Update();
            EditorGUILayout.PropertyField(_genderOrType);
            EditorGUILayout.PropertyField(_animators);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion
}