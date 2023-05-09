﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KatnisssSpaceSimulator.Core.ReferenceFrames
{
    /// <summary>
    /// An arbitrary reference frame.
    /// </summary>
    public interface IReferenceFrame
    {
        // "Absolute Inertial Reference Frame" (AIRF) is my invention.
        // It represents the absolute world space.
        // We can't use Unity's world space for that because of 32-bit float precision issues. So instead, we make the worldspace act like the current world space reference frame.




        // Bottom line is that we need to make the Unity's world space act like the local space of the selected reference frame.

        // frames of reference can be used for that.

        // a rotating frame of reference will impart forces on the object just because it is rotating.
        // the reference frame needs to keep high precision position / rotation of the reference object.
        // - Every object's position will be transformed by this frame to get its "true" position, which might have arbitrary precision (and in reverse too, inverse to get local position).

        // There is only one global reference frame for the scene.

        // objects that are not centered on the reference frame need to be updated every frame (possibly less if they're very distant and can't be seen) to remain correct.
        // - if the frame is centered on the active vessel, then "world" space in Unity needs to be transformed into local space for that frame.
        // - - This can be done by applying forces/changing positions manually.

        /// <summary>
        /// Returns a new reference frame that is shifted by a given amount in the Absolute Inertial Reference Frame (AIRF) space.
        /// </summary>
        IReferenceFrame Shift( Vector3Dbl vector );

        /// <summary>
        /// Transforms a point in the reference frame's space to the Absolute Inertial Reference Frame (AIRF) space.
        /// </summary>
        Vector3Dbl TransformPosition( Vector3 localPosition );

        /// <summary>
        /// Transforms a point in the Absolute Inertial Reference Frame (AIRF) space to the reference frame's space.
        /// </summary>
        Vector3 InverseTransformPosition( Vector3Dbl globalPosition );

        /// <summary>
        /// Transforms a direction in the reference frame's space to the Absolute Inertial Reference Frame (AIRF) space.
        /// </summary>
        Vector3 TransformDirection( Vector3 localDirection );

        /// <summary>
        /// Transforms a direction in the Absolute Inertial Reference Frame (AIRF) space to the reference frame's space.
        /// </summary>
        Vector3 InverseTransformDirection( Vector3 globalDirection );

        /// <summary>
        /// Transforms a rotation/orientation in the reference frame's space to the Absolute Inertial Reference Frame (AIRF) space.
        /// </summary>
        Quaternion TransformRotation( Quaternion localPosition );

        /// <summary>
        /// Transforms a rotation/orientation in the Absolute Inertial Reference Frame (AIRF) space to the reference frame's space.
        /// </summary>
        Quaternion InverseTransformRotation( Quaternion globalPosition );
    }
}