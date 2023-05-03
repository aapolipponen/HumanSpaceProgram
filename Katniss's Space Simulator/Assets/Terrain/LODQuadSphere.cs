﻿using KatnisssSpaceSimulator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KatnisssSpaceSimulator.Terrain
{
    /// <summary>
    /// Wraps around 6 faces of a sphere.
    /// </summary>
    [RequireComponent( typeof( CelestialBody ) )]
    public class LODQuadSphere : MonoBehaviour
    {
        /// <summary>
        /// The number of binary subdivisions per edge of each of the quads.
        /// </summary>
        public int EdgeSubdivisions { get; private set; } = 4;

        /// <summary>
        /// The level of subdivision (lN) at which the quad will stop subdividing.
        /// </summary>
        public int HardLimitSubdivLevel { get; set; } = 18;

        LODQuadTree[] _quadTree;
        CelestialBody _celestialBody;

        public const float QUAD_RANGE_MULTIPLIER = 2.0f; // 3.0 makes all joints only between the same subdiv.

        void Awake()
        {
            // Possibly move this to a child, so it can be disabled without disabling entire CB.
            _celestialBody = this.GetComponent<CelestialBody>();
        }

        void Start()
        {
            zzzTestGameManager t = FindObjectOfType<zzzTestGameManager>();

            _quadTree = new LODQuadTree[6];
            for( int i = 0; i < 6; i++ )
            {
                Vector2 center = Vector2.zero;
                int lN = 0;

                _quadTree[i] = new LODQuadTree( new LODQuadTree.Node( null, center, LODQuadTree_NodeUtils.GetSize( lN ) ) );

#warning TODO - there is some funkiness with the collider physics (it acts as if the object was unparented (when unparenting, it changes scene position slightly)).


                Material mat = new Material( t.cbShader );
                mat.SetTexture( "_MainTex", t.cbTextures[i] );
                mat.SetFloat( "_Glossiness", 0.05f );
                mat.SetFloat( "_NormalStrength", 0.0f );

                var face = LODQuad.Create( _celestialBody.transform, ((Direction3D)i).ToVector3() * (float)_celestialBody.Radius, this, _celestialBody, center, lN, _quadTree[i].Root, (float)_celestialBody.Radius * QUAD_RANGE_MULTIPLIER, mat, (Direction3D)i );
            }
        }

        void Update()
        {
            foreach( var q in _quadTree )
            {
                foreach( var qq in q.GetNonNullLeafNodes() ) // This can be optimized for large numbers of subdivs.
                {
                    qq.AirfPOIs = new Vector3Dbl[] { VesselManager.ActiveVessel.AIRFPosition };
                }
            }
        }

        // ondestroy delete itself?
    }
}