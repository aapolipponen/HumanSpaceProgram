using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityPlus.UILib.UIElements
{
    /// <summary>
    /// Represents a generic UI element.
    /// </summary>
    public class UIElement
    {
        // PURPOSE:
        // - This class (and its subclasses) is a wrapper around hierarchies of Unity components / gameobjects.
        // Technically, it could also be made a monobehaviour with no changes to the outside code.

        // REASON:
        // - Many UI elements consist of multiple objects.
        // - Their initialization is an annoying ordeal and they can end up with an invalid or non-standard state easily.
        // - This fixes that by encapsulating everything.

        // Some UI elements here have somewhat duplicated purpose. This is for increased verbosity. Lets you specify exactly what you're creating.

        readonly RectTransform _rectTransform;
        /// <summary>
        /// Don't directly modify the fields/state of the rectTransform unless you know what you're doing. You can produce invalid state.
        /// </summary>
        public RectTransform rectTransform { get => _rectTransform; }
        readonly GameObject _gameObject;
        /// <summary>
        /// Don't directly modify the fields/state of the gameObject unless you know what you're doing. You can produce invalid state.
        /// </summary>
        public GameObject gameObject { get => _gameObject; }

        // LAYOUT:
        // - Since everything is encapsulated here, we have FULL CONTROL over when the properties of the underlying components change.
        // - We can do static and dynamic layout without having to rely on Unity's builtin layout components.
        // - Once assigned, the parent can't be changed.

        public UIElement( RectTransform rectTransform )
        {
            this._rectTransform = rectTransform;
            this._gameObject = rectTransform.gameObject;
        }

        /// <summary>
        /// Checks if the underlying UI element has been destroyed.
        /// </summary>
        public virtual bool IsDestroyed => this.gameObject == null;

        /// <summary>
        /// Destroys the specified UI element along with its children UI elements.
        /// </summary>
        public virtual void Destroy()
        {
            // TODO - add a guard that prevents destruction of certain objects, like the contents container of a scroll view.

            if( IsDestroyed )
            {
                return; // Silent quit.
            }
            UnityEngine.Object.Destroy( this.gameObject );
        }

        public virtual Vector2 GetPreferredSize()
        {
            return rectTransform.sizeDelta;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static (GameObject go, RectTransform t) CreateUI( RectTransform parent, string name, UILayoutInfo layout )
        {
            GameObject rootGO = new GameObject( name );

            RectTransform rootT = rootGO.AddComponent<RectTransform>();
            rootT.SetParent( parent );
            rootT.SetLayoutInfo( layout );
            rootT.localScale = Vector3.one;

            return (rootGO, rootT);
        }

        public static explicit operator UIElement( RectTransform rectTransform )
        {
            return new UIElement( rectTransform );
        }

        public static explicit operator RectTransform( UIElement uiElement )
        {
            return uiElement.rectTransform;
        }

        public static explicit operator UIElement( Transform transform )
        {
            return new UIElement( (RectTransform)transform );
        }

        public static explicit operator Transform( UIElement uiElement )
        {
            return uiElement.rectTransform;
        }
    }
}