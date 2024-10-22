using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : Button
{
        [SerializeField]
        public Selectable upSelectable;

        [SerializeField]
        public Selectable downSelectable;

        [SerializeField]
        public Selectable leftSelectable;

        [SerializeField]
        public Selectable rightSelectable;

        public override Selectable FindSelectableOnUp()
        {
            return upSelectable != null ? upSelectable : base.FindSelectableOnUp();
        }

        public override Selectable FindSelectableOnDown()
        {
            return downSelectable != null ? downSelectable : base.FindSelectableOnDown();
        }

        public override Selectable FindSelectableOnLeft()
        {
            return leftSelectable != null ? leftSelectable : base.FindSelectableOnLeft();
        }

        public override Selectable FindSelectableOnRight()
        {
            return rightSelectable != null ? rightSelectable : base.FindSelectableOnRight();
        }
}
