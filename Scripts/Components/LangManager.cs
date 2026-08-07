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
using System;
using UnityEngine;
using Palexen.Sequences;
using Palexen.Gameplay.UI;
using Palexen.Scriptables;

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
        [FieldColor(FieldPropertyColor.salmon, ShowObjectMessage.errorMessage)] [SerializeField] private Languages _languages;
        [SerializeField] [LanguagesDropdown("Languages")] private string _langName;
        [SerializeField] private SubtitlesUsage _subtitles;

        int langIndex = 0;

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
            LangIndex = 0;
            _langName = _languages.LanguagesList[langIndex];
        }
        public void SetSpanish()
        {
            LangIndex = 1;
            _langName = _languages.LanguagesList[langIndex];
        }
        public void SetFrench()
        {
            LangIndex = 2;
            _langName = _languages.LanguagesList[langIndex];
        }
        public void SetGerman()
        {
            LangIndex = 3;
            _langName = _languages.LanguagesList[langIndex];
        }
        public void SetJapanese()
        {
            LangIndex = 4;
            _langName = _languages.LanguagesList[langIndex];
        }
        public void SetChinese()
        {
            LangIndex = 5;
            _langName = _languages.LanguagesList[langIndex];
        }
        public void SetKorean()
        {
            LangIndex = 6;
            _langName = _languages.LanguagesList[langIndex];
        }
        public void SetRussian()
        {
            LangIndex = 7;
            _langName = _languages.LanguagesList[langIndex];
        }

        #endregion

        #region PROPERTIES

        public SubtitlesUsage Subtitles {  get { return _subtitles; } set { _subtitles = value; } }
        public int LangIndex
        {
            get { return langIndex; }

            set 
            {
                if (value >= 0 && value < _languages.LanguagesList.Length)
                {
                    langIndex = value;
                    _langName = _languages.LanguagesList[langIndex];
                    UpdateDialogSystems();
                    UpdateCC();
                }
            }
        }
        public string LangName { get { return _languages.LanguagesList[langIndex]; } }
        public Languages LangContainer { get { return _languages; } set { _languages = value; } }

        #endregion

        #region API



        #endregion
    }
}
