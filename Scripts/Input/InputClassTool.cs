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
using Palexen.Tools;
using UnityEngine;
using System;

namespace Palexen.Gameplay.Input
{
    [Serializable]
    public class ActionVariables
    {
        public string schemaName;
        [FieldColor(FieldPropertyColor.pink, ShowObjectMessage.errorMessage)] public Sprite[] variants;
    }
}
