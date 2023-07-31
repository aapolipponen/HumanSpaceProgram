using System;
using System.Collections;
using System.Collections.Generic;
using UILib;
using UnityEngine;

namespace KSS.Core
{
    public class MainMenuManager : MonoBehaviour
    {
#warning TODO - turn MainMenuManager into a singleton with the editor-assigned fields.

        [SerializeField]
        Canvas _mainMenuCanvas;

        [SerializeField]
        UIStyle _style;

        void Awake()
        {
            HSPOverridableEvent.EventManager.TryInvoke( HSPOverridableEvent.EVENT_STARTUP_MAINMENU, (_mainMenuCanvas, _style) );
        }
    }
}