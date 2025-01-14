using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityPlus.UILib;
using UnityPlus.UILib.UIElements;
using KSS.Core;
using UnityEngine.UI;
using UnityPlus.AssetManagement;

namespace KSS.UI
{
    public class SaveWindow : MonoBehaviour
    {
        /// <summary>
        /// Creates a save window with the current context.
        /// </summary>
        public static SaveWindow Create()
        {
            UIWindow window = CanvasManager.Get( CanvasName.WINDOWS ).AddWindow( new UILayoutInfo( new Vector2( 0.5f, 0.5f ), Vector2.zero, new Vector2( 250f, 100f ) ), AssetRegistry.Get<Sprite>( "builtin::Resources/Sprites/UI/part_window" ) )
                .Draggable()
                .Focusable()
                .WithCloseButton( new UILayoutInfo( Vector2.one, new Vector2( -7, -5 ), new Vector2( 20, 20 ) ), AssetRegistry.Get<Sprite>( "builtin::Resources/Sprites/UI/button_x_gold_large" ), out _ );

            SaveWindow saveWindow = window.gameObject.AddComponent<SaveWindow>();

            UIScrollView scrollView = window.AddVerticalScrollView( UILayoutInfo.Fill( 2, 2, 30, 22 ), new Vector2( 0, 75 ) )
                .WithVerticalScrollbar( UILayoutInfo.FillVertical( 2, 2, 1f, 0, 10 ), null, AssetRegistry.Get<Sprite>( "builtin::Resources/Sprites/UI/scrollbar_handle" ), out UIScrollBar scrollbar );


            UIButton saveBtn = window.AddButton( new UILayoutInfo( Vector2.right, new Vector2( -2, 5 ), new Vector2( 95, 15 ) ), AssetRegistry.Get<Sprite>( "builtin::Resources/Sprites/UI/button_biaxial" ) );

            saveBtn.AddText( UILayoutInfo.Fill(), "Save" )
                .WithAlignment( TMPro.HorizontalAlignmentOptions.Center )
                .WithFont( AssetRegistry.Get<TMPro.TMP_FontAsset>( "builtin::Resources/Fonts/liberation_sans" ), 12, Color.white );

            UIInputField inputField = window.AddInputField( UILayoutInfo.FillHorizontal( 2, 99, 0f, 5, 15 ), AssetRegistry.Get<Sprite>( "builtin::Resources/Sprites/UI/input_field" ) );

            return saveWindow;
        }
    }
}