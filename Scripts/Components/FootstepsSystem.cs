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
using Palexen.Audio;
using Palexen.Scriptables;

namespace Palexen.Gameplay.Player
{
    [ScriptDescription("Footsteps", "Dynamic footsteps for gameplay")]
    [AddComponentMenu("Palexen/Gameplay/Footsteps System")]
    public class FootstepsSystem : MonoBehaviour
    {
#if PALEXEN_TOOLS
        [MyHeader("Config")]
#else
        [Header("Config")]
#endif

        public SurfaceType surfaceBehaviour;
        public LayerMask meshLayerMask;
        public LayerMask terrainLayerMask;
        public FootstepsSurface currentSurface;
        [FieldColor(FieldPropertyColor.orange, ShowObjectMessage.errorMessage)] public AudioSource foots;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] public AudioLibrary concrete;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] public AudioLibrary grass;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] public AudioLibrary water;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] public AudioLibrary glass;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] public AudioLibrary wood;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] public AudioLibrary gravel;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] public AudioLibrary rock;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] public AudioLibrary sand;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] public AudioLibrary dirt;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] public AudioLibrary snow;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] public AudioLibrary mud;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] public AudioLibrary metal;

        [MyHeader("Terrain Settings")]
        public int terrainTextureIndex;
        [FieldColor(FieldPropertyColor.salmon, ShowObjectMessage.warningMessage)] public PlayerTerrainSurfaceSettings terrainSurfaceSettings;

        [MyHeader("Misc")]
        [FieldColor(FieldPropertyColor.orange, ShowObjectMessage.warningMessage)] public AudioSource voice;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] public AudioLibrary climb;

        private void Update()
        {
            EvaluateSurface();

            switch (surfaceBehaviour)
            {
                case SurfaceType.mesh:
                    UpdateMeshSurface();
                    break;
                default:

                    case SurfaceType.terrain:
                    UpdateTerrainSurface();
                    break;
            }
        }

        void EvaluateSurface()
        {
            RaycastHit hit;

            if(Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity, meshLayerMask, QueryTriggerInteraction.Collide))
            {
                surfaceBehaviour = SurfaceType.mesh;
            }
            else if(Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity, terrainLayerMask, QueryTriggerInteraction.Collide))
            {
                surfaceBehaviour = SurfaceType.terrain;
            }
        }

        void UpdateMeshSurface()
        {
            RaycastHit hit;

            if(Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity, meshLayerMask, QueryTriggerInteraction.Collide))
            {
                if (hit.collider.CompareTag("Concrete"))
                {
                    currentSurface = FootstepsSurface.concrete;
                }

                if (hit.collider.CompareTag("Grass"))
                {
                    currentSurface = FootstepsSurface.grass;
                }

                if (hit.collider.CompareTag("Water"))
                {
                    currentSurface = FootstepsSurface.water;
                }

                if (hit.collider.CompareTag("Glass"))
                {
                    currentSurface = FootstepsSurface.glass;
                }

                if (hit.collider.CompareTag("Wood"))
                {
                    currentSurface = FootstepsSurface.wood;
                }

                if (hit.collider.CompareTag("Gravel"))
                {
                    currentSurface = FootstepsSurface.gravel;
                }

                if (hit.collider.CompareTag("Rock"))
                {
                    currentSurface = FootstepsSurface.rock;
                }

                if (hit.collider.CompareTag("Sand"))
                {
                    currentSurface = FootstepsSurface.sand;
                }

                if (hit.collider.CompareTag("Dirt"))
                {
                    currentSurface = FootstepsSurface.dirt;
                }

                if (hit.collider.CompareTag("Snow"))
                {
                    currentSurface = FootstepsSurface.snow;
                }

                if (hit.collider.CompareTag("Mud"))
                {
                    currentSurface = FootstepsSurface.mud;
                }

                if (hit.collider.CompareTag("Metal"))
                {
                    currentSurface = FootstepsSurface.metal;
                }
            }
        }

        void UpdateTerrainSurface()
        {
            Terrain terrain = Terrain.activeTerrain;
            if (terrain == null) return;

            TerrainData terrainData = terrain.terrainData;
            Vector3 terrainPos = terrain.transform.position;

            int mapX = Mathf.FloorToInt((transform.position.x - terrainPos.x) / terrainData.size.x * terrainData.alphamapWidth);
            int mapZ = Mathf.FloorToInt((transform.position.z - terrainPos.z) / terrainData.size.z * terrainData.alphamapHeight);

            if (mapX < 0 || mapZ < 0 || mapX >= terrainData.alphamapWidth || mapZ >= terrainData.alphamapHeight)
            {
                //Debug.LogWarning("Posición fuera del rango del mapa de splats.");
                return;
            }

            float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
            if (splatmapData.GetLength(2) == 0)
            {
                Debug.LogWarning("El terreno no tiene texturas asignadas.");
                return;
            }

            float[] weights = new float[splatmapData.GetLength(2)];

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = splatmapData[0, 0, i];
            }

            int dominantTextureIndex = GetDominantTextureIndex(weights);

            AssignSurfaceByTextureIndex(dominantTextureIndex);
        }


        int GetDominantTextureIndex(float[] weights)
        {
            terrainTextureIndex = 0;
            float maxWeight = 0;

            for (int i = 0; i < weights.Length; i++)
            {
                if (weights[i] > maxWeight)
                {
                    maxWeight = weights[i];
                    terrainTextureIndex = i;
                }
            }
            return terrainTextureIndex;
        }

        void AssignSurfaceByTextureIndex(int index)
        {
            currentSurface = terrainSurfaceSettings.terrainSurfaceSettings[index].surfaceType;
        }

        public void PlayFootstep()
        {
            switch (currentSurface)
            {
                case FootstepsSurface.concrete:

                    if (concrete != null)
                    {
                        if (foots != null)
                        {
                            foots.PlayOneShot(concrete.sounds[Random.Range(0, concrete.sounds.Length)]);
                        }
                    }
                    break;
                default:

                case FootstepsSurface.grass:

                    if (grass != null)
                    {
                        if (foots != null)
                        {
                            foots.PlayOneShot(grass.sounds[Random.Range(0, grass.sounds.Length)]);
                        }
                    }
                    break;

                case FootstepsSurface.water:

                    if (water != null)
                    {
                        if (foots != null)
                        {
                            foots.PlayOneShot(water.sounds[Random.Range(0, water.sounds.Length)]);
                        }
                    }
                    break;

                case FootstepsSurface.glass:

                    if (glass != null)
                    {
                        if (foots != null)
                        {
                            foots.PlayOneShot(glass.sounds[Random.Range(0, glass.sounds.Length)]);
                        }
                    }
                    break;

                case FootstepsSurface.wood:

                    if (wood != null)
                    {
                        if (foots != null)
                        {
                            foots.PlayOneShot(wood.sounds[Random.Range(0, wood.sounds.Length)]);
                        }
                    }
                    break;

                case FootstepsSurface.gravel:

                    if (water != null)
                    {
                        if (foots != null)
                        {
                            foots.PlayOneShot(gravel.sounds[Random.Range(0, gravel.sounds.Length)]);
                        }
                    }
                    break;

                case FootstepsSurface.rock:

                    if (water != null)
                    {
                        if (foots != null)
                        {
                            foots.PlayOneShot(rock.sounds[Random.Range(0, rock.sounds.Length)]);
                        }
                    }
                    break;

                case FootstepsSurface.sand:

                    if (water != null)
                    {
                        if (foots != null)
                        {
                            foots.PlayOneShot(sand.sounds[Random.Range(0, sand.sounds.Length)]);
                        }
                    }
                    break;

                case FootstepsSurface.dirt:

                    if (water != null)
                    {
                        if (foots != null)
                        {
                            foots.PlayOneShot(dirt.sounds[Random.Range(0, dirt.sounds.Length)]);
                        }
                    }
                    break;

                case FootstepsSurface.snow:

                    if (water != null)
                    {
                        if (foots != null)
                        {
                            foots.PlayOneShot(snow.sounds[Random.Range(0, snow.sounds.Length)]);
                        }
                    }
                    break;

                case FootstepsSurface.mud:

                    if (water != null)
                    {
                        if (foots != null)
                        {
                            foots.PlayOneShot(mud.sounds[Random.Range(0, mud.sounds.Length)]);
                        }
                    }
                    break;

                case FootstepsSurface.metal:

                    if (water != null)
                    {
                        if (foots != null)
                        {
                            foots.PlayOneShot(metal.sounds[Random.Range(0, metal.sounds.Length)]);
                        }
                    }
                    break;
            }
        }

        public void ClimbSound()
        {
            if (climb != null)
            {
                if (voice != null)
                {
                    voice.PlayOneShot(climb.sounds[Random.Range(0, climb.sounds.Length)]);
                }
            }
        }
    }
}
