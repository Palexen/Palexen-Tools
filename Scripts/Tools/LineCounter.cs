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
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Palexen.Tools
{
    public class LineCounter : EditorWindow
    {
        private string folderPath = "Assets/Scripts";
        string coreName = "Core";
        string[] files;
        int totalLines = 0;
        int allLetters;
        int totalWords;

        [MenuItem("Palexen/Window/Code Statistics")]
        public static void ShowWindow()
        {
            GetWindow<LineCounter>("Code Statistics");
        }

        private void OnGUI()
        {
            string logoPath = "Packages/com.palexen.tools/Editor Default Resources/Palexen Logo Semi 512.png";
            Texture2D logo = AssetDatabase.LoadAssetAtPath<Texture2D>(logoPath);

            EditorGUILayout.BeginHorizontal();
            //GUI.DrawTexture(new Rect(20, 20, 92, 92), logo, ScaleMode.ScaleToFit);
            GUILayout.Label("Code Statistics", PalexenEditorStyles.CoolTitle(24, TextAnchor.MiddleCenter, FontStyle.Bold, 40));
            GUILayout.EndHorizontal();

            GUILayout.Label("Directory Setup", EditorStyles.boldLabel);
            folderPath = EditorGUILayout.TextField("Root Path", folderPath);
            coreName = EditorGUILayout.TextField("Core Name", coreName);

            PalexenEditorStyles.DrawHorizontalLine(Color.gray);

            if (GUILayout.Button("Calculate Total Lines", PalexenEditorStyles.BigButton))
            {
                CountLines();
            }

            if(files != null && files.Length > 0)
            {
                string r = totalLines.ToString("N0");
                GUI.color = new Color(.68f, 1, .18f, 1);
                GUILayout.Box($"You have written: {r} lines of C# code (in {files.Length} files) " +
                    $"for {coreName}.", 
                    PalexenEditorStyles.CoolBox(18));
                GUI.color = Color.white;

                if(totalLines > 10000)
                {
                    GUI.color = Color.cyan;
                    GUILayout.Box($"Total letters: {allLetters:N0} and total words: {totalWords:N0}", PalexenEditorStyles.CoolBox(18));
                    GUI.color = Color.white;

                    GUILayout.Box("Whoa! Are you trying to build a new universe?", PalexenEditorStyles.CoolBox(18));
                }
                else if (totalLines > 5000 && totalLines < 10000)
                {
                    GUI.color = Color.cyan;
                    GUILayout.Box($"Total letters: {allLetters:N0} and total words: {totalWords:N0}", PalexenEditorStyles.CoolBox(18));
                    GUI.color = Color.white;

                    GUILayout.Box("My God! Are you creating a new operating system?", PalexenEditorStyles.CoolBox(18));
                }
                else if (totalLines > 500 && totalLines < 5000)
                {
                    GUI.color = Color.cyan;
                    GUILayout.Box($"Total letters: {allLetters:N0} and total words: {totalWords:N0}", PalexenEditorStyles.CoolBox(18));
                    GUI.color = Color.white;

                    GUILayout.Box("Watch out, we've got a genius spitting fire!", PalexenEditorStyles.CoolBox(18));
                }
                else if (totalLines > 51 && totalLines < 500)
                {
                    GUI.color = Color.cyan;
                    GUILayout.Box($"Total letters: {allLetters:N0} and total words: {totalWords:N0}", PalexenEditorStyles.CoolBox(18));
                    GUI.color = Color.white;

                    GUILayout.Box("The coffee is kicking in—keep it up!", PalexenEditorStyles.CoolBox(18));
                }
                else
                {
                    GUI.color = Color.cyan;
                    GUILayout.Box($"Total letters: {allLetters:N0} and total words: {totalWords:N0}", PalexenEditorStyles.CoolBox(18));
                    GUI.color = Color.white;

                    GUILayout.Box("Every great empire begins with a brick!", PalexenEditorStyles.CoolBox(18));
                }
            }
        }

        private void CountLines()
        {
            if (!Directory.Exists(folderPath))
            {
                Debug.LogError("Folder does not exist: " + folderPath);
                return;
            }

            files = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);
            totalLines = 0;
            allLetters = 0;
            totalWords = 0;

            foreach (string file in files)
            {
                totalLines += File.ReadAllLines(file).Length;
                allLetters += File.ReadAllText(file).Count(c => !char.IsWhiteSpace(c));
                totalWords += Regex.Matches(File.ReadAllText(file), @"\b[A-Za-zÀ-ÿ0-9_]+\b").Count;
            }
        }
    }
}
#endif