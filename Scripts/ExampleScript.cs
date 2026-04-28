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
using UnityEngine;
using Palexen.Tools;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Palexen.Misc
{
    [ScriptDescription("Example Script", "This is an example, you not need do nothing")]
    public class ExampleScript : MonoBehaviour
    {
        
        [VectorSlider(0, 10)] public Vector2 minMaxFloat;
        [VectorSlider(0, 10)] public Vector2Int minMaxInt;

        [MyHeader("Red Object Without Error")]
        [FieldColor] public GameObject redObject;

        [MyHeader("Red Object With Error")]
        [FieldColor(FieldPropertyColor.red, ShowObjectMessage.errorMessage)] public GameObject singleRedObject;

        [MyHeader("Green Object With message")]
        [FieldColor(FieldPropertyColor.green, ShowObjectMessage.message)] public GameObject greenObject;

        [MyHeader("Blue Object With Warning Message")]
        [FieldColor(FieldPropertyColor.blue, ShowObjectMessage.warningMessage)] public GameObject blueObject;

        [MyHeader("Yellow Object")]
        [FieldColor(FieldPropertyColor.yellow)] public GameObject yellowObject;

        [MyHeader("Cyan Object")]
        [FieldColor(FieldPropertyColor.cyan)] public GameObject cyanObject;

        [MyHeader("Magenta Object")]
        [FieldColor(FieldPropertyColor.magenta)] public GameObject magentaObject;

        [MyHeader("Orange Object")]
        [FieldColor(FieldPropertyColor.orange)] public GameObject orangeObject;

        [MyHeader("Clear Blue Object")]
        [FieldColor(FieldPropertyColor.clearBlue)] public GameObject clearBlueObject;

        [MyHeader("Pink Object")]
        [FieldColor(FieldPropertyColor.pink)] public GameObject pinkObject;

        [MyHeader("Neon Green Object")]
        [FieldColor(FieldPropertyColor.neonGreen)] public GameObject neonGreenObject;

        [MyHeader("Salmon Object")]
        [FieldColor(FieldPropertyColor.salmon)] public GameObject salmonObject;

        [MyHeader("Array")]
        [FieldColor] public GameObject[] arrayObjects;

        public void GoToOnlineManual()
        {
#if UNITY_EDITOR
            Help.BrowseURL("https://www.palexen.com");
#endif
        }
    }
}