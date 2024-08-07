using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public class PopupHome : Popup
    {
        [Header("[ PopupHome ]")]
        [SerializeField] private HelperHome quitHelper;
        [SerializeField] private HelperHome resumeHelper;
        [Space]
        [SerializeField] private GameAutoQuit gameAutoQuit;


        private void Awake()
        {
            quitHelper.AddAction(OnClick_Quit, quitHelper);
            resumeHelper.AddAction(OnClick_Resume, resumeHelper);
        }

        public override void Init(params object[] values)
        {

        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            yield return null;
        }

        private void OnClick_Quit(Helper helper)
        {
            gameAutoQuit.Stop();
            
            ProcessManager.Instance.OnQuit();
        }

        private void OnClick_Resume(Helper helper)
        {
            gameAutoQuit.Stop();

            PopupManager.Instance.PlayTo(PopupState.Home);
        }
    }
}
