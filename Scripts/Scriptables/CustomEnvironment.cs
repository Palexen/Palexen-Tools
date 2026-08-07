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
		[SerializeField] private TurnOnScriptDescription scriptDescriptionState;
        [SerializeField] private string scriptNameSpace = "Palexen";
        [SerializeField] private Color scriptTitleColor = new Color(1, .6275f, .4784f, 1);
		[Range(14, 25)][SerializeField] private int scriptTitleSize = 18;

		[Space(10)]

		[MyHeader("Header Setup & Color")]
        [SerializeField] private Color headerColorValue = new Color(.6784f, .8471f, .902f, 1);
		[Range(12, 22)][SerializeField] private int headerSize = 14;

		[Space(10)]

		[MyHeader("Separators")]
        [Notepad("Use <color=yellow>[Separator]</color> and <color=yellow>[Line]</color> Attributes to paint your Separators " +
            "and use <color=yellow>[Notepad]</color> attribute to place custom messages directly on your scripts", FontStyle.BoldAndItalic)]
        [Separator]
        [Line(DrawOn.bottom)]
        [SerializeField] private Color contextSeparatorColor = Color.cyan;

        [MyHeader("Global Gizmos Settings")]
        [SerializeField] private GizmoForm contextGizmoForm = GizmoForm.sphere;
        [SerializeField] private Color gizmosColor = Color.white;
        [SerializeField] private Color inactiveGizmosColor = Color.red;
        [SerializeField] private float gizmoSize = .25f;

        [MyHeader("<color=green>Physics</color> Simulation")]
        [SerializeField] private LayerMask physicsSimulationLayer = 1;

        [MyHeader("Scriptables Folder Path: <size=10>(The scriptables generated from the toolbar.)</size>")]
        [Line(DrawOn.bottom)]
        [SerializeField] private string scriptablesFolderPath = "Assets/";

        [MyHeader("Quick Prefabs Settings")]
        [EasyDropdown("Prefab Collection")][SerializeField] private string prefabIndex;
        [FieldColor(FieldPropertyColor.salmon, ShowObjectMessage.warningMessage)] [SerializeField] private EntityManager _entities;

        [MyHeader("Language Settings")]
        [LanguagesDropdown("Languages List")][SerializeField] private string currentLanguage;
        [FieldColor(FieldPropertyColor.salmon, ShowObjectMessage.errorMessage)][SerializeField] private Languages _languages;

        [Space(12)]

        [MyHeader("Messages to Show in inspector")]
        [TextArea] [SerializeField] private string infoString = "Info Message | Example";
        [TextArea] [SerializeField] private string warningString = "Warning Message | Example";
        [TextArea] [SerializeField] private string errorString = "Error Message | Example";


		[MyHeader("Field Colors and messages")]
        [Notepad("The Field Colors and messages, will look like this (No need to add any reference here)\n" +
            "Use <color=yellow>[FieldColor(FieldPropertyColor, ShowObjectMessage, bool)]</color> to use it", FontStyle.BoldAndItalic)]
        [FieldColor(FieldPropertyColor.cyan, ShowObjectMessage.message)] [SerializeField] private GameObject infoObject;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] [SerializeField] private GameObject warningObject;
        [FieldColor(FieldPropertyColor.red, ShowObjectMessage.errorMessage)] [SerializeField] private GameObject errorObject;

        [MyHeader("Tag Attribute")]
        [Notepad("Use <color=yellow>[TagField]</color> on your string tag to use it", FontStyle.BoldAndItalic)]
        [TagField] [SerializeField] private string tagFieldAttribute;

        [MyHeader("Slider Attribute")]
        [Notepad("Use <color=YELLOW>[VectorSlider(Min Value, Max Value)]</color> to use it", FontStyle.BoldAndItalic)]
        [VectorSlider(0, 1)] [SerializeField] private Vector2 vector2Slider = new(0, 1);
        [VectorSlider(0, 100)] [SerializeField] private Vector2Int vector2SliderInt = new(0, 50);

        [MyHeader("All Field Colors")]
        [FieldColor(FieldPropertyColor.cyan)] [SerializeField] private GameObject cyanObject;
        [FieldColor(FieldPropertyColor.yellow)] [SerializeField] private GameObject yellowObject;
        [FieldColor(FieldPropertyColor.red)] [SerializeField] private GameObject redObject;
        [FieldColor(FieldPropertyColor.green)] [SerializeField] private GameObject greenObject;
        [FieldColor(FieldPropertyColor.blue)] [SerializeField] private GameObject blueObject;
        [FieldColor(FieldPropertyColor.magenta)] [SerializeField] private GameObject magentaObject;
        [FieldColor(FieldPropertyColor.orange)] [SerializeField] private GameObject orangeObject;
        [FieldColor(FieldPropertyColor.clearBlue)] [SerializeField] private GameObject clearBlueObject;
        [FieldColor(FieldPropertyColor.pink)] [SerializeField] private GameObject pinkObject;
        [FieldColor(FieldPropertyColor.neonGreen)] [SerializeField] private GameObject neonGreenObject;
        [FieldColor(FieldPropertyColor.salmon)] [SerializeField] private GameObject salmonObject;

        #endregion

        #region PROPERTIES

        public TurnOnScriptDescription ScriptDescriptionState { get { return scriptDescriptionState; } }
        public GizmoForm ContextGizmoForm { get { return contextGizmoForm; } }

        public Color ScriptTitleColor { get { return scriptTitleColor; } }
        public Color HeaderColor { get { return headerColorValue; } }
        public Color ContextSeparatorColor { get { return contextSeparatorColor; } }
        public Color GizmoColor { get { return gizmosColor; } }
        public Color InactiveGizmosColor { get { return inactiveGizmosColor; } }

        public int ScriptTitleSize { get { return scriptTitleSize; } }
        public int HeaderSize { get { return headerSize; } }

        public float GizmoSize { get { return gizmoSize; } }

        public string ScriptNameSpace { get { return scriptNameSpace; } }
        public string InfoString { get { return infoString; } }
        public string WarningString { get { return warningString; } }
        public string ErrorString { get { return errorString; } }

        public string ScriptablesFolderPath
        {
            get { return scriptablesFolderPath; }
            set { scriptablesFolderPath = value; }
        }

        public Languages Languages
        {
            get { return _languages; }
            set { _languages = value; }
        }

        public string PrefabIndex
        {
            get { return prefabIndex; }
            set { prefabIndex = value; }
        }

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

        /// <summary>
        /// This method is used to get the current prefab index.
        /// </summary>
        public string CurrentPrefab
        {
            get {  return prefabIndex; }
            set
            {
                prefabIndex = value;
            }
        }

        public EntityManager Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        #endregion
    }
}
