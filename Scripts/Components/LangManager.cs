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
using Palexen.Gameplay.UI;
using Palexen.Sequences;
using UnityEngine;

namespace Palexen.Tools
{
    #if PALEXEN_TOOLS
    [ScriptDescription("Lang Manager", "This is the global language setting")]
#endif
    [AddComponentMenu("Palexen/Tools/Lang Manager")]
    public class LangManager : MonoBehaviour
    {
        #region VARIABLES

        public static LangManager instance;
        public Language _lang;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            if(instance == null )
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            gameObject.name = "Lang Manager";
        }

#endif

        #endregion

        #region MECHANICS

        void UpdateDialogSystems()
        {
            DialogSystem[] ds = FindObjectsByType<DialogSystem>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            for (int i = 0; i < ds.Length; i++)
            {
                ds[i].UpdateLang();
            }
        }

        void UpdateCC()
        {
            LangTextConversion[] ltc = FindObjectsByType<LangTextConversion>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            for(int i = 0;i < ltc.Length; i++)
            {
                ltc[i].UpdateLang();
            }
        }

        public void SetEnglish()
        {
            SetLang(Language.english);
        }
        public void SetSpanish()
        {
            SetLang(Language.spanish);
        }
        public void SetFrench()
        {
            SetLang(Language.french);
        }
        public void SetGerman()
        {
            SetLang(Language.german);
        }
        public void SetJapanese()
        {
            SetLang(Language.japanese);
        }
        public void SetChinese()
        {
            SetLang(Language.chinese);
        }
        public void SetKorean()
        {
            SetLang(Language.korean);
        }
        public void SetRussian()
        {
            SetLang(Language.russian);
        }

        #endregion

        #region API

        /// <summary>
        /// Set a new language entry; the singleton will update, but you'll still need to update all your other 
        /// objects that use the singleton to get the language, or wait to reload/load a scene. 
        /// </summary>
        /// <param name="newLang"> It will establish a new language; you will be able to select it when setting it up. </param>
        /// <remarks>Note: You'll need to create a system that saves the language state, either by saving to PlayerPrefs or something more complex.</remarks>
        public void SetLang(Language newLang)
        {
            _lang = newLang;
            UpdateDialogSystems();
            UpdateCC();
        }

        /// <summary>
        /// Gets the current language
        /// If the language manager singleton exists, it retrieves it and sets it for use in the dialog system.
        /// </summary>
        /// <returns>Language enum</returns>
        public Language GetLang()
        {
            return _lang;
        }

        #endregion
    }
}
