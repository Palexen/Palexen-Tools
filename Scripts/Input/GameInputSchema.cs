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
        [FieldColor(FieldPropertyColor.salmon, ShowObjectMessage.errorMessage)] public InputSchemaContainer _actionSchema;

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

        /// <summary>
        /// Gets the input schema container associated with the specified action identifier.
        /// </summary>
        /// <param name="actionID">The identifier of the action for which to retrieve the input schema container.</param>
        /// <returns>An <see cref="InputSchemaContainer"/> instance containing the input schema for the specified action.</returns>
        public InputSchemaContainer GetInputSchemaContainer(int actionID)
        {
            _actionSchema.GetSchema(actionID);
            return _actionSchema;
        }

        #endregion

        #region API

        /// <summary>
        /// Sets the input schema used by the current instance.
        /// </summary>
        /// <param name="newSchema">The new input schema to apply. Cannot be null.</param>
        public void SetSchema(InputSchema newSchema)
        {
            schema = newSchema;
        }

        #endregion
    }
}
