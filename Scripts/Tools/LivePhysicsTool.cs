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
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Palexen.Scriptables;
using System.Collections.Generic;

namespace Palexen.CustomPhysics.Editor
{
    public class LivePhysicsTool
    {
        private const string V = "Environment Settings/Palexen Environment Settings";

        static bool UpdatePhysics;
        static LayerMask AllowedLayer = 7;

        static CustomEnvironment setting;
        static string customMessagePath = V;

        [MenuItem("Physics Simulation/Simulate Physics on Edit Mode %#T")]
        private static void ToggleOption()
        {
            UpdatePhysics = !UpdatePhysics;

            if (UpdatePhysics)
            {
                Debug.Log("<color=green>Physics Simulation</color>: " + UpdatePhysics);
            }
            else
            {
                Debug.Log("<color=yellow>Physics Simulation</color>: " + UpdatePhysics);
            }

            if (UpdatePhysics)
            {
                EditorApplication.update += EditorUpdate;
            }
            else
            {
                EditorApplication.update -= EditorUpdate;
            }
        }

        [MenuItem("Physics Simulation/Simulate Physics on Edit Mode %#T", true)]
        private static bool ToggleOptionValidate()
        {
            Menu.SetChecked("Physics Simulation/Simulate Physics on Edit Mode", UpdatePhysics);
            return true;
        }

        static void EditorUpdate()
        {
            if (!EditorApplication.isPlaying)
            {
                if (UpdatePhysics)
                {
                    SimulatePhysics();
                }
            }
        }

        static void SimulatePhysics()
        {
            if (EditorApplication.isPlaying)
                return;

            Rigidbody[] allBodies = Object.FindObjectsByType<Rigidbody>(
                FindObjectsSortMode.None);

            Dictionary<Rigidbody, bool> originalStates = new();

            if (setting == null)
            {
                setting = Resources.Load<CustomEnvironment>(customMessagePath);
            }

            AllowedLayer = setting.physicsSimulationLayer;

            foreach (Rigidbody rb in allBodies)
            {
                originalStates.Add(rb, rb.isKinematic);

                if ((AllowedLayer.value & (1 << rb.gameObject.layer)) == 0)
                {
                    rb.isKinematic = true;

                    if (rb.transform.hasChanged)
                    {
                        rb.linearVelocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;

                        rb.position = rb.transform.position;
                        rb.rotation = rb.transform.rotation;

                        rb.Sleep();
                        rb.WakeUp();

                        rb.transform.hasChanged = false;
                    }
                }
            }

            Physics.SyncTransforms();

#if UNITY_6000_0_OR_NEWER
            Physics.simulationMode = SimulationMode.Script;
#else
            Physics.autoSimulation = false;
#endif

            Physics.Simulate(Time.deltaTime);

#if UNITY_6000_0_OR_NEWER
            Physics.simulationMode = SimulationMode.Update;
#else
            Physics.autoSimulation = true;
#endif

            foreach (var pair in originalStates)
            {
                if (pair.Key != null)
                {
                    pair.Key.isKinematic = pair.Value;
                }
            }
        }
    }
}
#endif