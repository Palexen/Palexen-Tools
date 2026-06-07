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
        public string sceneName;
        public float _delayTimer = 5f;
        public bool _rootActivation;

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

        public void SetScene(string scene, float delay = 5f, bool ra = false)
        {
            sceneName = scene;
            _delayTimer = delay;
            _rootActivation = ra;
        }

        public string GetScene()
        {
            return sceneName;
        }

        public void SetDelay(float delay)
        {
            _delayTimer = delay;
        }

        public float GetDelay()
        {
            return _delayTimer;
        }

        public bool CheckRootActivation()
        {
            return _rootActivation;
        }

        #endregion
    }
}
