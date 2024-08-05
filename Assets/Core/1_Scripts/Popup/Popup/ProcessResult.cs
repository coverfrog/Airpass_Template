using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoverFrog
{
    public class ProcessResult : Process
    {
        [Header("[ PopupResult _ ]")] 
        [SerializeField] private Text scoreText;
        [SerializeField] private AnimFade btnFade;
        
        [Header("[ PopupResult _ ]")] 
        [SerializeField] private HelperResult quitHelper;
        [SerializeField] private HelperResult retryHelper;

        private void Awake()
        {
            quitHelper.AddAction(OnClick_Quit, quitHelper);
            retryHelper.AddAction(OnClick_Retry, retryHelper);
        }

        public override void Init(params object[] values)
        {
            quitHelper.SetInteract(false);
            retryHelper.SetInteract(false);
        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            var value = values[0];
            var valueFormat = (string)values[1];
            var txtSize = (int)values[2];

            scoreText.text = string.Format(valueFormat, value);
            scoreText.fontSize = txtSize;

            yield return new WaitForSeconds(btnFade.Duration);
            
            quitHelper.SetInteract(true);
            retryHelper.SetInteract(true);
        }

        private void OnClick_Quit(Helper helper)
        {
            ProcessManager.Instance.OnQuit();
        }
        
        private void OnClick_Retry(Helper helper)
        {
            ProcessManager.Instance.ToState(ProcessState.LevelSelect);
        }
    }
}
