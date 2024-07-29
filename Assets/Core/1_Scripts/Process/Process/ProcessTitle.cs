using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoverFrog
{
    public class ProcessTitle : Process
    {
        [SerializeField, Range(1.0f, 10.0f)] private float waitDurationAutoCompleted = 3.0f;
        [Space]
        [SerializeField] private Text textTitle;
        [SerializeField] private List<FlipBook> flipBooks;
        
        public override IEnumerator CoPlay()
        {
            yield return new WaitForSeconds(waitDurationAutoCompleted);
            
            Completed();
        }

        public override void Completed()
        {
            base.Completed();

            ProcessManager.Instance.ToStateBetweenFade(ProcessState.LevelSelect);
        }
    }
}
