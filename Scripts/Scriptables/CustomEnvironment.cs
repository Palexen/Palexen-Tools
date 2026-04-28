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
using Palexen.Tools;
using System.ComponentModel;
using UnityEngine;

namespace Palexen.Scriptables
{
	[CreateAssetMenu(fileName = "Palexen Environment Configuration", menuName = "Palexen/Environment Settings")]
	public class CustomEnvironment : ScriptableObject
	{
		#region VARIABLES
		[MyHeader("Script Description Setup")]
		public TurnOnScriptDescription scriptDescriptionState;
		public string scriptNameSpace = "Palexen";
        public Color scriptTitleColor = new Color(1, .6275f, .4784f, 1);
		[Range(14, 25)] public int scriptTitleSize = 18;

		[Space(10)]

		[MyHeader("Header Setup & Color")]
		public Color headerColorValue = new Color(.6784f, .8471f, .902f, 1);
		[Range(12, 22)] public int headerSize = 14;

		[Space(10)]

		[MyHeader("Colors")]
		public Color contextSeparatorColor = Color.cyan;
        [Header("Global gizmo Color")]
        public Color gizmosColor = Color.white;
        public Color inactiveGizmosColor = Color.white;

		[Space(12)]

        [MyHeader("Messages to Show in inspector")]
        [TextArea] public string infoString = "Info Message | Example";
        [TextArea] public string warningString = "Warning Message | Example";
        [TextArea] public string errorString = "Error Message | Example";


		[MyHeader("The color fields and messages will look like this (No need to add any reference here)")]
        [FieldColor(FieldPropertyColor.cyan, ShowObjectMessage.message)] [SerializeField] private GameObject infoObject;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] [SerializeField] private GameObject warningObject;
        [FieldColor(FieldPropertyColor.red, ShowObjectMessage.errorMessage)] [SerializeField] private GameObject errorObject;

		[MyHeader("All Field Colors")]
        [FieldColor(FieldPropertyColor.cyan)] public GameObject cyanObject;
        [FieldColor(FieldPropertyColor.yellow)] public GameObject yellowObject;
        [FieldColor(FieldPropertyColor.red)] public GameObject redObject;
        [FieldColor(FieldPropertyColor.green)] public GameObject greenObject;
        [FieldColor(FieldPropertyColor.blue)] public GameObject blueObject;
        [FieldColor(FieldPropertyColor.magenta)] public GameObject magentaObject;
        [FieldColor(FieldPropertyColor.orange)] public GameObject orangeObject;
        [FieldColor(FieldPropertyColor.clearBlue)] public GameObject clearBlueObject;
        [FieldColor(FieldPropertyColor.pink)] public GameObject pinkObject;
        [FieldColor(FieldPropertyColor.neonGreen)] public GameObject neonGreenObject;
        [FieldColor(FieldPropertyColor.salmon)] public GameObject salmonObject;

        /// Deprecated, will be removed in future updates, use the new message system instead
        /*[MyHeader("Other Colors")]
		public Color[] colors = { Color.white };*/

        #endregion
    }
}
