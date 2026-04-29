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

[InitializeOnLoad]
public static class PalexenDefineSetter
{
    const string DEFINE_SYMBOL = "PALEXEN_TOOLS";

    static PalexenDefineSetter()
    {
        AddDefineIfNeeded();
    }

    static void AddDefineIfNeeded()
    {
        var targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

        if (!defines.Contains(DEFINE_SYMBOL))
        {
            defines = string.IsNullOrEmpty(defines) ? DEFINE_SYMBOL : $"{defines};{DEFINE_SYMBOL}";
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
            Debug.Log($"[PALEXEN] Define '{DEFINE_SYMBOL}' automatically added.");
        }
    }
}
#endif

