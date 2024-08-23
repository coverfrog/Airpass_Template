using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoverFrog
{
    public class ProcessTitle : Process
    {

        enum titleTyp 
        {
            Subject = 0,
            Math
        }

        [SerializeField] titleTyp TitleType = titleTyp.Subject;

        public override void Init(params object[] values)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        public override void Play(params object[] values)
        {
            base.Play(values);
            transform.GetChild((int)TitleType).gameObject.SetActive(true);
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

            if (TitleType == titleTyp.Math)
                transform.GetChild((int)TitleType).GetChild(0).GetChild(2).GetComponent<UnityEngine.Video.VideoPlayer>().Stop();

            //ProcessManager.Instance.ToState(ProcessState.ConceptVideo);

            ProcessManager.Instance.ToState(ProcessState.LevelSelect);
        }
    }
}
