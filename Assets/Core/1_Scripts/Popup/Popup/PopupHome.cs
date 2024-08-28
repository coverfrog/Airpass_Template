using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoverFrog
{
    public class PopupHome : Popup
    {
        [Header("[ PopupHome ]")]
        [SerializeField] private HelperHome quitHelper;
        [SerializeField] private HelperHome resumeHelper;
        [Space]
        [SerializeField] private GameAutoQuit gameAutoQuit;

        [SerializeField] GameManager Gm;
        [SerializeField] Button back_btn;
        [SerializeField] RectTransform Back_rect;

        private void Awake()
        {
            quitHelper.AddAction(OnClick_Quit, quitHelper);
            resumeHelper.AddAction(OnClick_Resume, resumeHelper);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (Gm.IsPlaying)
            {
                back_btn.gameObject.SetActive(true);
                back_btn.onClick.RemoveAllListeners();
                back_btn.onClick.AddListener(() => { AirpassUnity.VRSports.VRSportsButton.interacting = null; Time.timeScale = 0; transform.gameObject.SetActive(false); Back_rect.gameObject.SetActive(true); });
            }
            else
                back_btn.gameObject.SetActive(false);
        }


        public override void OnDisable()
        {
            PopupManager.Instance.PopupOpenCountDown(this);
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
