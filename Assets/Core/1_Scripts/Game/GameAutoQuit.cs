using System;
using System.Collections;
using System.Collections.Generic;
using AirpassUnity.VRSports;
using UnityEngine;

namespace CoverFrog
{
    public class GameAutoQuit : CoBehaviour
    {
        [Header("[ Option ]")]
        [SerializeField] private float quitWait = 120.0f;
        [SerializeField] private float current;

        private VRSportsButton[] _vrBtnArr;

        private void BtnInit()
        {
            _vrBtnArr = FindObjectsOfType<VRSportsButton>();
            
            foreach (var button in _vrBtnArr)
            {
                if(button == null)
                    continue;
                
                button.OnHolding?.AddListener(() =>
                {
                    Init();
                });
                
                button.UnityBtn?.onClick.AddListener(() =>
                {
                    Init();
                });
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();

            if (_vrBtnArr == null)
            {
                BtnInit();
            }

            Init();
            
            PopupManager.Instance.OnCountUp += Pause;
            PopupManager.Instance.OnCountDown += UnPause;
        }

        public override void OnDisable()
        {
            base.OnDisable();

            PopupManager.Instance.OnCountUp -= Pause;
            PopupManager.Instance.OnCountDown -= UnPause;
        }

        public override void Init(params object[] values)
        {
            current = quitWait;
        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            current = quitWait;

            while (current > 0.0f)
            {
                current = Mathf.Clamp(current - Time.deltaTime, 0.0f, float.MaxValue);
                yield return null;
            }
            
            ProcessManager.Instance.OnQuit();
        }
    }
}
