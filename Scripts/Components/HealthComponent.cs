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
using UnityEngine.Events;

#if PALEXEN_TOOLS
using Palexen.Tools;
#endif

namespace Palexen.Gameplay
{
#if PALEXEN_TOOLS
    [ScriptDescription("Health Component", "It manages the HP of this object and handles events that occur when it is affected")]
#endif
    [AddComponentMenu("Palexen/Gameplay/Health Component")]
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        #region VARIABLES

        [MyHeader("Health Setup")]
        [VectorSlider(0, 1000)] public Vector2Int _health = new(0, 1000);
        [SerializeField] private int _healt;

        [VectorSlider(-1000, 0)] public Vector2Int _exceededThreshold = new(-1000, 0);
        [SerializeField] private int _exceededOn;
        public HealthCondition _affectsOn;
        public HealthImportance _importanceLevel;

        [HideInInspector] public bool showEvents = false;

        [MyHeader("Actions on start")]
        public UnityEvent _atStart;

        [MyHeader("Actions on Take Damage")]
        public UnityEvent _onTakeDamage;

        [MyHeader("Actions on Die")]
        public UnityEvent _atDie;

        [MyHeader("Actions on Exceeded")]
        public UnityEvent _atExceeded;

        [SerializeField][FieldColor(FieldPropertyColor.salmon, ShowObjectMessage.errorMessage)] public Transform _healthParent;

        [HideInInspector] public bool animationFeatures = false;
        [FieldColor(FieldPropertyColor.cyan, ShowObjectMessage.errorMessage)] public Animator _animator;
        public string[] triggerNames = { "Trigger0", "Trigger1" };

        bool alreadyInstantiated = false;
        bool isAlive = true;

        [HideInInspector] public bool showPresets = false;

        #endregion

        #region UNITY METHODS

        /// <summary>
        /// Initializes the component when the script instance is being loaded.
        /// </summary>
        /// <remarks>This method is called by Unity before the first frame update. Override this method to
        /// perform any setup required before the game starts.</remarks>
        void Start()
        {
            InvokeAtStart();
        }

        #endregion

        #region MECHANICS

        /// <summary>
        /// Initializes health and threshold values with random values within their defined ranges and invokes the start
        /// event.
        /// </summary>
        /// <remarks>Call this method at the beginning of the object's lifecycle to ensure that all
        /// dependent values are set and the start event is triggered. This method should typically be called once per
        /// object initialization.</remarks>
        void InvokeAtStart()
        {
            _healt = Random.Range(_health.x, _health.y);
            _exceededOn = Random.Range(_exceededThreshold.x, _exceededThreshold.y);

            _atStart.Invoke();
        }

        /// <summary>
        /// Invokes the end-of-life logic for the current object, triggering any registered callbacks and, if
        /// applicable, notifying the parent damageable component.
        /// </summary>
        /// <remarks>If the object's importance level is set to important, this method attempts to call
        /// the Die method on a parent component that implements IDamageable. If no such component is found, a warning
        /// is logged. This method is typically used to handle cleanup or death-related events in health or damage
        /// systems.</remarks>
        void InvokeEnd()
        {
            _atDie.Invoke();

            if(_importanceLevel == HealthImportance.important)
            {
                try
                {
                    _healthParent.TryGetComponent(out IDamageable damageable);
                    damageable.Die();

                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("No parent with IDamageable found to add health to. Exception: " + e);
                }
            }
        }

        /// <summary>
        /// Invokes the associated event when a predefined limit or condition has been exceeded.
        /// </summary>
        /// <remarks>Use this method to notify subscribers that a predefined limit or condition has been exceeded. 
        /// Ensure that there are subscribers to the event before calling this method to avoid exceptions.</remarks>
        void InvokeExceeded()
        {
            _atExceeded.Invoke();
        }

        #endregion

        #region API

        #region INTERFACE METHODS

        /// <summary>
        /// Attempts to add health to the parent object if it implements the IDamageable interface.
        /// </summary>
        /// <remarks>If the parent object does not implement IDamageable, no health is added and a warning
        /// is logged. This method is typically used to restore or increase the health of a parent component in a
        /// component-based architecture.</remarks>
        public void AddHealthAtParent()
        {
            try
            {
                _healthParent.TryGetComponent(out IDamageable damageable);
                damageable.AddHealth(_healt);

            }
            catch (System.Exception e)
            {
                Debug.LogWarning("No parent with IDamageable found to add health to. Exception: " + e);
            }
        }
     
        /// <summary>
        /// Adds the specified amount of health to the current value.
        /// </summary>
        /// <param name="health">The amount of health to add. Must be a positive integer.</param>
        public void AddHealth(int health)
        {

        }

