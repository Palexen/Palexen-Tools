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
using Palexen.Gameplay.Input;

namespace Palexen.Scriptables
{
    [ScriptDescription("Input Schema Container", "Scriptable")]
    [CreateAssetMenu(fileName = "New Input Schema", menuName = "Palexen/UI/Input Schema")]
    public class InputSchemaContainer : ScriptableObject
    {
        #region VARIABLES

        [MyHeader("Custom UI Input Schema")]
        public string _schemaName;
        [Header("Setup your custom UI Schema controller")]
        public ActionVariables[] buttonSchemas;
        ActionVariables _uiVariableResult;

        #endregion

        #region METHODS

        public ActionVariables GetSchema(int SchemaID)
        {
            _uiVariableResult = buttonSchemas[SchemaID];

            return _uiVariableResult;
        }

        #endregion
    }
}