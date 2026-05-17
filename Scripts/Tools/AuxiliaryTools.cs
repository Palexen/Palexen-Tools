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
using System;
using UnityEngine;
using System.Linq;
using Palexen.Misc;
using System.Reflection;
using Palexen.Scriptables;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
#endif

namespace Palexen.Tools
{
    #region ENUMS
    public enum FieldPropertyColor { red, green, blue, yellow, cyan, magenta, orange, clearBlue, pink, neonGreen, salmon }
    public enum ShowObjectMessage { no, message, warningMessage, errorMessage }
    public enum GizmoForm { sphere, cube, cylinder, cone, arrow, circle, square, dot }
    public enum TurnOnScriptDescription { On, Off }

    #endregion

    #region CUSTOM PROPERTY FIELD COLOR
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(FieldColor))]
    [CanEditMultipleObjects]
    public class VariableColor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            FieldColor field = attribute as FieldColor;

            float height = EditorGUI.GetPropertyHeight(property, label, true);

            if (property.objectReferenceValue == null && field.toShow != ShowObjectMessage.no)
            {
                height += EditorGUIUtility.singleLineHeight * 2f;
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            FieldColor field = attribute as FieldColor;

            CustomEnvironment msj = Resources.Load<CustomEnvironment>("Environment Settings/Palexen Environment Settings");

            Rect fieldRect = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(property, label));

            if (!field.keepColor)
            {
                if (property.objectReferenceValue == null)
                {
                    GUI.color = field.color;
                    EditorGUI.PropertyField(fieldRect, property, label, true);
                    GUI.color = Color.white;

                    if (field.toShow != ShowObjectMessage.no && msj != null)
                    {
                        Rect helpBoxRect = new Rect(
                            position.x,
                            fieldRect.y + fieldRect.height + 2,
                            position.width,
                            EditorGUIUtility.singleLineHeight * 2f
                        );

                        MessageType type = MessageType.Info;

                        if (field.toShow == ShowObjectMessage.warningMessage)
                            type = MessageType.Warning;
                        else if (field.toShow == ShowObjectMessage.errorMessage)
                            type = MessageType.Error;

                        string message = field.toShow == ShowObjectMessage.message ? msj.infoString :
                                         field.toShow == ShowObjectMessage.warningMessage ? msj.warningString :
                                         msj.errorString;

                        EditorGUI.HelpBox(helpBoxRect, message, type);
                    }
                }
                else
                {
                    EditorGUI.PropertyField(fieldRect, property, label, true);
                }
            }
            else
            {
                GUI.color = field.color;
                EditorGUI.PropertyField(fieldRect, property, label, true);
                GUI.color = Color.white;

                if (property.objectReferenceValue == null) 
                {
                    if (field.toShow != ShowObjectMessage.no && msj != null)
                    {
                        Rect helpBoxRect = new Rect(
                            position.x,
                            fieldRect.y + fieldRect.height + 2,
                            position.width,
                            EditorGUIUtility.singleLineHeight * 2f
                        );

                        MessageType type = MessageType.Info;

                        if (field.toShow == ShowObjectMessage.warningMessage)
                            type = MessageType.Warning;
                        else if (field.toShow == ShowObjectMessage.errorMessage)
                            type = MessageType.Error;

                        string message = field.toShow == ShowObjectMessage.message ? msj.infoString :
                                         field.toShow == ShowObjectMessage.warningMessage ? msj.warningString :
                                         msj.errorString;
                        EditorGUI.HelpBox(helpBoxRect, message, type);
                    }
                    else
                    {
                        EditorGUI.PropertyField(fieldRect, property, label, true);
                    }
                }
            }
        }
    }
