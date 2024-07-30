using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

namespace CoverFrog
{
    public class ProcessGamePlay : Process
    {
        [SerializeField] private float gamePlayTimer;
        
        private float _gamePlayDuration;
        
        public override void Init(params object[] values)
        {
            // [0] gamePlayDuration
            _gamePlayDuration = (float)values[0];
            gamePlayTimer = _gamePlayDuration;
            
            // _
            gameObject.SetActive(true);
        }

        public override IEnumerator CoPlay(params object[] values)
        {
            while (gamePlayTimer > 0.0f)
            {
                yield return null;
            }
        }
    }
}
