﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KSS.Core.Managers
{
    /// <summary>
    /// Manages the speed at which the time flows.
    /// </summary>
    public class TimeWarpManager : MonoBehaviour
    {
        public struct TimeScaleChangedData
        {
            /// <summary>
            /// The old timescale (before it was updated).
            /// </summary>
            public float Old { get; set; }
            /// <summary>
            /// The new timescale (after it was updated).
            /// </summary>
            public float New { get; set; }
        }

        static float _maxTimeScale = 128.0f; // VS is being dumb, it is read inside GetMaxTimescale() (but only when running outside of unity editor).

        /// <summary>
        /// Gets the current maximum timescale.
        /// </summary>
        public static float GetMaxTimescale()
        {
#if UNITY_EDITOR
            return 100f;
#else
            return _maxTimeScale;
#endif
        }

        /// <summary>
        /// Sets the current maximum timescale.
        /// </summary>
        public static void SetMaxTimeScale( float value )
        {
#if UNITY_EDITOR
            if( value > 100f )
            {
                Debug.LogWarning( $"Inside Unity Editor, timescale can be at most 100 :(." );
                value = 100f;
            }
#endif
            _maxTimeScale = value;
        }

        private static float _timeScale;

        /// <summary>
        /// Checks if the game is currently paused.
        /// </summary>
        public static bool IsPaused { get => _timeScale == 0.0f; }

        /// <summary>
        /// Invoked when the timescale is changed by the <see cref="TimeWarpManager"/>.
        /// </summary>
        public static event Action<TimeScaleChangedData> OnTimescaleChanged;

        /// <summary>
        /// Pauses the game, sets the timescale to 0.
        /// </summary>
        public static void Pause()
        {
            SetTimeScale( 0.0f );
        }

        /// <summary>
        /// Gets the current value of the timescale.
        /// </summary>
        public static float GetTimeScale()
        {
            return _timeScale;
        }

        /// <summary>
        /// Sets the timescale to the specified value.
        /// </summary>
        public static void SetTimeScale( float timeScale )
        {
            if( timeScale < 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( timeScale ), $"Timescale must be greater or equal to 0." );
            }
            float max = GetMaxTimescale();
            if( timeScale > max )
            {
                throw new ArgumentOutOfRangeException( nameof( timeScale ), $"Timescale must be smaller or equal to maximum timescale (currently {max})." );
            }

            float oldTimeScale = _timeScale;
            _timeScale = timeScale;
            Time.timeScale = timeScale;
            OnTimescaleChanged?.Invoke( new TimeScaleChangedData() { Old = oldTimeScale, New = timeScale } );
        }

        void Start()
        {
            SetTimeScale( 1 );
        }

        void Update()
        {
#warning TODO - When saving, the game needs to be paused and not be unpaused until saving ends.
            if( Input.GetKeyDown( KeyCode.Period ) )
            {
                if( IsPaused )
                {
                    SetTimeScale( 1f );
                    return;
                }
                float newscale = _timeScale * 2f;

                if( newscale > GetMaxTimescale() )
                    return;
                SetTimeScale( newscale );
            }

            if( Input.GetKeyDown( KeyCode.Comma ) )
            {
                if( _timeScale <= 1f )
                    Pause();
                else
                    SetTimeScale( _timeScale / 2f );
            }
        }
    }
}