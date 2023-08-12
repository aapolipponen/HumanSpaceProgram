using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPlus.UILib.UIElements
{
    /// <summary>
    /// Represents a section of the canvas, or of a different UI element.
    /// </summary>
    public sealed class UIPanel : UIElement, IUIElementParent
    {
        internal readonly UnityEngine.UI.Image backgroundComponent;
        public RectTransform contents => base.rectTransform;

        public List<UIElement> Children { get; }
        internal readonly IUIElementParent _parent;
        public IUIElementParent parent { get => _parent; }

        internal UIPanel( RectTransform transform, IUIElementParent parent, UnityEngine.UI.Image backgroundComponent ) : base( transform )
        {
            this._parent = parent;
            this.parent.Children.Add( this );
            Children = new List<UIElement>();
            this.backgroundComponent = backgroundComponent;
        }

        public Sprite Background { get => backgroundComponent.sprite; set => backgroundComponent.sprite = value; }

        public override void Destroy()
        {
            base.Destroy();
            this.parent.Children.Remove( this );
        }
    }
}