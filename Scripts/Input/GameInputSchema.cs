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
using UnityEngine;
using Palexen.Tools;
using Palexen.Scriptables;

namespace Palexen.Gameplay.Input
{

    [ScriptDescription("Input Schema", "Set Input Schema for the game")]
    [AddComponentMenu("Palexen/UI/Input Scheme Manager")]
    public class GameInputSchema : MonoBehaviour
    {
        #region VARIABLES
        public static GameInputSchema Instance;
        [MyHeader("Current Input Schema")]
        public InputSchema schema;

        [MyHeader("UI Schemas")]
        [Header("Actions Buttons")]
        public InputSchemaContainer _actionSchema;

        #endregion


        #region METHODS

        /// <summary>
        /// Create an instance for this component
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Allow get to the game get the current input scheme
        /// if you need change it during gameplay, create a method to update!
        /// if you need update, get all interactable objects and restart the EnableInputSchema, but first change the new theme here!
        /// </summary>
        /// <returns></returns>
        public InputSchema GetInputSchema()
        {
            return schema;
        }

        public InputSchemaContainer GetInputSchemaContainer(int actionID)
        {
            _actionSchema.GetSchema(actionID);
            return _actionSchema;
        }

        #endregion
    }
}
