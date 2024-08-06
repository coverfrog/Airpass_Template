using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public abstract class AnimLure : CoBehaviour
    {
        protected abstract bool IsCanLure { get; }

        public abstract void SetCanLure(bool isValue);

        public abstract void ToDefault();

        public override void OnEnable()
        {
            base.OnEnable();

            PopupManager.Instance.OnCountUp += Pause;
            PopupManager.Instance.OnCountDown += UnPause;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            
            PopupManager.Instance.OnCountUp -= Pause;
            PopupManager.Instance.OnCountDown -= UnPause;
        }

        public override void Play(params object[] values)
        {
            if(!IsCanLure)
                return;
            
            base.Play(values);
        }

        public override void Stop()
        {
            if(!IsCanLure)
                return;
            
            base.Stop();
            
            ToDefault();
        }

        public override void Pause()
        {
            if(!IsCanLure)
                return;
            
            base.Pause();
            
            ToDefault();

        }

        public override void UnPause()
        {
            if(!IsCanLure)
                return;
            
            base.UnPause();
            
            ToDefault();
        }
    }
}
