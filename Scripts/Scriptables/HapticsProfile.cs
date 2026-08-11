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
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if PALEXEN_TOOLS
using Palexen.Tools;
#endif

namespace Palexen.Scriptables
{
    #if PALEXEN_TOOLS
    [ScriptDescription("HapticsProfile", "Improved Monobehavior")]
#endif
    [CreateAssetMenu(fileName = "New Haptics Profile", menuName = "Palexen/Haptics Profile")]
    public class HapticsProfile : ScriptableObject
    {
        #region VARIABLES

        [MyHeader("Left")]
        [SerializeField] private AnimationCurve _leftPattern;
        [SerializeField] private float _leftSpeed = 1;

        [MyHeader("Right")]
        [SerializeField] private AnimationCurve _rightPattern;
        [SerializeField] private float _rightSpeed = 1;

        [MyHeader("Miscellaneous")]
        [SerializeField] private float _timer;
        float _progressLeft;
        float _progressRight;

        #endregion

        #region PROPERTIES

        public AnimationCurve LeftPattern { get { return _leftPattern; } }
        public float LeftSpeed { get { return _leftSpeed; } }

        public AnimationCurve RightPattern { get { return _rightPattern; } }
        public float RightSpeed { get { return _rightSpeed; } }

        public float Timer { get { return _timer; } }

        public float ProgressLeft { get { return _progressLeft; } }
        public float ProgressRight { get { return _progressRight; } }

        #endregion

        #region UNITY METHODS

        private void OnDisable()
        {
            Gamepad.current?.SetMotorSpeeds(0, 0);
        }

        private void OnDestroy()
        {
            Gamepad.current?.SetMotorSpeeds(0, 0);
        }

        #endregion

        #region MECHANICS



        #endregion

        #region API

        public void PlayPattern(float leftTime, float rightTime, float input)
        {
            float leftLoop = leftTime % 1.0f;
            float leftCurve = _leftPattern.Evaluate(leftLoop);
            float left = leftCurve * input;
            _progressLeft = left;

            float rightLoop = rightTime % 1.0f;
            float rightCurve = _rightPattern.Evaluate(rightLoop);
            float right = rightCurve * input;
            _progressRight = right;

            Rumble(left, right);
        }

        public void Rumble(float left, float right, float frontLeft = 0, float frontRight = 0)
        {
            Gamepad.current?.SetMotorSpeeds(left, right);
        }

        public void StopPattern(float lowFrecuency = 0)
        {
            Gamepad.current?.SetMotorSpeeds(lowFrecuency, lowFrecuency);
        }

        #endregion
    }

#if UNITY_EDITOR

    #region MAIN CUSTOM EDITOR

    [CustomEditor(typeof(HapticsProfile))]
    public class HapticsProfileEditor : Editor
    {
        HapticsProfile _hp;
        SerializedProperty _leftPattern;
        SerializedProperty _leftSpeed;

        SerializedProperty _rightPattern;
        SerializedProperty _rightSpeed;

        SerializedProperty _timer;

        private void OnEnable()
        {
            _hp = (HapticsProfile)target;
            _leftPattern = serializedObject.FindProperty("_leftPattern");
            _leftSpeed = serializedObject.FindProperty("_leftSpeed");

            _rightPattern = serializedObject.FindProperty("_rightPattern");
            _rightSpeed = serializedObject.FindProperty("_rightSpeed");

            _timer = serializedObject.FindProperty("_timer");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.ScriptTitleColor.ConvertToHex()}>Haptics</color>",
                PalexenEditorStyles.CoolTitle(setting.ScriptTitleSize));
            GUILayout.Box("Draw patterns for the motors of the connected controllers to follow!" +
                "\r\nNote: Not compatible with devices that do not support XInput (e.g., Android, Meta Quest). ",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 80));

            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);

            serializedObject.Update();

            EditorGUILayout.PropertyField(_leftPattern);
            EditorGUILayout.PropertyField(_leftSpeed);

            EditorGUILayout.PropertyField(_rightPattern);
            EditorGUILayout.PropertyField(_rightSpeed);

            EditorGUILayout.PropertyField(_timer);

            serializedObject.ApplyModifiedProperties();
        }
    }

    public class CreateTextTranslatorContainer
    {
#if PALEXEN_UP_TOOLBAR
        [MenuItem("Haptics/Create Haptics Profile")]
#else
        [MenuItem("Palexen/Create Haptics Profile", false, 5)]
#endif
        static void CreateAsset()
        {
            HapticsProfile asset = ScriptableObject.CreateInstance<HapticsProfile>();

            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            string folderPath = setting.ScriptablesFolderPath;

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder($"{folderPath}", "Haptics Profile");
            }

            string assetPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/New Haptics Profile.asset");

            AssetDatabase.CreateAsset(asset, assetPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;

            Debug.Log($"<color=green>Haptics Profile created at: </color><color=cyan>{assetPath}</color>");
        }
    }

    #endregion

#endif
}