#endif

    /// <summary>
    /// This attribute allows you to change the color of a field in the inspector, and you can 
    /// also choose to show a message below the field when it's empty,
    /// </summary>
    public class FieldColor : PropertyAttribute
    {
        public Color color;
        public ShowObjectMessage toShow;
        public bool keepColor;

        public FieldColor(FieldPropertyColor _color = FieldPropertyColor.red, ShowObjectMessage _message = ShowObjectMessage.no, bool _keepColor = false)
        {
            keepColor = _keepColor;

            switch (_color)
            {
                case FieldPropertyColor.red:
                    toShow = _message;
                    color = Color.red;
                    break;

                case FieldPropertyColor.green:
                    toShow = _message;
                    color = Color.green;
                    break;

                case FieldPropertyColor.blue:
                    toShow = _message;
                    color = new Color(.392f, .584f, .929f, 1);
                    break;

                case FieldPropertyColor.yellow:
                    toShow = _message;
                    color = Color.yellow;
                    break;

                case FieldPropertyColor.cyan:
                    toShow = _message;
                    color = Color.cyan;
                    break;

                case FieldPropertyColor.magenta:
                    toShow = _message;
                    color = Color.magenta;
                    break;

                case FieldPropertyColor.orange:
                    toShow = _message;
                    color = new Color(1, 0.388f, 0);
                    break;

                case FieldPropertyColor.clearBlue:
                    toShow = _message;
                    color = new Color(0, 0.671f, 1);
                    break;

                case FieldPropertyColor.pink:
                    toShow = _message;
                    color = new Color(1, 0.561f, 0.988f);
                    break;

                case FieldPropertyColor.neonGreen:
                    toShow = _message;
                    color = new Color(0.392f, 1, 0);
                    break;

                case FieldPropertyColor.salmon:
                    toShow = _message;
                    color = new Color(1, 0.549f, 0.412f);
                    break;
            }
        }
    }
    #endregion

    #region CREATE ASSETS

#if UNITY_EDITOR
    public class PalexenAssetsCreator : Editor
    {
        //[MenuItem("Palexen/Create/Palexen Environment Settings Asset")]
        public static void CreateCustomMessageSettings()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";

            CustomEnvironment msj = Resources.Load<CustomEnvironment>(customMessagePath);

            if (msj == null)
            {
                CustomEnvironment cms = CreateInstance<CustomEnvironment>();
                AssetDatabase.CreateAsset(cms, customMessagePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log("Custom Environment Settings are created successfully!");
            }
            else
            {
                Debug.Log("Custom Environment Settgins already exist!");
            }
        }
    }
#endif

    #endregion

    #region PANEL EDITOR WINDOW SETUP

