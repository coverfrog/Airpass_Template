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

        [Header("[ PopupResult _ ]")] 
        [SerializeField] private GameAutoQuit gameAutoQuit;

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

            
            quitHelper.SetInteract(false);
            retryHelper.SetInteract(false);
            
            yield return new WaitForSeconds(btnFade.Delay + btnFade.Duration);
            
            quitHelper.SetInteract(true);
            retryHelper.SetInteract(true);
        }

        private void OnClick_Quit(Helper helper)
        {
            gameAutoQuit.Stop();
            
            ProcessManager.Instance.OnQuit();
        }
        
        private void OnClick_Retry(Helper helper)
        {
            gameAutoQuit.Stop();

            ProcessManager.Instance.ToState(ProcessState.LevelSelect);
        }
    }
}
