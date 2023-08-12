using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPlus.UILib.UIElements
{
    /// <summary>
    /// Represents a simple icon UI element.
    /// </summary>
    public sealed class UIIcon : UIElement, IUIElementParent
    {
        internal readonly UnityEngine.UI.Image imageComponent;
        public RectTransform contents => base.rectTransform;

        public List<UIElement> Children { get; }

        internal readonly IUIElementParent _parent;
        public IUIElementParent parent { get => _parent; }

        internal UIIcon( RectTransform transform, IUIElementParent parent, UnityEngine.UI.Image imageComponent ) : base( transform )
        {
            this._parent = parent;
            this.parent.Children.Add( this );
            Children = new List<UIElement>();
            this.imageComponent = imageComponent;
        }

        public Sprite Sprite { get => imageComponent.sprite; set => imageComponent.sprite = value; }

        public override void Destroy()
        {
            base.Destroy();
            this.parent.Children.Remove( this );
        }
    }
}