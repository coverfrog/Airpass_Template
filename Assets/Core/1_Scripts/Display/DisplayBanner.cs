using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public class DisplayBanner : MonoBehaviour
    {
        [Header("[ PopupBanner ]")] 
        [SerializeField] private HelperBanner homeHelper;
        [SerializeField] private HelperBanner optionHelper;

        private void Awake()
        {
            homeHelper.AddAction(OnClick_Home, homeHelper);
            optionHelper.AddAction(OnClick_Option, optionHelper);
        }

        private void OnClick_Home(Helper helper)
        {
            PopupManager.Instance.PlayTo(PopupState.Home);
        }

        private void OnClick_Option(Helper helper)
        {
            PopupManager.Instance.PlayTo(PopupState.Option);
        }
    }
}
