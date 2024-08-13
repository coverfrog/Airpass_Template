using System.Collections;
using System.Collections.Generic;
using CoverFrog;
using UnityEngine;

namespace Bum9Dust
{
    public class GameManager : CoverFrog.GameManager
    {
        public override void Init()
        {
            Debug.Log("Init");
        }

        public override void Play(int selectLevel)
        {
            Debug.Log("Play");
        }

        protected override IEnumerator CoCompleted(bool isWin)
        {
            yield return null;            
            ProcessManager.Instance.ToState(ProcessState.Result);
        }
    }
}
