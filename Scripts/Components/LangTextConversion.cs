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
        public Language _lang;
        public Initializer _catchLang;

        [MyHeader("Setup")]
        [FieldColor(FieldPropertyColor.pink, ShowObjectMessage.errorMessage) ]public TMP_Text _text;
        [FieldColor(FieldPropertyColor.clearBlue, ShowObjectMessage.errorMessage)] public LangTextContainer _conversions;

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
            switch ( _lang )
            {
                case Language.english:
                    _text.text = _conversions._conversions[0]._text;
                    break;
                case Language.spanish:
                    _text.text = _conversions._conversions[1]._text;
                    break;
                case Language.french:
                    _text.text = _conversions._conversions[2]._text;
                    break;
                case Language.german:
                    _text.text = _conversions._conversions[3]._text;
                    break;
                case Language.japanese:
                    _text.text = _conversions._conversions[4]._text;
                    break;
                case Language.chinese:
                    _text.text = _conversions._conversions[5]._text;
                    break;
                case Language.korean:
                    _text.text = _conversions._conversions[6]._text;
                    break;
                case Language.russian:
                    _text.text = _conversions._conversions[7]._text;
                    break;
            }
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
                _lang = LangManager.instance.GetLang();
                SwitchLang();
            }
        }

        #endregion
    }
}
