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
#if PALEXEN_TOOLS
using Palexen.Tools;
#endif

namespace Palexen.Levels
{
    #if PALEXEN_TOOLS
    [ScriptDescription("Level Manager", "Catch and load levels")]
#endif
    [AddComponentMenu("Palexen/Levels/Level Manager (Master)")]
    public class LevelManager : MonoBehaviour
    {
        #region VARIABLES

        public static LevelManager instance;
        private string _sceneName;
        private float _delayTimer;
        private bool _rootActivation;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Gets and sets the scene name
        /// </summary>
        public string SceneName { get => _sceneName; set => _sceneName = value; }

        /// <summary>
        /// Gets and sets a timer
        /// </summary>
        public float Delay { get => _delayTimer; set => _delayTimer = value; }

        /// <summary>
        /// Checks and sets whether or not a root object is used at the next level.
        /// </summary>
        public bool IsRootActivation { get => _rootActivation; set => _rootActivation = value; }

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            if (instance == null) 
            { 
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        #region MECHANICS



        #endregion

        #region API

        public void SetScene(string scene, float delay = 5f, bool rootActivation = false)
        {
            SceneName = scene;
            Delay = delay;
            IsRootActivation = rootActivation;
        }

        #region OBSOLETE
        [Obsolete("Use SceneName instead.")]
        public string GetScene() => SceneName;

        [Obsolete("Use Delay instead.")]
        public void SetDelay(float delay) => Delay = delay;

        [Obsolete("Use Delay instead.")]
        public float GetDelay() => Delay;

        [Obsolete("Use IsRootActivation instead.")]
        public bool CheckRootActivation() => IsRootActivation;
        #endregion

        #endregion
    }
}
