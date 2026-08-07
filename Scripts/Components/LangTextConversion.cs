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
using TMPro;
using UnityEngine;

#if PALEXEN_TOOLS
using Palexen.Tools;
#endif

namespace Palexen.Gameplay.UI
{
    #if PALEXEN_TOOLS
    [ScriptDescription("Lang Text Conversion", "Translate the text into the selected language, previously configured in a subtitles component.")]
#endif
    [AddComponentMenu("Palexen/UI/Text Translator")]
    public class LangTextConversion : MonoBehaviour
    {
        #region VARIABLES

        [MyHeader("Language")]
        [SerializeField] [LanguagesDropdown("Language")] private string _lang;
        [SerializeField] private Initializer _catchLang;

        [MyHeader("Setup")]
        [FieldColor(FieldPropertyColor.pink, ShowObjectMessage.errorMessage) ][SerializeField] private TMP_Text _text;
        [FieldColor(FieldPropertyColor.clearBlue, ShowObjectMessage.errorMessage)][SerializeField] private LangTextContainer _conversions;

        #endregion

        #region UNITY METHODS

        void Start()
        {
            UpdateLang();
        }

        #endregion

        #region MECHANICS

        void SwitchLang()
        {
            _text.text = _conversions._conversions[LangManager.instance.LangIndex]._text;
        }

        #endregion

        #region API

        /// <summary>
        /// Update the text language; this action is the default when calling the Lang Manager singleton.
        /// </summary>
        public void UpdateLang()
        {
            if (_catchLang == Initializer.auto)
            {
                _lang = LangManager.instance.LangName;
                SwitchLang();
            }
        }

        #endregion
    }
}