#if UNITY_EDITOR
    public class EnvironmentSettingsWindow : EditorWindow
    {
        private CustomEnvironment environmentAsset;
        private Vector2 scrollPosition = Vector2.zero;

#if PALEXEN_UP_TOOLBAR
        [MenuItem("Environment Configuration/Show Environment Window")]
#else
        [MenuItem("Palexen/Window/Environment Configuration")]
#endif
        public static void ShowWindow()
        {
            EnvironmentSettingsWindow window = GetWindow<EnvironmentSettingsWindow>();

            Texture icon = EditorGUIUtility.isProSkin ? AssetDatabase.LoadAssetAtPath<Texture2D>
                ("Packages/com.palexen.tools/Editor Default Resources/Palexen_window_Icon.png") : 
                AssetDatabase.LoadAssetAtPath<Texture2D>
                ("Packages/com.palexen.tools/Editor Default Resources/Palexen_window_Icon_2.png");

            window.titleContent = new GUIContent("Environment Configuration", icon);

            window.Show();

            CustomEnvironment data = Resources.Load<CustomEnvironment>("MyData");
            if (data != null)
                window.environmentAsset = data;
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();

            environmentAsset = Resources.Load<CustomEnvironment>("Environment Settings/Palexen Environment Settings");

            EditorGUILayout.Space();

            if (environmentAsset != null)
            {
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                EditorGUILayout.LabelField($"<color={"#" + environmentAsset.scriptTitleColor.ConvertToHex()}>Current Environment</color>", 
                    PalexenEditorStyles.CoolTitle(25, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));
                DisplayScriptableObjectInfo(environmentAsset);
                EditorGUILayout.Space(20);

                EditorGUILayout.LabelField($"<color={"#" + environmentAsset.scriptTitleColor.ConvertToHex()}>Toolbar Settings</color>",
                    PalexenEditorStyles.CoolTitle(25, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));
                EditorGUILayout.Space(20);

                if (GUILayout.Button("Select path to save scriptables", PalexenEditorStyles.BigButton))
                {
                    string selectedPath = EditorUtility.OpenFolderPanel("Select Path", "", "");

                    if (!string.IsNullOrEmpty(selectedPath))
                    {
                        string projectPath = Application.dataPath.Replace("Assets", "");

                        if (selectedPath.StartsWith(projectPath))
                        {
                            string relativePath = selectedPath.Substring(projectPath.Length);

                            environmentAsset.SetPath(relativePath);

                            EditorUtility.SetDirty(environmentAsset);
                            AssetDatabase.SaveAssets();
                        }
                        else
                        {
                            Debug.LogError("Selected folder must be inside this Unity project.");
                        }
                    }
                }

                EditorGUILayout.EndScrollView();
            }
        }


        private void DisplayScriptableObjectInfo(CustomEnvironment data)
        {
            if (data == null) return;

            SerializedObject serializedObject = new(data);
            serializedObject.Update();
            SerializedProperty property = serializedObject.GetIterator();
            bool enterChildren = true;

            while (property.NextVisible(enterChildren))
            {
                enterChildren = false;

                if (property.name == "m_Script") continue;

                EditorGUILayout.PropertyField(property, true);
            }

            serializedObject.ApplyModifiedProperties();
        }

    }

    [CustomEditor(typeof(CustomEnvironment))]
    public class EnvironmentDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            CustomEnvironment environmentAsset = (CustomEnvironment)target;

            EditorGUILayout.LabelField("Script Name", environmentAsset.GetType().Name);
            EditorGUILayout.LabelField("Namespace", environmentAsset.scriptNameSpace);
            EditorGUILayout.Space();

            DisplayScriptableObjectInfo(environmentAsset);
        }

        private void DisplayScriptableObjectInfo(CustomEnvironment data)
        {
            SerializedObject serializedObject = new(data);
            SerializedProperty property = serializedObject.GetIterator();

            while (property.NextVisible(true))
            {
                EditorGUILayout.PropertyField(property, true);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

    #endregion

    #region EDITOR CUSTOMIZATION

#if UNITY_EDITOR

    public class ToolbarCustomization
    {
        private const string DEFINE = "PALEXEN_UP_TOOLBAR";

        static bool isExpanded = false;

        [MenuItem("Palexen/Use Full Toolbar")]
        public static void CustomizeToolbarToggle()
        {
            NamedBuildTarget target = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            string defines = PlayerSettings.GetScriptingDefineSymbols(target);

            var defineList = defines
                .Split(';')
                .Where(d => !string.IsNullOrWhiteSpace(d))
                .ToList();

            bool hasDefine = defineList.Contains(DEFINE);

            if (!hasDefine)
            {
                defineList.Add(DEFINE);

                EditorUtility.DisplayProgressBar("Applying Toolbar Settings", "Expanding toolbar...", 1f);

                Debug.Log("Toolbar Expanded");
                EditorUtility.ClearProgressBar();
            }
            else
            {
                defineList.Remove(DEFINE);

                EditorUtility.DisplayProgressBar("Applying Toolbar Settings", "Collapsing toolbar...", 1f);

                Debug.Log("Toolbar Collapsed");
                EditorUtility.ClearProgressBar();
            }

            string result = string.Join(";", defineList);

            PlayerSettings.SetScriptingDefineSymbols(target, result);

            isExpanded = !hasDefine;
        }

        [MenuItem("Palexen/Use Full Toolbar", true)]
        static bool ToggleToolBar()
        {
            NamedBuildTarget target = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            string defines = PlayerSettings.GetScriptingDefineSymbols(target);
            isExpanded = defines.Contains(DEFINE);

            Menu.SetChecked("Palexen/Use Full Toolbar", isExpanded);

            return true;
        }
    }

#endif

#endregion

    #region CUSTOM SCRIPT

    #region Example Script
#if UNITY_EDITOR
        [CustomEditor(typeof(ExampleScript))]
    public class Example : Editor
    {
        GUIStyle buttonRegionStyle;
        GUIStyle messageRegionStyle;

        public override void OnInspectorGUI()
        {
            buttonRegionStyle = new(EditorStyles.helpBox);
            buttonRegionStyle.alignment = TextAnchor.MiddleCenter;
            buttonRegionStyle.normal.textColor = Color.cyan;
            buttonRegionStyle.fontStyle = FontStyle.Bold;
            buttonRegionStyle.fontSize = 15;

            messageRegionStyle = new(EditorStyles.helpBox);
            messageRegionStyle.alignment = TextAnchor.MiddleCenter;
            messageRegionStyle.normal.textColor = Color.white;
            messageRegionStyle.fontStyle = FontStyle.Bold;
            messageRegionStyle.fontSize = 13;

            ExampleScript sample = (ExampleScript)target;
            GUILayout.Box("Each color represents an empty area, which you can use to remind yourself if the object is important or not, " +
                "Use it however you like! :)", messageRegionStyle);

            EditorGUILayout.HelpBox("Warning: these functions should only be used when it comes to referencing objects like " +
                "Scene GameObjects, Prefabs, Scripts or Components | don't work with editable values ​​like " +
                "Strings, Floats, Ints, or bools, can generate errors", MessageType.Warning);

            DrawDefaultInspector();

            EditorGUILayout.Space();
            GUILayout.Box("Search more in the Official Online Page", buttonRegionStyle);

            if (EditorGUIUtility.isProSkin)
            {
                if (GUILayout.Button("Go to <color=cyan>Online Page</color>", PalexenEditorStyles.BigButton))
                {
                    sample.GoToOnlineManual();
                }
            }
            else
            {
                if (GUILayout.Button("Go to <color=green>Online Page</color>", PalexenEditorStyles.BigButton))
                {
                    sample.GoToOnlineManual();
                }
            }
        }
    }
#endif
    #endregion

    #region SCRIPT TITLES
#if UNITY_EDITOR

    /// <summary>
    /// This custom editor is responsible for displaying the script title and description in the 
    /// inspector for any MonoBehaviour script that has the ScriptDescription attribute,
    /// </summary>
    [CustomEditor(typeof(MonoBehaviour), editorForChildClasses: true)]
    [CanEditMultipleObjects]
    public class MyRoot : Editor
    {
        ScriptDescription attribs = null;
        bool isPalexenScript = false;

        private void OnEnable()
        {
            string targetNamespace = target.GetType().Namespace;

            string customMessagePath = "Environment Settings/Palexen Environment Settings";

            CustomEnvironment msj = Resources.Load<CustomEnvironment>(customMessagePath);

            if (!string.IsNullOrEmpty(targetNamespace))
            {
                if (msj != null)
                {
                    isPalexenScript = targetNamespace.StartsWith(msj.scriptNameSpace);
                }
            }

            if (attribs == null)
                attribs = GetMyAttribs(target);
        }

        public override void OnInspectorGUI()
        {
            if (isPalexenScript)
                HeaderGUI(attribs);

            base.OnInspectorGUI();
        }

        public static void HeaderGUI(ScriptDescription attrib)
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";

            CustomEnvironment msj = Resources.Load<CustomEnvironment>(customMessagePath);

            if (msj != null)
            {
                if (msj.scriptDescriptionState == TurnOnScriptDescription.On)
                {
                    if (attrib != null)
                    {
                        GUILayout.Space(10);
                        GUILayout.BeginVertical();
                        GUILayout.FlexibleSpace();

                        GUIStyle guiForName = new GUIStyle(GUI.skin.label);
                        guiForName.fontStyle = FontStyle.Bold;
                        guiForName.alignment = TextAnchor.UpperCenter;
                        guiForName.richText = true;
                        GUILayout.Label($"<color={"#" + msj.scriptTitleColor.ConvertToHex()}><size={msj.scriptTitleSize}>{attrib.Name}</size></color>", guiForName);

                        GUILayout.Space(5);
                        GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        GUILayout.Box(attrib.Description, GUILayout.Width(Screen.width * .8f));
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();

                        GUILayout.FlexibleSpace();
                        GUILayout.EndVertical();
                        GUILayout.Space(20);
                    }
                }
            }
        }

        public static ScriptDescription GetMyAttribs(UnityEngine.Object obj)
        {
            return obj.GetType().GetCustomAttribute<ScriptDescription>() ?? new ScriptDescription(obj.GetType().Name);
        }
    }
#endif
    #endregion

    #endregion

    #region CUSTOM ATTRIBS

    #region Script Description Component

    /// <summary>
    /// This attribute allows you to add a title and description to your scripts, which will be displayed 
    /// in the inspector when you select a GameObject with that script attached.
    /// </summary>
    public class ScriptDescription : Attribute
    {
        public string Name { get; private set; } = null;
        public string Description { get; private set; } = null;

        public ScriptDescription(string name)
        {
            Name = name;
        }

        public ScriptDescription(string name, string description)
            : this(name)
        {
            Description = description;
        }
    }
    #endregion

    #region My Header Attrib

    /// <summary>
    /// This attribute allows you to create a custom header in the inspector with a custom height, 
    /// and you can also customize the color and size of the header text from the Custom 
    /// Environment Settings asset.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public class MyHeaderAttribute : PropertyAttribute
    {
        public readonly string header;
        public float h;

        public MyHeaderAttribute(string header, float height = 2)
        {
            this.header = header;
            this.h = height;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(MyHeaderAttribute), true)]
    [CanEditMultipleObjects]
    public class HeaderDecoratorDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            MyHeaderAttribute myAttribute = (MyHeaderAttribute)attribute;

            return EditorGUIUtility.singleLineHeight * myAttribute.h;
        }

        public override void OnGUI(Rect position)
        {
            MyHeaderAttribute myAttribute = (MyHeaderAttribute)attribute;

            string customMessagePath = "Environment Settings/Palexen Environment Settings";

            CustomEnvironment msj = Resources.Load<CustomEnvironment>(customMessagePath);

            position.yMin += EditorGUIUtility.singleLineHeight * .5f;

            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.richText = true;

            if (msj != null)
            {
                GUIContent label = new GUIContent($"<color={"#" + msj.headerColorValue.ConvertToHex()}><size={msj.headerSize}>{(attribute as MyHeaderAttribute)?.header}</size></color>");
                GUI.Label(position, label, style);
            }
        }
    }
#endif
#endregion

    #region Custom Vector2 Slider

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(VectorSliderAttribute))]
    public class MinMaxSliderDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            var minMaxAttribute = (VectorSliderAttribute)attribute;
            var propertyType = property.propertyType;

            label.tooltip = minMaxAttribute.min.ToString("F2") + " to " + minMaxAttribute.max.ToString("F2");

            Rect controlRect = EditorGUI.PrefixLabel(position, label);

            Rect[] splittedRect = SplitRect(controlRect, 3);

            if (propertyType == SerializedPropertyType.Vector2)
            {

                EditorGUI.BeginChangeCheck();

                Vector2 vector = property.vector2Value;
                float minVal = vector.x;
                float maxVal = vector.y;

                minVal = EditorGUI.FloatField(splittedRect[0], float.Parse(minVal.ToString("F2")));
                maxVal = EditorGUI.FloatField(splittedRect[2], float.Parse(maxVal.ToString("F2")));

                EditorGUI.MinMaxSlider(splittedRect[1], ref minVal, ref maxVal,
                minMaxAttribute.min, minMaxAttribute.max);

                if (minVal < minMaxAttribute.min)
                {
                    minVal = minMaxAttribute.min;
                }

                if (maxVal > minMaxAttribute.max)
                {
                    maxVal = minMaxAttribute.max;
                }

                vector = new Vector2(minVal > maxVal ? maxVal : minVal, maxVal);

                if (EditorGUI.EndChangeCheck())
                {
                    property.vector2Value = vector;
                }

            }
            else if (propertyType == SerializedPropertyType.Vector2Int)
            {

                EditorGUI.BeginChangeCheck();

                Vector2Int vector = property.vector2IntValue;
                float minVal = vector.x;
                float maxVal = vector.y;

                minVal = EditorGUI.FloatField(splittedRect[0], minVal);
                maxVal = EditorGUI.FloatField(splittedRect[2], maxVal);

                EditorGUI.MinMaxSlider(splittedRect[1], ref minVal, ref maxVal,
                minMaxAttribute.min, minMaxAttribute.max);

                if (minVal < minMaxAttribute.min)
                {
                    maxVal = minMaxAttribute.min;
                }

                if (minVal > minMaxAttribute.max)
                {
                    maxVal = minMaxAttribute.max;
                }

                vector = new Vector2Int(Mathf.FloorToInt(minVal > maxVal ? maxVal : minVal), Mathf.FloorToInt(maxVal));

                if (EditorGUI.EndChangeCheck())
                {
                    property.vector2IntValue = vector;
                }

            }

        }

        Rect[] SplitRect(Rect rectToSplit, int n)
        {


            Rect[] rects = new Rect[n];

            for (int i = 0; i < n; i++)
            {

                rects[i] = new Rect(rectToSplit.position.x + (i * rectToSplit.width / n), rectToSplit.position.y, rectToSplit.width / n, rectToSplit.height);

            }

            int padding = (int)rects[0].width - 40;
            int space = 5;

            rects[0].width -= padding + space;
            rects[2].width -= padding + space;

            rects[1].x -= padding;
            rects[1].width += padding * 2;

            rects[2].x += padding + space;


            return rects;

        }

    }
