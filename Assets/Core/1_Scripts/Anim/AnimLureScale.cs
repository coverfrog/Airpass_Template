using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public class AnimLureScale : AnimLure
    {
        [Header("[ Option ]")]
        [SerializeField] private bool toCanLureWhenUnPause;
        [SerializeField] private bool isCanLure = true;

        [Header("[ Value ]")] 
        [SerializeField] private float delay = 0.0f;
        [SerializeField] private float speed = 1.0f;
        [SerializeField, Range(0.0f, 1.0f)] private float minScale = 0.95f;
        [SerializeField, Range(0.0f, 1.0f)] private float maxScale = 1.0f;
        
        protected override bool IsCanLure => isCanLure;

        public override void SetCanLure(bool isValue)
        {
            isCanLure = isValue;
        }

        public override void ToDefault()
        {
            transform.localScale = Vector3.one * maxScale;
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }
        
        public override void Init(params object[] values)
        {
           
        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            yield return new WaitForSeconds(delay);
            
            var timer = 0.0f;

            while (true)
            {
                timer += Time.deltaTime * speed;
                var value = Mathf.Cos(timer);
                    
                transform.localScale = Vector3.one * (minScale + (maxScale - minScale) * value);

                yield return null;
            }
        }

    }
}