        /// <summary>
        /// Applies damage to the current object, reducing its health by the specified amount. If configured, also
        /// propagates the damage to a parent object implementing IDamageable.
        /// </summary>
        /// <remarks>If the object's health falls below zero, the object is considered dead and the Die
        /// method is called. If the health falls below or equals a configured threshold, the Exceeded method is
        /// invoked. When HealthCondition is set to parent, the method attempts to propagate the damage to a parent
        /// object that implements IDamageable. If no such parent exists, a warning is logged.</remarks>
        /// <param name="damageAmount">The amount of damage to apply. Must be a non-negative integer.</param>
        public void TakeDamage(int damageAmount)
        {
            _healt -= damageAmount;

            if (_affectsOn == HealthCondition.parent)
            {
                try
                {
                    _healthParent.TryGetComponent(out IDamageable damageable);
                    damageable.TakeDamage(damageAmount);

                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("No parent with IDamageable found to add health to. Exception: " + e);
                }
            }

            if (isAlive)
            {
                _onTakeDamage.Invoke();
            }

            if ( _healt < 0)
            {
                Die();
                isAlive = false;
            }

            if(_healt <= _exceededOn)
            {
                Exceeded();
            }
        }

        /// <summary>
        /// Performs the necessary actions to terminate the current instance and trigger any end-of-life logic.
        /// </summary>
        public void Die()
        {
            InvokeEnd();
        }

        /// <summary>
        /// Notifies that a limit or threshold defined by the class logic has been exceeded.
        /// </summary>
        public void Exceeded()
        {
            InvokeExceeded();
        }

        #endregion

        #region UTILITY METHODS

        /// <summary>
        /// Sets the local scale of the object's transform to zero, effectively making the body part invisible or
        /// removed from the scene.
        /// </summary>
        /// <remarks>This method can be used to visually remove a body part from the scene without
        /// destroying the GameObject. The object remains in the hierarchy and can be reactivated by adjusting its
        /// scale.</remarks>
        public void CutBodyPart()
        {
            transform.localScale = Vector3.zero;
        }

        /// <summary>
        /// Detaches the specified GameObject from its parent in the scene hierarchy.
        /// </summary>
        /// <remarks>After calling this method, the GameObject will become a root object in the scene
        /// hierarchy. This operation does not destroy the GameObject or its children.</remarks>
        /// <param name="go">The GameObject to detach from its parent. Cannot be null.</param>
        public void DetachGO(GameObject go)
        {
            go.transform.parent = null;
        }

        /// <summary>
        /// Instantiates the specified GameObject at the current transform's position and rotation if it has not already
        /// been instantiated.
        /// </summary>
        /// <remarks>This method ensures that the GameObject is instantiated only once. Subsequent calls
        /// have no effect if the object has already been instantiated.</remarks>
        /// <param name="target">The GameObject to instantiate at the current transform's position and rotation. Cannot be null.</param>
        public void InstantiateThisGameObject(GameObject target)
        {
            if(!alreadyInstantiated)
            {
                Instantiate(target, transform.position, transform.rotation);
                alreadyInstantiated = true;
            }
        }

        /// <summary>
        /// Triggers a random animation by selecting a trigger name from the available set and activating it on the
        /// associated animator, if animation features are enabled.
        /// </summary>
        /// <remarks>This method has no effect if animation features are disabled. The specific animation
        /// triggered is chosen at random from the configured trigger names. Ensure that the animator and trigger names
        /// are properly initialized before calling this method to avoid runtime errors.</remarks>
        public void TriggerAnimations()
        {
            if (animationFeatures)
            {
                string triggerName = triggerNames[Random.Range(0, triggerNames.Length)];
                _animator.SetTrigger(triggerName);
            }
        }

        #endregion

        #region EXAMPLES

        /// <summary>
        /// Sets predefined example values ​​for head-related properties in the context of
        /// the current object.
        /// </summary>
        /// <remarks>Use this method to quickly assign example values to the head's health
        /// and exceeded threshold fields, typically for testing or debugging purposes in the editor.</remarks>
        [ContextMenu("Preset Head")]
        public void HeadExample()
        {
            _health.x = 25; _health.y = 52;
            _exceededThreshold.x = -170; _exceededThreshold.y = -129;
        }

        /// <summary>
        /// Sets example values for the health and threshold fields to demonstrate a preset body part configuration.
        /// </summary>
        /// <remarks>This method is intended for use in the Unity Editor to quickly assign sample values
        /// for testing or demonstration purposes. It does not affect runtime behavior outside of the editor
        /// context.</remarks>
        [ContextMenu("Preset Body Part")]
        public void BodyPartExample()
        {
            _health.x = 50; _health.y = 143;
            _exceededThreshold.x = -121; _exceededThreshold.y = -32;
        }

        /// <summary>
        /// Sets example values for the health and threshold fields to demonstrate a preset chest configuration.
        /// </summary>
        /// <remarks>Use this method to quickly restore the chest's state to standard preset values,
        /// typically for testing or demonstration purposes. This method is intended to be invoked from the Unity Editor
        /// context menu.</remarks>
        [ContextMenu("Preset Chest")]
        public void ChestExample()
        {
            _health.x = 200; _health.y = 400;
            _exceededThreshold.x = -946; _exceededThreshold.y = -786;
        }

        #endregion

        #endregion
    }
}