#endif

    /// <summary>
    /// This attribute allows you to create a slider for Vector2 or Vector2Int fields in the Unity Inspector,
    /// specifying the minimum and maximum values for the slider.
    /// </summary>
    public class VectorSliderAttribute : PropertyAttribute
    {

        public float min;
        public float max;

        public VectorSliderAttribute(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }

    #endregion

    #region TAGFIELD

    /// <summary>
    /// This attribute allows you to select a tag from a dropdown menu in the inspector, 
    /// ensuring that only valid tags can be assigned to the string field.
    /// </summary>
    public class TagFieldAttribute : PropertyAttribute
    {
        //Reference:
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TagFieldAttribute))]
    public class TagSelectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.BeginProperty(position, label, property);

                List<string> tagList = new()
                {
                    "Untagged"
                };
                tagList.AddRange(UnityEditorInternal.InternalEditorUtility.tags);

                int index = tagList.IndexOf(property.stringValue);
                if (index < 0) index = 0;

                EditorGUI.BeginChangeCheck();
                index = EditorGUI.Popup(position, label.text, index, tagList.ToArray());

                if (EditorGUI.EndChangeCheck())
                {
                    if (index == 0)
                        property.stringValue = "";
                    else
                        property.stringValue = tagList[index];
                }

                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }

