using KSS.Control;
using System.Collections.Generic;
using UnityEngine;

namespace KSS.Functionalities
{
    public class FPlayerSteeringInput : MonoBehaviour
    {
        public List<FRocketEngine> engines = new List<FRocketEngine>();

        private float yawDelta = 1.0f;
        private float pitchDelta = 1.0f;

        private void Update()
        {
            // Capture the yaw input:
            float yawInput = 0.0f;
            if (Input.GetKey(KeyCode.A))
            {
                yawInput = -yawDelta;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                yawInput = yawDelta;
            }
    
            // Capture the pitch input:
            float pitchInput = 0.0f;
            if (Input.GetKey(KeyCode.W))
            {
                pitchInput = pitchDelta;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                pitchInput = -pitchDelta;
            }
    
            // Apply the input to each engine's gimbal:
            foreach (var engine in engines)
            {
                engine.AdjustGimbalYaw(yawInput);
                engine.AdjustGimbalPitch(pitchInput);
            }
        }
    }

}
