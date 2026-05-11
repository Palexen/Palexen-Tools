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
using UnityEditor;
using UnityEngine;
public static class PalexenEditorStyles
{
    /// <summary>
    /// A custom GUIStyle for big buttons in the Unity Editor.
    /// </summary>
    /// <remarks>This style is designed for use with buttons that require a larger, more prominent appearance. 
    /// It features bold text, increased font size, and a fixed height to ensure consistency across different button instances.</remarks>
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

    /// <summary>
    /// A custom GUIStyle for boxes in the Unity Editor with customizable font size, text alignment, font style, and height.
    /// </summary>
    /// <param name="fontSize">The font size of the text inside the box.</param>
    /// <param name="textAnchor">The alignment of the text inside the box.</param>
    /// <param name="fontStyle">The style of the font (e.g., Normal, Bold, Italic).</param>
    /// <param name="height">The fixed height of the box.</param>
    /// <returns>A GUIStyle configured with the specified parameters.</returns>
    public static GUIStyle CoolBox(int fontSize = 10, TextAnchor textAnchor = TextAnchor.MiddleCenter, FontStyle fontStyle = FontStyle.Normal, int height = 40)
    {
        GUIStyle style = new(GUI.skin.box)
        {
            richText = true,
            alignment = textAnchor,
            fontSize = fontSize,
            fontStyle = fontStyle,
            fixedHeight = height,
            fixedWidth = Screen.width * .9f,
            stretchWidth = true
        };
        return style;
    }

    /// <summary>
    /// A custom GUIStyle for titles in the Unity Editor with customizable font size, text alignment, font style, and height.
    /// </summary>
    /// <param name="fontSize">The font size of the title text.</param>
    /// <param name="textAlignment">The alignment of the title text.</param>
    /// <param name="fontStyle">The style of the font (e.g., Normal, Bold, Italic).</param>
    /// <param name="height">The fixed height of the title.</param>
    /// <returns>A GUIStyle configured with the specified parameters.</returns>
    public static GUIStyle CoolTitle(int fontSize = 18, TextAnchor textAlignment = TextAnchor.MiddleCenter, FontStyle fontStyle = FontStyle.Bold, int height = 40)
    {
        GUIStyle style = new(GUI.skin.label)
        {        
            richText = true,
            alignment = textAlignment,
            fontSize = fontSize,
            fontStyle = fontStyle,
            fixedHeight = height
        };
        return style;
    }

    /// <summary>
    /// Draws a horizontal line in the Unity Editor with customizable color, thickness, padding, and margin.
    /// </summary>
    /// <param name="color">The color of the horizontal line.</param>
    /// <param name="thickness">The thickness of the horizontal line.</param>
    /// <param name="padding">The padding above and below the line.</param>
    /// <param name="margin">The margin on the left and right sides of the line.</param>
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