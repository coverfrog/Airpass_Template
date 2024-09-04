using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CoverFrog.Title
{
    public class AnimTitleToMove : Anim
    {
        [Title("AnimTitleToMove")]
        [SerializeField] private float delay;
        [SerializeField] private float duration;
        [Space]
        [SerializeField] private Vector2 startPoint;
        [SerializeField] private Vector2 addPoint;
        
        public override void Init(params object[] values)
        {
           
        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            for (var t = 0.0f; t < delay; t += Time.deltaTime)
            {
                yield return null;
            }

            
            var rt = GetComponent<RectTransform>();
            var endPoint = startPoint + addPoint;

            rt.anchoredPosition = startPoint;
            
            for (var t = 0.0f; t < duration; t += Time.deltaTime)
            {
                rt.anchoredPosition = Vector2.Lerp(startPoint, endPoint, t / duration);
                yield return null;
            }

            rt.anchoredPosition = endPoint;
        }

        [ContextMenu("[ Point Pick - Start]")]
        private void PointPickStart()
        {
            startPoint = GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
