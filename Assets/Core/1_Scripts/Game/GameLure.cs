using System.Collections;
using System.Collections.Generic;
using AirpassUnity.VRSports;
using UnityEngine;

namespace CoverFrog
{
    public class GameLure : CoBehaviour
    {
        [Header("[ Option ]")] 
        [SerializeField] private bool isCanLureToTrueUnPause;
        [SerializeField] private float lureWait = 7.0f;
        [SerializeField] private float current = 0.0f;

        private VRSportsButton[] _vrBtnArr;
        private List<AnimLure> _animLures;

        private void BtnInit()
        {
            _vrBtnArr = FindObjectsOfType<VRSportsButton>();
            _animLures = new List<AnimLure>();
            
            foreach (var button in _vrBtnArr)
            {
                var lure = button.GetComponent<AnimLure>();

                if (lure)
                {
                    _animLures.Add(lure);
                }

                button.OnHolding.AddListener(() =>
                {
                    Stop();
                    _animLures.ForEach(x => x.Stop());
                });
                
                button.OnHoldLost.AddListener(() =>
                {
                    Init();
                    Stop();
                    Play();
                });
                
                button.UnityBtn?.onClick.AddListener(() =>
                {
                    Init();
                    Stop();
                    _animLures.ForEach(x => x.ToDefault());
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
            
            PopupManager.Instance.OnCountUp += Pause;
            PopupManager.Instance.OnCountDown += UnPause;
            
            Init();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            
            PopupManager.Instance.OnCountUp -= Pause;
            PopupManager.Instance.OnCountDown -= UnPause;
        }
        

        public override void Pause()
        {
            base.Pause();
            _animLures.ForEach(x => x.Pause());
        }

        public override void UnPause()
        {
            base.UnPause();

            if (!IsPlaying)
            {
                Play();
            }

            _animLures.ForEach(x =>
            {
                if(isCanLureToTrueUnPause)
                    x.SetCanLure(true);
                
                x.UnPause();
            });
        }


        public override void Stop()
        {
            base.Stop();
            _animLures.ForEach(x => x.Stop());
        }

        public override void Init(params object[] values)
        {
            current = lureWait;
        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            current = lureWait;

            while (current > 0.0f)
            {
                current = Mathf.Clamp(current - Time.deltaTime, 0.0f, float.MaxValue);
                yield return null;
            }
            
            _animLures.ForEach(x => x.Play());
        }
    }
}
