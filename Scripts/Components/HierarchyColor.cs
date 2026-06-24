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

namespace Palexen.Tools
{
    #if PALEXEN_TOOLS
    [ScriptDescription("Hierarchy Color", "Improved Monobehavior")]
#endif
    [AddComponentMenu("Palexen/Tools/Custom Hierarchy")]
    public class HierarchyColor : MonoBehaviour
    {
        #region VARIABLES

        [MyHeader("Color Indentation")]
        [SerializeField] private HierarchyIndentation _indentation;

        [MyHeader("Setup")]
        [SerializeField] private Color _myColor = new(0.5019608f, 0.5019608f, 0.5019608f, 0.1960784f);
        [SerializeField] private Texture2D[] _icons;

        [MyHeader("Font Style")]
        [SerializeField] private FontStyle _fontStyle;
        [SerializeField] private Color _fontColor = Color.white;

        #endregion

        #region UNITY METHODS

        #endregion

        #region PROPERTIES

        public Color MyColor { get { return _myColor; } set { _myColor = value; } }
        public Color FontColor { get { return _fontColor; } set { _fontColor = value; } }
        public FontStyle MyFontStyle { get { return _fontStyle; } set { _fontStyle = value; } }
        public HierarchyIndentation Indentation { get { return _indentation; } set {_indentation = value; } }
        public Texture2D[] Icons { get {return _icons;} set { _icons = value; } }

        #endregion

        #region MECHANICS



        #endregion

        #region API

        #endregion
    }
}
