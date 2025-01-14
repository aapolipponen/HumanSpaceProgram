﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityPlus.AssetManagement
{
#if UNITY_EDITOR
    [CustomPropertyDrawer( typeof( AssetRegisterer.Entry ) )]
    public class EntryDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            EditorGUI.BeginProperty( position, label, property );

            // Calculate the width for each field
            float labelWidth = 0.666666f;

            // Create rectangles for the two fields
            Rect assetIDRect = new Rect( position.x, position.y, (position.width * labelWidth), position.height );
            Rect assetRect = new Rect( position.x + (position.width * labelWidth), position.y, (position.width * (1 - labelWidth)), position.height );

            // Get the serialized properties for the two fields
            SerializedProperty assetIDProperty = property.FindPropertyRelative( nameof( AssetRegisterer.Entry.assetID ) );
            SerializedProperty assetProperty = property.FindPropertyRelative( nameof( AssetRegisterer.Entry.asset ) );

            // Draw the two fields side by side without labels
            EditorGUI.PropertyField( assetIDRect, assetIDProperty, GUIContent.none );
            EditorGUI.PropertyField( assetRect, assetProperty, GUIContent.none );

            EditorGUI.EndProperty();
        }
    }
#endif

    /// <summary>
    /// Registers a specific set of assets (Unity Objects) when its first initialized.
    /// </summary>
    [DefaultExecutionOrder( int.MinValue )]
    public class AssetRegisterer : MonoBehaviour
    {
        /// <summary>
        /// Describes a specific `asset ID - asset reference` pair.
        /// </summary>
        [Serializable]
        public struct Entry
        {
            /// <summary>
            /// The asset ID to register under.
            /// </summary>
            public string assetID;

            /// <summary>
            /// The asset to register.
            /// </summary>
            public UnityEngine.Object asset;
        }

        [SerializeField]
        private Entry[] _assetsToRegister;

        public void TrySetAssetsToRegister( Entry[] assetsToRegister )
        {
#if UNITY_EDITOR
            if( !Application.isPlaying )
            {
                _assetsToRegister = assetsToRegister;
                return;
            }
#endif
            Debug.LogWarning( $"{nameof( AssetRegisterer )} - Tried to set `{nameof( _assetsToRegister )}` while in play mode." );
        }

        void Awake()
        {
            if( _assetsToRegister == null )
            {
                return;
            }

            foreach( var entry in _assetsToRegister )
            {
                if( entry.assetID == null )
                {
                    Debug.LogWarning( $"Null Asset ID present in the list of Assets to register (Asset: {entry.asset})." );
                    continue;
                }
                if( entry.asset == null )
                {
                    Debug.LogWarning( $"Null Asset present in the list of Assets to register (Asset ID: {entry.asset})." );
                    continue;
                }
                AssetRegistry.Register( entry.assetID, entry.asset );
            }

            // Allows to garbage collect them later, if unloaded from the registry.
            _assetsToRegister = null;
        }
    }
}
