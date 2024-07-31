using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public enum FadeType
    {
        FadeIn,
        FadeOut,
    }

    public class ProcessFade : Process
    {
        private CanvasGroup _cg;

        private CanvasGroup Cg => _cg ??= transform.GetChild(1).GetComponent<CanvasGroup>();

        private const float Duration = 0.75f;
        
        private bool _isPlaying;
        private float _alphaStartCurrent, _alphaEndCurrent;
        private Action _callbackBetween, _callbackEnd;

        public override void Init(params object[] values)
        {
            
        }
        
        public void BeTweenPlayWithCallback(Action callbackBetween, Action callbackEnd)
        {
            if(_isPlaying) 
                return;

            _callbackBetween = callbackBetween;
            _callbackEnd = callbackEnd;
            _isPlaying = true;

            gameObject.SetActive(true);
            
            StartCoroutine(CoBetweenPlay());
        }

        private IEnumerator CoBetweenPlay()
        {
            _alphaStartCurrent = 0.0f;
            _alphaEndCurrent = 1.0f;
            
            yield return StartCoroutine(CoPlay());
            
            _callbackBetween?.Invoke();
            _alphaStartCurrent = 1.0f;
            _alphaEndCurrent = 0.0f;
            
            yield return StartCoroutine(CoPlay());
            
            _callbackEnd?.Invoke();
            
            Stop();
            
            gameObject.SetActive(false);
        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            Cg.alpha = _alphaStartCurrent;
            
            for (var t = 0.0f; t < Duration; t += Time.deltaTime)
            {
                var alpha = Mathf.Lerp(_alphaStartCurrent, _alphaEndCurrent, t / Duration);
                Cg.alpha = alpha;
                
                yield return null;
            }

            _isPlaying = false;
        }
    }
}
