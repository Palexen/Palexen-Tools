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
public static class PalexenEditorStyles
{
    public static GUIStyle BigButton
    {
        get
        {
            GUIStyle style = new(GUI.skin.button)
            {
                richText = true,
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                fixedHeight = 40
            };
            return style;
        }
    }

    public static GUIStyle CoolBox(int fontSize = 10, TextAnchor textAnchor = TextAnchor.MiddleCenter, FontStyle fontStyle = FontStyle.Normal)
    {
        GUIStyle style = new(GUI.skin.box)
        {
            richText = true,
            alignment = textAnchor,
            fontSize = fontSize,
            fontStyle = fontStyle,
            fixedHeight = 40,
            fixedWidth = Screen.width * .9f,
            stretchWidth = true
        };
        return style;
    }

    public static GUIStyle CoolTitle(int fontSize = 18, TextAnchor textAlignment = TextAnchor.MiddleCenter, FontStyle fontStyle = FontStyle.Bold)
    {
        GUIStyle style = new(GUI.skin.label)
        {        
            richText = true,
            alignment = textAlignment,
            fontSize = fontSize,
            fontStyle = fontStyle,
            fixedHeight = 40
        };
        return style;
    }

    public static void DrawHorizontalLine(Color color, int thickness = 1, int padding = 10, int margin = 0)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, GUILayout.Height(padding + thickness));

        rect.height = thickness;

        rect.y += padding * 0.5f;

        switch (margin)
        {
            case < 0:
                rect.x = 0;
                rect.width = EditorGUIUtility.currentViewWidth;
                break;
            case > 0:
                rect.x += margin;
                rect.width -= margin * 2;
                break;
        }

        EditorGUI.DrawRect(rect, color);
    }
}
#endif