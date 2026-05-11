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

using Palexen.Scriptables;
using Palexen.Tools;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Palexen.Audio
{
    [CreateAssetMenu(fileName = "New Audio Library", menuName = "Palexen/Audio/Audio Library")]
    [ScriptDescription("Audio Library", "A sound collection in one place!")]
    public class AudioLibrary : ScriptableObject
    {
        #region VARIABLES

        [MyHeader("Sounds")]
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.errorMessage)] public AudioClip[] sounds;

        #endregion

        #region API

        /// <summary>
        /// Get a random AudioClip from the library.
        /// </summary>
        /// <returns>Get random AudioClip from the library.</returns>
        public AudioClip GetRandomClip()
        {
            if (sounds == null || sounds.Length == 0)
            {
                Debug.LogError("<color=yellow>Audio Library</color> is empty! Please add some audio clips.");
                return null;
            }
            int randomIndex = Random.Range(0, sounds.Length);
            return sounds[randomIndex];
        }

        #endregion
    }

    #region MAIN CUSTOM EDITOR
#if UNITY_EDITOR
    [CustomEditor(typeof(AudioLibrary))]
    [CanEditMultipleObjects]
    public class AudioLibraryEditor : Editor
    {
        private SerializedProperty _sounds;

        private void OnEnable()
        {
            _sounds = serializedObject.FindProperty("sounds");
        }
        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Audio Library</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Add audio clips here, and then you can call them in your scripts, or you can randomly select one from " +
                "here using the <color=yellow>GetRandomClip();</color> method.\r\n\r\nThis is great for sound effects in your game.",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 90));

            GUILayout.Space(10);

            serializedObject.Update();
            EditorGUILayout.PropertyField(_sounds);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion
}