#endif

    #endregion

    #endregion

    #region UTILITY

    /// <summary>
    /// Extension method to convert Color to Hex string
    /// </summary>
    public static class ColorExtension
    {
        public static string ConvertToHex(this Color color)
        {
            Color32 c = color;
            return $"{c.r:X2}{c.g:X2}{c.b:X2}{c.a:X2}";
        }
    }

    #endregion

    #region Prefabs Creator

#if UNITY_EDITOR
    public static class PrefabsCreator
    {
        #region UI

        [MenuItem("GameObject/Palexen/UI/3D Icon", false, 0)]
        static void Create3DIcon()
        {
            GameObject prefabAsset = Resources.Load<GameObject>("Prefabs/3D Icon");

            if(prefabAsset != null)
            {
                GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
                Selection.activeGameObject = clone;
                EditorGUIUtility.PingObject(clone);
            }
            else
            {
                Debug.LogError("Can't Find prefab in the <color=yellow>Prefabs/ </color> folder, " +
                    "create new prefab and put in there, or <color=cyan>Reimport</color> the package");
            }
        }

        [MenuItem("GameObject/Palexen/UI/Input Schema", false, 0)]
        static void CreateInputSchema()
        {
            GameObject prefabAsset = Resources.Load<GameObject>("Prefabs/Game Input Schema");

            if (prefabAsset != null)
            {
                GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
                Selection.activeGameObject = clone;
                EditorGUIUtility.PingObject(clone);
            }
            else
            {
                Debug.LogError("Can't Find prefab in the <color=yellow>Prefabs/ </color> folder, " +
                    "create new prefab and put in there, or <color=cyan>Reimport</color> the package");
            }
        }

        [MenuItem("GameObject/Palexen/UI/Interactable HUD", false, 0)]
        static void CreateInteractableHUD()
        {
            GameObject prefabAsset = Resources.Load<GameObject>("Prefabs/Interactable HUD");

            if (prefabAsset != null)
            {
                GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
                Selection.activeGameObject = clone;
                EditorGUIUtility.PingObject(clone);
            }
            else
            {
                Debug.LogError("Can't Find prefab in the <color=yellow>Prefabs/ </color> folder, " +
                    "create new prefab and put in there, or <color=cyan>Reimport</color> the package");
            }
        }

        #endregion

        #region AUDIO
        [MenuItem("GameObject/Palexen/Audio/Ambience (Zone)", false, 0)]
        static void CreateAmbienceZone()
        {
            GameObject prefabAsset = Resources.Load<GameObject>("Prefabs/Ambience Zone");

            if (prefabAsset != null)
            {
                GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
                Selection.activeGameObject = clone;
                EditorGUIUtility.PingObject(clone);
            }
            else
            {
                Debug.LogError("Can't Find prefab in the <color=yellow>Prefabs/ </color> folder, " +
                    "create new prefab and put in there, or <color=cyan>Reimport</color> the package");
            }
        }

        [MenuItem("GameObject/Palexen/Audio/Ambience", false, 0)]
        static void CreateAmbience()
        {
            GameObject prefabAsset = Resources.Load<GameObject>("Prefabs/Audio Ambience");

            if (prefabAsset != null)
            {
                GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
                Selection.activeGameObject = clone;
                EditorGUIUtility.PingObject(clone);
            }
            else
            {
                Debug.LogError("Can't Find prefab in the <color=yellow>Prefabs/ </color> folder, " +
                    "create new prefab and put in there, or <color=cyan>Reimport</color> the package");
            }
        }

        [MenuItem("GameObject/Palexen/Audio/Loudspeaker Audio", false, 0)]
        static void CreateLoudspeaker()
        {
            GameObject prefabAsset = Resources.Load<GameObject>("Prefabs/ExtraSource");

            if (prefabAsset != null)
            {
                GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
                Selection.activeGameObject = clone;
                EditorGUIUtility.PingObject(clone);
            }
            else
            {
                Debug.LogError("Can't Find prefab in the <color=yellow>Prefabs/ </color> folder, " +
                    "create new prefab and put in there, or <color=cyan>Reimport</color> the package");
            }
        }

        #endregion

        #region GAME LOGICS

        [MenuItem("GameObject/Palexen/Game Logics/Trigger Object Manager", false, 0)]
        static void CreateTriggerObjectManager()
        {
            GameObject prefabAsset = Resources.Load<GameObject>("Prefabs/ObjectsCollection");

            if (prefabAsset != null)
            {
                GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
                Selection.activeGameObject = clone;
                EditorGUIUtility.PingObject(clone);
            }
            else
            {
                Debug.LogError("Can't Find prefab in the <color=yellow>Prefabs/ </color> folder, " +
                    "create new prefab and put in there, or <color=cyan>Reimport</color> the package");
            }
        }

        [MenuItem("GameObject/Palexen/Game Logics/Trigger Event", false, 0)]
        static void CreateTriggerEvent()
        {
            GameObject prefabAsset = Resources.Load<GameObject>("Prefabs/Trigger Event");

            if (prefabAsset != null)
            {
                GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
                Selection.activeGameObject = clone;
                EditorGUIUtility.PingObject(clone);
            }
            else
            {
                Debug.LogError("Can't Find prefab in the <color=yellow>Prefabs/ </color> folder, " +
                    "create new prefab and put in there, or <color=cyan>Reimport</color> the package");
            }
        }

        [MenuItem("GameObject/Palexen/Game Logics/Interactable GameObject", false, 0)]
        static void CreateInteractableGO()
        {
            GameObject prefabAsset = Resources.Load<GameObject>("Prefabs/Interactable Object Sample");

            if (prefabAsset != null)
            {
                GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
                Selection.activeGameObject = clone;
                EditorGUIUtility.PingObject(clone);
            }
            else
            {
                Debug.LogError("Can't Find prefab in the <color=yellow>Prefabs/ </color> folder, " +
                    "create new prefab and put in there, or <color=cyan>Reimport</color> the package");
            }
        }

        [MenuItem("GameObject/Palexen/Game Logics/Async Loader", false, 0)]
        static void CreateAsyncLoader()
        {
            GameObject prefabAsset = Resources.Load<GameObject>("Prefabs/Async Loader");

            if (prefabAsset != null)
            {
                GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
                Selection.activeGameObject = clone;
                EditorGUIUtility.PingObject(clone);
            }
            else
            {
                Debug.LogError("Can't Find prefab in the <color=yellow>Prefabs/ </color> folder, " +
                    "create new prefab and put in there, or <color=cyan>Reimport</color> the package");
            }
        }

        [MenuItem("GameObject/Palexen/Game Logics/Footsteps (Preconfigured)", false, 0)]
        static void CreatePlayerFootSteps()
        {
            GameObject prefabAsset = Resources.Load<GameObject>("Prefabs/Footsteps - Preconfigured");

            if (prefabAsset != null)
            {
                GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
                Selection.activeGameObject = clone;
                EditorGUIUtility.PingObject(clone);
            }
            else
            {
                Debug.LogError("Can't Find prefab in the <color=yellow>Prefabs/ </color> folder, " +
                    "create new prefab and put in there, or <color=cyan>Reimport</color> the package");
            }
        }

        #endregion

        #region TOOLS

        [MenuItem("GameObject/Palexen/Tools/Lang Manager", false, 0)]
        static void CreateLangManager()
        {
            GameObject prefabAsset = Resources.Load<GameObject>("Prefabs/Lang Manager");

            if (prefabAsset != null)
            {
                GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
                Selection.activeGameObject = clone;
                EditorGUIUtility.PingObject(clone);
            }
            else
            {
                Debug.LogError("Can't Find prefab in the <color=yellow>Prefabs/ </color> folder, " +
                    "create new prefab and put in there, or <color=cyan>Reimport</color> the package");
            }
        }

        [MenuItem("GameObject/Palexen/Tools/Dialog (Example)", false, 1)]
        static void CreateDialogExample()
        {
            GameObject prefabAsset = Resources.Load<GameObject>("Prefabs/Dialog Example");

            if (prefabAsset != null)
            {
                GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
                Selection.activeGameObject = clone;
                EditorGUIUtility.PingObject(clone);
            }
            else
            {
                Debug.LogError("Can't Find prefab in the <color=yellow>Prefabs/ </color> folder, " +
                    "create new prefab and put in there, or <color=cyan>Reimport</color> the package");
            }
        }

        #endregion
    }
#endif

    #endregion
}