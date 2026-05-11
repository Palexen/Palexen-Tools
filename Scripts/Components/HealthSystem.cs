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
    [ScriptDescription("Health Game Object", "Manage the HP of this object; after it reaches 0, the object will " +
        "handle after-kill events, implemented in an event")]
#endif
    [AddComponentMenu("Palexen/Gameplay/Health System")]
    public class HealthSystem : MonoBehaviour, IDamageable
    {
        #region VARIABLES

        [MyHeader("Health Setup")]
        public HealthCondition _behaviour = HealthCondition.byChilds;
        [VectorSlider(0, 1000)] public Vector2Int _healthRange = new(0, 1000);
        [SerializeField] private int _totalHealth;

        [VectorSlider(-1000, 0)] public Vector2Int _exceededThreshold = new(-1000, 0);
        [SerializeField] private int _exceededOn;

        [HideInInspector] public bool showEventsGroup = false;

        [MyHeader("After Kill Events")]
        public UnityEvent _afterKillObject;

        [MyHeader("After Exceeded Events")]
        public UnityEvent _afterExceeded;

        [HideInInspector] public bool useAnimationFeatures;
        //[MyHeader("Animation Features")]
        [FieldColor(FieldPropertyColor.cyan, ShowObjectMessage.warningMessage)] public Animator _animator;
        public string[] dieTriggerNames = { "Die1", "Die2" };

        [MyHeader("Disable Components on Die")]
        public UnityEvent onFinishDieAnimations;

        bool isAlive = true;
        bool animationIsPlayed = false;

        [HideInInspector] public bool usePhysicsFeatures;
        //[Header("Ragdoll or Physics Features")]
        [FieldColor(FieldPropertyColor.neonGreen, ShowObjectMessage.errorMessage)] public Rigidbody[] _rigidbodies;

        [HideInInspector] public bool showVelocityLimiters = false;
        [HideInInspector] public bool showShapeSettings = false;


        #endregion

        #region UNITY METHODS

        /// <summary>
        /// Initializes the component when the script instance is being loaded.
        /// </summary>
        /// <remarks>This method is called by Unity before the first frame update. Override this method to
        /// perform any initialization logic required before gameplay begins.</remarks>
        void Start()
        {
            GetHealth();
        }

        #endregion

        #region MECHANICS

        /// <summary>
        /// Initializes or updates the health and exceeded threshold values based on the current health condition.
        /// </summary>
        /// <remarks>This method assigns random values within the specified health and exceeded threshold
        /// ranges. The resulting values depend on the current health condition and are used to determine the object's
        /// health state.</remarks>
        void GetHealth()
        {
            if (_behaviour == HealthCondition.single || _behaviour == HealthCondition.parent)
            {
                _totalHealth = Random.Range(_healthRange.x, _healthRange.y);
            }

            _exceededOn = Random.Range(_exceededThreshold.x, _exceededThreshold.y);
        }

        /// <summary>
        /// Invokes the action to be performed when a predefined limit or threshold has been exceeded.
        /// </summary>
        /// <remarks>Call this method to trigger any logic associated with exceeding the configured limit.
        /// The specific behavior depends on the action provided during initialization.</remarks>
        public void Exceeded()
        {
            _afterExceeded.Invoke();
        }

        #endregion

        #region API

        #region INTERFACE METHODS

        /// <summary>
        /// Adds the specified amount of health to the current total.
        /// </summary>
        /// <param name="health">The amount of health to add. Can be positive or negative to increase or decrease the total health.</param>
        public void AddHealth(int health)
        {
            _totalHealth += health;
        }

        /// <summary>
        /// Reduces the total health by the specified damage amount and triggers death logic if health falls below zero.
        /// </summary>
        /// <remarks>If the resulting health is less than zero, the method invokes death handling.
        /// Additional logic is executed if health falls below a defined threshold. Callers should ensure that the
        /// damage amount does not exceed the expected range for the entity.</remarks>
        /// <param name="damageAmount">The amount of damage to subtract from the total health. Must be a non-negative value.</param>
        public void TakeDamage(int damageAmount)
        {
            _totalHealth -= damageAmount;

            if (_totalHealth < 0)
            {
                Die();

                if(_totalHealth <= _exceededOn)
                {
                    Exceeded();
                }
            }
        }

        /// <summary>
        /// Marks the object as no longer alive and triggers any registered post-death actions.
        /// </summary>
        /// <remarks>Call this method to indicate that the object should transition to a non-active state.
        /// Any actions registered to occur after the object's death will be invoked. This method has no effect if
        /// called multiple times in succession.</remarks>
        public void Die()
        {
            isAlive = false;
            _afterKillObject.Invoke();
        }
        #endregion

        #region FEATURES

        /// <summary>
        /// Triggers a random die animation if one has not already been played.
        /// </summary>
        /// <remarks>This method selects a random trigger from the available die animation names and
        /// activates it on the associated animator. Subsequent calls have no effect if the die animation has already
        /// been triggered.</remarks>
        public void TriggerDieAnimations()
        {
            if (!animationIsPlayed)
            {
                if (_animator != null)
                {
                    _animator.SetTrigger(dieTriggerNames[Random.Range(0, dieTriggerNames.Length)]);
                    animationIsPlayed = true;
                }
            }
        }

        /// <summary>
        /// Disables relevant components by invoking the associated finish-die animation event.
        /// </summary>
        /// <remarks>Call this method to trigger any actions or cleanup associated with the end of a die
        /// animation. The specific components affected depend on the event handlers attached to the finish-die
        /// animation event.</remarks>
        public void DisableComponents()
        {
            onFinishDieAnimations.Invoke();
        }

        /// <summary>
        /// Enables ragdoll physics by allowing all associated rigidbodies to be affected by external forces.
        /// </summary>
        /// <remarks>Call this method to switch the object from an animated or controlled state to a
        /// physics-driven state. After calling this method, the object's rigidbodies will respond to collisions and
        /// forces according to the physics engine.</remarks>
        public void EnableRagdoll()
        {
            for (int i = 0; i < _rigidbodies.Length; i++)
            {
                _rigidbodies[i].isKinematic = false;
            }
        }

        /// <summary>
        /// Retrieves all Rigidbody components attached to this GameObject and its child GameObjects and stores them for
        /// later use.
        /// </summary>
        /// <remarks>This method is intended to be used in the Unity Editor via the context menu. It
        /// replaces any previously stored Rigidbody references with the current set found in the GameObject
        /// hierarchy.</remarks>
        [ContextMenu("Fetch Rigidbodies")]
        public void FetchRigidbodies()
        {
            _rigidbodies = GetComponents<Rigidbody>();
            _rigidbodies = GetComponentsInChildren<Rigidbody>();
        }

        /// <summary>
        /// Sets all associated rigid bodies in kinematic mode, disabling physical simulation on
        /// them.
        /// </summary>
        /// <remarks>Use this method to convert the ragdoll into an animation-controlled state or
        /// scripts, in which rigid bodies do not respond to physical forces. This is useful, for example, when
        /// alternate between animation and physical simulation in characters.</remarks>
        [ContextMenu("Mark as Kinematic Ragdoll")]
        public void KinematicRagdoll()
        {
            for (int i = 0; i < _rigidbodies.Length; i++)
            {
                _rigidbodies[i].isKinematic = true;
            }
        }

        #endregion

        #endregion
    }
}