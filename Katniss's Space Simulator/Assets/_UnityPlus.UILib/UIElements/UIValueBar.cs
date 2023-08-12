using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPlus.UILib.UIElements
{
    public class UIValueBar : UIElement
    {
        internal readonly UnityPlus.UILib.ValueBar valueBarComponent;

        public UIValueBar( RectTransform transform, UnityPlus.UILib.ValueBar valueBarComponent ) : base( transform )
        {
            this.valueBarComponent = valueBarComponent;
        }

        public void ClearSegments()
        {
            valueBarComponent.ClearSegments();
        }

        public ValueBarSegment AddSegment( float width )
        {
            return valueBarComponent.AddSegment( width );
        }

        public ValueBarSegment InsertSegment( int index, float width )
        {
            return valueBarComponent.InsertSegment( index, width );
        }
    }
}