﻿using KSS.Core;
using KSS.Core.Mods;
using KSS.Core.Scenes;
using KSS.UI;
using System;
using System.Collections.Generic;
using UnityPlus.UILib;
using UnityEngine;
using UnityEngine.UI;
using UnityPlus.UILib.UIElements;

namespace Assets.KSS.UI.SceneFactories
{
    public static class VanillaGameplayUIFactory
    {
        [OverridableEventListener( HSPOverridableEvent.STARTUP_GAMEPLAY, HSPOverridableEvent.NAMESPACE_VANILLA + ".gameplay_ui" )]
        public static void Create( object obj )
        {
            UIElement canvasTransform = (UIElement)CanvasManager.Get( CanvasName.STATIC );

#warning todo complete the mainmenu factory.
        }

        // #-#-#-#-#-#-#-#-#-#

        private static void CreateMainMenuButton( RectTransform parent )
        {

        }

        private static void CreateSaveButton( RectTransform parent )
        {

        }

        private static void CreateLoadButton( RectTransform parent )
        {

        }
    }
}
