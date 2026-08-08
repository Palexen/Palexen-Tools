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
using Palexen.Scriptables;

#if PALEXEN_TOOLS
using Palexen.Tools;
#endif
using System.Collections.Generic;

namespace Palexen.Levels
{
    #if PALEXEN_TOOLS
    [ScriptDescription("RegionInstancer", "Improved Monobehavior")]
#endif
    [AddComponentMenu("Palexen/Level Design/Region Instancer")]
    [ExecuteInEditMode]
    public class RegionInstancer : MonoBehaviour
    {
        #region VARIABLES

        [SerializeField] private RegionInstancerBehaviour _behaviour;
        [SerializeField] private RotationRandomizerBehaviour _randomizeRotation;
        [SerializeField] private int _maxInstances;
        [FieldColor(FieldPropertyColor.clearBlue, ShowObjectMessage.errorMessage)] [SerializeField] private PrefabCollection _prefabs;
        [SerializeField] private Bounds _bounds = new(Vector3.zero, Vector3.one);

        List<GameObject> _instancedObjects = new List<GameObject>();
        int currentInstances;
        bool instancing;

        #endregion

        #region PROPERTIES
        public Bounds Bounds { get { return new Bounds(_bounds.center, _bounds.size); } set { _bounds = value; } }
        public int CurrentInstances { get { return transform.childCount; } }

        #endregion

        #region UNITY METHODS

        void Update()
        {
            if(currentInstances >= _maxInstances)
            {
                instancing = false;
                currentInstances = 0;
            }

            if (instancing)
            {
                InstanceBoxVolume();
            }
        }

        #endregion

        #region MECHANICS

        void InstanceBoxVolume()
        {
            if (_prefabs == null || _prefabs.Prefabs.Length == 0)
            {
                Debug.LogError("No prefabs assigned to instance.");
                return;
            }

            while (currentInstances < _maxInstances)
            {
                foreach (GameObject prefab in _prefabs.Prefabs)
                {
                    if (currentInstances >= _maxInstances) break;

                    if (prefab == null) continue;

                    Vector3 spawnPosition = _behaviour switch
                    {
                        RegionInstancerBehaviour.volume => RandomPosition(),
                        RegionInstancerBehaviour.ground => RandomGround(),
                        RegionInstancerBehaviour.up => RandomUp(),
                        RegionInstancerBehaviour.left => RandomLeft(),
                        RegionInstancerBehaviour.right => RandomRight(),
                        RegionInstancerBehaviour.forward => RandomForward(),
                        RegionInstancerBehaviour.backward => RandomBackwards(),
                        _ => transform.position
                    };

                    GameObject clone = Instantiate(prefab, spawnPosition, RotationHandler(), transform);

                    _instancedObjects.Add(clone);
                    currentInstances++;
                }
            }
        }


        Vector3 RandomPosition()
        {
            float randomX = Random.Range(_bounds.min.x, _bounds.max.x);
            float randomY = Random.Range(_bounds.min.y, _bounds.max.y);
            float randomZ = Random.Range(_bounds.min.z, _bounds.max.z);

            Vector3 randomPosition = new(randomX, randomY, randomZ);

            return transform.TransformPoint(randomPosition);
        }

        Vector3 RandomGround()
        {
            float randomX = Random.Range(_bounds.min.x, _bounds.max.x);
            float randomZ = Random.Range(_bounds.min.z, _bounds.max.z);
            float groundY = _bounds.min.y;

            Vector3 randomPosition = new(randomX, groundY, randomZ);

            return transform.TransformPoint(randomPosition);
        }

        Vector3 RandomUp()
        {
            float randomX = Random.Range(_bounds.min.x, _bounds.max.x);
            float randomZ = Random.Range(_bounds.min.z, _bounds.max.z);
            float upY = _bounds.max.y;

            Vector3 randomPosition = new(randomX, upY, randomZ);

            return transform.TransformPoint(randomPosition);
        }

        Vector3 RandomLeft()
        {
            float leftX = _bounds.min.x;
            float randomY = Random.Range(_bounds.min.y, _bounds.max.y);
            float randomZ = Random.Range(_bounds.min.z, _bounds.max.z);

            Vector3 randomPosition = new(leftX, randomY, randomZ);

            return transform.TransformPoint(randomPosition);
        }

        Vector3 RandomRight()
        {
            float rightX = _bounds.max.x;
            float randomY = Random.Range(_bounds.min.y, _bounds.max.y);
            float randomZ = Random.Range(_bounds.min.z, _bounds.max.z);

            Vector3 randomPosition = new(rightX, randomY, randomZ);

            return transform.TransformPoint(randomPosition);
        }

        Vector3 RandomForward()
        {
            float randomX = Random.Range(_bounds.min.x, _bounds.max.x);
            float randomY = Random.Range(_bounds.min.y, _bounds.max.y);

            float forwardZ = _bounds.max.z;

            Vector3 randomPosition = new(randomX, randomY, forwardZ);

            return transform.TransformPoint(randomPosition);
        }

        Vector3 RandomBackwards()
        {
            float randomX = Random.Range(_bounds.min.x, _bounds.max.x);
            float randomY = Random.Range(_bounds.min.y, _bounds.max.y);
            float backwardZ = _bounds.min.z;

            Vector3 randomPosition = new(randomX, randomY, backwardZ);

            return transform.TransformPoint(randomPosition);
        }

        Quaternion RotationHandler()
        {
            var rot = _randomizeRotation switch
            {
                RotationRandomizerBehaviour.all => Random.rotation,
                RotationRandomizerBehaviour.up => Quaternion.Euler(0, Random.Range(0f, 360f), 0),
                RotationRandomizerBehaviour.no => Quaternion.identity,
                _ => Quaternion.identity,
            };
            return rot;
        }

        #endregion

        #region API

        [ContextMenu("Volume Instancing")]
        public void VolumeInstancing()
        {
            if (_prefabs == null || _prefabs.Prefabs.Length == 0)
            {
                Debug.LogWarning("No prefabs assigned to the RegionInstancer.");
                return;
            }

            instancing = true;
        }

        [ContextMenu("Delete Instances")]
        public void DeleteInstances()
        {
            foreach (GameObject obj in _instancedObjects)
            {
                if (obj != null)
                {
                    DestroyImmediate(obj);
                }
            }

            _instancedObjects.Clear();
            currentInstances = 0;
        }

        #endregion
    }
}
