using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoverFrog
{
    public class ProcessTitle : Process
    {
        public override void Init(params object[] values)
        {
            
        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            // [0]
            var titleWaitDuration = (float)values[0];
            
            yield return new WaitForSeconds(titleWaitDuration);
            
            Completed();
        }

        public override void Completed()
        {
            base.Completed();

            ProcessManager.Instance.ToState(ProcessState.ConceptVideo);
        }
    }
}
