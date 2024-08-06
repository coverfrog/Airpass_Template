using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog.Title
{
    public class AnimTitleDown : Anim
    {
        [Header("[ AnimTitleDown ]")] 
        [SerializeField] private float duration;
        [SerializeField] private float delay;
        [Space] 
        [SerializeField] private float x = -48;
        [SerializeField] private float startHeight = 600;
        [SerializeField] private float endHeight = -141;

        private RectTransform _rt;

        private RectTransform Rt => _rt ??= GetComponent<RectTransform>();
        
        public override void Init(params object[] values)
        {
            
        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            yield return new WaitForSeconds(delay);
            
            Rt.anchoredPosition = Vector2.up * startHeight + x * Vector2.right;

            for (var t = 0.0f; t < duration; t += Time.deltaTime)
            {
                var percent = t / duration;

                Rt.anchoredPosition = Mathf.Lerp(startHeight, endHeight, percent) * Vector2.up + x * Vector2.right;
                yield return null;
            }

            Rt.anchoredPosition = Vector2.up * endHeight + x * Vector2.right;
        }
    }
}
