using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    [RequireComponent(typeof(CanvasGroup))]
    public class AnimFade : Anim
    {
        private CanvasGroup _cg;
        
        private CanvasGroup Cg => 
            _cg ??= GetComponent<CanvasGroup>();

        [Header("[ AnimFade ]")] 
        [SerializeField] private bool setStartAlphaBeforeDelay;
        [SerializeField] private float delay;
        [SerializeField] private float duration = 2.0f;
        [Space]
        [SerializeField, Range(0.0f, 1.0f)] private float startAlpha;
        [SerializeField, Range(0.0f, 1.0f)] private float endAlpha = 1.0f;

        public float Duration => duration;
        
        public override void Init(params object[] values)
        {
            
        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            if (setStartAlphaBeforeDelay)
            {
                Cg.alpha = startAlpha;
            }

            for (var t = 0.0f; t < delay; t += Time.deltaTime)
            {
                yield return null;
            }

            Cg.alpha = startAlpha;

            for (var t = 0.0f; t < duration; t += Time.deltaTime)
            {
                Cg.alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
                yield return null;
            }

            Cg.alpha = endAlpha;
        }
    }
}
