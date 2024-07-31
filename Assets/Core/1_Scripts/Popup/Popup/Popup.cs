using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public abstract class Popup : CoBehaviour
    {
        public override void OnEnable()
        {
            base.OnEnable();
            PopupManager.Instance.PopupOpenCountUp(this);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PopupManager.Instance.PopupOpenCountDown(this);
        }
    }
}
