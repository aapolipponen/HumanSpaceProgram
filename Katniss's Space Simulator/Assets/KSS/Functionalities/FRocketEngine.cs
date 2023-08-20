using KSS.Core;
using KSS.Core.ResourceFlowSystem;
using KSS.Control;
using System;
using UnityEngine;
using UnityPlus.Serialization;

namespace KSS.Functionalities
{
    [Serializable]
    public class FRocketEngine : MonoBehaviour, IResourceConsumer, IPersistent
    {
        [field: SerializeField]
        public float MaxGimbalAngle { get; set; } = 5f; // 5 degrees as an example.

        private float gimbalYawAngle = 0f;
        private float gimbalPitchAngle = 0f;

        const float ISP_TO_EXVEL = 9.80665f;

        float _currentThrust;

        /// <summary>
        /// The maximum thrust of the engine, in [N].
        /// </summary>
        [field: SerializeField]
        public float MaxThrust { get; set; } = 10000f;

        /// <summary>
        /// Specific impulse of the engine, in [s].
        /// </summary>
        [field: SerializeField]
        public float Isp { get; set; } = 100f; // TODO - curve based on atmospheric pressure.

        /// <summary>
        /// Maximum mass flow (at max thrust), in [kg/s]
        /// </summary>
        public float MaxMassFlow => MaxThrust / (Isp * ISP_TO_EXVEL);

        [field: SerializeField]
        public float Throttle { get; set; }

        public bool engineEnabled = false;

        [field: SerializeField]
        public float ThrottleIncrement = 0.1f;

        /// <summary>
        /// Defines which way the engine thrusts (thrust is applied along its `forward` (positive) axis).
        /// </summary>
        [field: SerializeField]
        public Transform ThrustTransform { get; set; }

        [field: SerializeField]
        public SubstanceStateCollection Inflow { get; private set; } = SubstanceStateCollection.Empty;

        Part _part;

        /// <summary>
        /// Returns the actual thrust at this moment in time.
        /// </summary>
        public float GetThrust( float massFlow )
        {
            return (this.Isp * ISP_TO_EXVEL) * massFlow * Throttle;
        }

        [ControlIn( "set.throttle", "Set Throttle" )]
        public void SetThrottle( float value )
        {
            this.Throttle = Mathf.Clamp(value, 0.0f, 1.0f);
        }

        public void StartEngine()
        {
            engineEnabled = true;
        }

        public void StopEngine()
        {
            engineEnabled = false;
        }

        public void IncreaseThrottle(float increment)
        {
            Throttle += increment;
            if (Throttle > 1.0f)
            {
                Throttle = 1.0f;
            }
            Debug.Log($"Increased throttle for engine: {name}. Current throttle: {Throttle}");
        }

        public void DecreaseThrottle(float decrement)
        {
            Throttle -= decrement;
            if (Throttle < 0.0f)
            {
                Throttle = 0.0f;
            }
            Debug.Log($"Decreased throttle for engine: {name}. Current throttle: {Throttle}");
        }

        public void ToggleEngine()
        {
            if (engineEnabled)
            {
                StopEngine();
            }
            else
            {
                StartEngine();
            }
        }

        private void Awake()
        {
            _part = this.GetComponent<Part>();
            if( _part == null )
            {
                Destroy( this );
                throw new InvalidOperationException( $"{nameof( FRocketEngine )} can only be added to a part." );
            }
        }

        void FixedUpdate()
        {
            float thrust = GetThrust(Inflow.GetMass());
            if (this.Throttle > 0.0f)
            {
                Vector3 gimbaledDirection = Quaternion.Euler(gimbalPitchAngle, gimbalYawAngle, 0f) * this.ThrustTransform.forward;
                this._part.Vessel.PhysicsObject.AddForceAtPosition(gimbaledDirection * thrust, this.ThrustTransform.position);
                Debug.Log($"Engine {this.name}: Applied thrust {thrust} in direction {gimbaledDirection}");
            }
            _currentThrust = thrust;
            Debug.Log($"Engine {this.name}: Current thrust = {_currentThrust}");
        }        

        public void ClampIn( SubstanceStateCollection inflow, float dt )
        {
            FlowUtils.ClampMaxMassFlow( inflow, Inflow.GetMass(), MaxMassFlow * Throttle );
        }

        public FluidState Sample( Vector3 localPosition, Vector3 localAcceleration, float holeArea )
        {
            return FluidState.Vacuum; // temp, inlet condition.
        }

        public void SetData( ILoader l, SerializedData data )
        {
            throw new NotImplementedException();
        }

        public SerializedData GetData( ISaver s )
        {
            throw new NotImplementedException();
        }
        public void AdjustGimbalYaw(float angleDelta)
        {
            gimbalYawAngle += angleDelta;
            ClampGimbalAngle();
            Debug.Log($"Engine {this.name}: Adjusted Yaw angle by {angleDelta}. New Yaw angle = {gimbalYawAngle}");
        }

        public void AdjustGimbalPitch(float angleDelta)
        {
            gimbalPitchAngle += angleDelta;
            ClampGimbalAngle();
            Debug.Log($"Engine {this.name}: Adjusted Pitch angle by {angleDelta}. New Pitch angle = {gimbalPitchAngle}");
        }

        private void ClampGimbalAngle()
        {
            float oldYaw = gimbalYawAngle;
            float oldPitch = gimbalPitchAngle;
            gimbalYawAngle = Mathf.Clamp(gimbalYawAngle, -MaxGimbalAngle, MaxGimbalAngle);
            gimbalPitchAngle = Mathf.Clamp(gimbalPitchAngle, -MaxGimbalAngle, MaxGimbalAngle);
            if (oldYaw != gimbalYawAngle || oldPitch != gimbalPitchAngle)
            {
                Debug.Log($"Engine {this.name}: Clamped gimbal angles. Yaw = {gimbalYawAngle}, Pitch = {gimbalPitchAngle}");
            }
        }
    }
}