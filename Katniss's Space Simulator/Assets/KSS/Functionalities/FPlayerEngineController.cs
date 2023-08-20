using KSS.Control;
using System.Collections.Generic;
using UnityEngine;

namespace KSS.Functionalities
{
    public class FPlayerEngineController : MonoBehaviour
    {
        public List<FRocketEngine> engines = new List<FRocketEngine>();
        public float ThrottleIncrement = 0.002f;

        [ControlIn("toggle.engine", "Toggle Engine")]
        public void ToggleEngine()
        {
            foreach (var engine in engines)
            {
                engine.ToggleEngine();
                Debug.Log($"Toggled engine: {engine.name}. Current status: {(engine.engineEnabled ? "Enabled" : "Disabled")}");
            }
        }

        [ControlIn("increase.throttle", "Increase Throttle")]
        public void IncreaseThrottle()
        {
            foreach (var engine in engines)
            {
                engine.IncreaseThrottle(ThrottleIncrement);
            }
        }

        [ControlIn("decrease.throttle", "Decrease Throttle")]
        public void DecreaseThrottle()
        {
            foreach (var engine in engines)
            {
                engine.DecreaseThrottle(ThrottleIncrement);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ToggleEngine();
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                IncreaseThrottle();
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                DecreaseThrottle();
            }
        }
    }
}