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
using Palexen.Tools;

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

        [MyHeader("Global Gizmos Settings")]
        public GizmoForm contextGizmoForm = GizmoForm.sphere;
        public Color gizmosColor = Color.white;
        public Color inactiveGizmosColor = Color.red;
        
        [MyHeader("<color=green>Physics</color> Simulation")]
        public LayerMask physicsSimulationLayer = 1;

        [MyHeader("Scriptables Folder Path: <size=10>(The scriptables generated from the toolbar.)</size>")]
        public string scriptablesFolderPath = "Assets/";

        [Space(12)]

        [MyHeader("Messages to Show in inspector")]
        [TextArea] public string infoString = "Info Message | Example";
        [TextArea] public string warningString = "Warning Message | Example";
        [TextArea] public string errorString = "Error Message | Example";


		[MyHeader("The Field Colors and messages will look like this (No need to add any reference here)" +
            "\n Use </color>[FieldColor(FieldPropertyColor, ShowObjectMessage, bool)] to use it", 3)]
        [FieldColor(FieldPropertyColor.cyan, ShowObjectMessage.message)] [SerializeField] private GameObject infoObject;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] [SerializeField] private GameObject warningObject;
        [FieldColor(FieldPropertyColor.red, ShowObjectMessage.errorMessage)] [SerializeField] private GameObject errorObject;

        [MyHeader("Tag Attribute, use </color>[TagField] on your string tag to use it")]
        [TagField] public string tagFieldAttribute;

        [MyHeader("Slider Attribute, use </color>[VectorSlider(0, 1)] to use it")]
        [VectorSlider(0, 1)] public Vector2 vector2Slider = new(0, 1);
        [VectorSlider(0, 100)] public Vector2Int vector2SliderInt = new(0, 50);

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

        #region API

        /// <summary>
        /// This method is used to set the scriptables folder path.
        /// </summary>
        /// <param name="path">The path to set for the scriptables folder.</param>
        public void SetPath(string path)
        {
            scriptablesFolderPath = path;
        }
        
        #endregion
    }
}
