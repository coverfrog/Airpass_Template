using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CoverFrog
{
    public class AnimScale : Anim
    {
        [Title("AnimScale")] 
        [SerializeField] private float delay;
        [SerializeField] private float duration;
        [Space]
        [SerializeField] private float startScale = 1.0f;
        [SerializeField] private float endScale = 1.0f;

        public override void OnEnable()
        {
            base.OnEnable();

            transform.localScale = Vector3.one * startScale;
        }

        public override void Init(params object[] values)
        {
            
        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            yield return new WaitForSeconds(delay);

            for (var t = 0.0f; t < duration; t += Time.deltaTime)
            {
                transform.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, t / duration);
                yield return null;
            }

            transform.localScale = Vector3.one * endScale;
        }
    }
}