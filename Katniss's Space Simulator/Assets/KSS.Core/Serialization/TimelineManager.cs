using KSS.Core.TimeWarp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityPlus.OverridableEvents;
using UnityPlus.Serialization;
using UnityPlus.Serialization.Strategies;

namespace KSS.Core.Serialization
{
    /// <summary>
    /// Manages the currently loaded timeline (save/workspace). See <see cref="TimelineMetadata"/> and <see cref="SaveMetadata"/>.
    /// </summary>
    public static class TimelineManager
    {
        static readonly JsonPrefabAndDataStrategy _serializationStrat = new JsonPrefabAndDataStrategy();

        /// <summary>
        /// Checks if a timeline is currently being either saved or loaded.
        /// </summary>
        public static bool IsSavingOrLoading =>
            (_saver != null && _saver.CurrentState != ISaver.State.Idle)
         || (_loader != null && _loader.CurrentState != ILoader.State.Idle);

        public static TimelineMetadata CurrentTimeline { get; private set; }

        static AsyncSaver _saver;
        static AsyncLoader _loader;

        static bool _wasPausedBeforeSerializing = false;

        public static void SerializationPauseFunc()
        {
            _wasPausedBeforeSerializing = TimeWarpManager.IsPaused;
            TimeWarpManager.Pause();
            TimeWarpManager.LockTimescale = true;
        }

        public static void SerializationUnpauseFunc()
        {
            if( !_wasPausedBeforeSerializing )
            {
                TimeWarpManager.Unpause();
            }
            TimeWarpManager.LockTimescale = false;
        }

        static void CreateDefaultSaver()
        {
            _saver = new AsyncSaver(
                SerializationPauseFunc, SerializationUnpauseFunc,
                new Func<ISaver, IEnumerator>[] { _serializationStrat.SaveSceneObjects_Object },
                new Func<ISaver, IEnumerator>[] { _serializationStrat.SaveSceneObjects_Data }
            );
        }

        static void CreateDefaultLoader()
        {
            _loader = new AsyncLoader(
                SerializationPauseFunc, SerializationUnpauseFunc,
                new Func<ILoader, IEnumerator>[] { _serializationStrat.LoadSceneObjects_Object },
                new Func<ILoader, IEnumerator>[] { _serializationStrat.LoadSceneObjects_Data }
            );
        }

        /// <summary>
        /// Asynchronously saves the current game state over multiple frames. <br/>
        /// The game should remain paused for the duration of the saving (this is generally handled automatically, but be careful).
        /// </summary>
        public static void BeginSaveAsync( string timelineId, string saveId )
        {
            if( string.IsNullOrEmpty( timelineId ) && string.IsNullOrEmpty( saveId ) )
            {
                throw new ArgumentException( $"Both can't be null." );
            }
            if( IsSavingOrLoading )
            {
                throw new InvalidOperationException( $"Can't start saving while already saving/loading." );
            }

            CreateDefaultSaver();

            HSPEvent.EventManager.TryInvoke( HSPEvent.TIMELINE_BEFORE_SAVE, _saver );

            //
            // TODO - Save
            //

            HSPEvent.EventManager.TryInvoke( HSPEvent.TIMELINE_AFTER_SAVE, _saver );

            throw new NotImplementedException();
        }

        /// <summary>
        /// Asynchronously loads the saved game state over multiple frames. <br/>
        /// The game should remain paused for the duration of the loading (this is generally handled automatically, but be careful).
        /// </summary>
        public static void BeginLoadAsync( string timelineId, string saveId )
        {
            if( string.IsNullOrEmpty( timelineId ) && string.IsNullOrEmpty( saveId ) )
            {
                throw new ArgumentException( $"Both can't be null." );
            }
            if( IsSavingOrLoading )
            {
                throw new InvalidOperationException( $"Can't start loading while already saving/loading." );
            }

            TimelineMetadata loadedTimeline = TimelineMetadata.EmptyFromFilePath( TimelineMetadata.GetSavesPath( timelineId ) );

            CreateDefaultLoader();
            HSPEvent.EventManager.TryInvoke( HSPEvent.TIMELINE_BEFORE_LOAD, _loader );

            //
            // TODO - Load
            //
            CurrentTimeline = loadedTimeline;

            HSPEvent.EventManager.TryInvoke( HSPEvent.TIMELINE_AFTER_LOAD, _loader );

            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new default (empty) timeline and "loads" it.
        /// </summary>
        public static void CreateNew( string timelineId, string saveId )
        {
            if( string.IsNullOrEmpty( timelineId ) && string.IsNullOrEmpty( saveId ) )
            {
                throw new ArgumentException( $"Both can't be null." );
            }
            if( IsSavingOrLoading )
            {
                throw new InvalidOperationException( $"Can't start loading while already saving/loading." );
            }

            TimelineMetadata newTimeline = TimelineMetadata.EmptyFromFilePath( TimelineMetadata.GetSavesPath( timelineId ) );

            HSPEvent.EventManager.TryInvoke( HSPEvent.TIMELINE_BEFORE_NEW );

            CurrentTimeline = newTimeline;
            HSPEvent.EventManager.TryInvoke( HSPEvent.TIMELINE_AFTER_NEW );
        }
    }
}