using System.Collections;
using System.Collections.Generic;
using CoverFrog;
using UnityEngine;
using UnityEngine.UI;


namespace Bum9Dust
{
    public class GameManager : CoverFrog.GameManager
    {

        public override void Init()
        {
            base.Init();
            Debug.Log("Init");
        }

        public override void Play(int selectLevel)
        {
            base.Play(selectLevel);
            Debug.Log("Play");
        }

        void _Cahing()
        {

        }

        void _StartInit()
        {

        }

        void _StartGame()
        {
            _StartInit();
        }

        void _StageInit()
        {

        }

        void _StartStage()
        {

        }

        void _EndStage()
        {

        }

        void _NextStage()
        {

        }

        void _EndGame()
        {
            StopCoroutine(_GameTimer);
        }

        protected override IEnumerator CoCompleted(bool isWin)
        {
            yield return null;            
            ProcessManager.Instance.ToState(ProcessState.Result);
        }
    }
}
