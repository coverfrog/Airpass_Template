using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

namespace CoverFrog
{
    public class ProcessGamePlay : Process
    {
        [Header("[ Debug Value ]")] 
        [SerializeField] private bool isGameWin;
        [SerializeField] private float gamePlayTimer;

        private int _score;
        private bool _isTimerReduce;

        private float _gamePlayDuration;
        private bool _isAlwaysGameWin;
        
        //

        public override void OnEnable()
        {
            base.OnEnable();

            PopupManager.Instance.OnCountUp += Pause;
            PopupManager.Instance.OnCountDown += UnPause;

            _isTimerReduce = isGameWin = false;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            
            PopupManager.Instance.OnCountUp -= Pause;
            PopupManager.Instance.OnCountDown -= UnPause;
        }

        //

        public override void Pause()
        {
            base.Pause();
            _isTimerReduce = false;
        }

        public override void UnPause()
        {
            base.UnPause();
            
            _isTimerReduce = true;
        }

        public override void Completed()
        {
            base.Completed();

            ProcessManager.Instance.OnGamePlayComplete(_score);
            
            if(_isAlwaysGameWin)
                GameWin();
            else
            {
                if (isGameWin)
                    GameWin();
                else
                    GameLose();
            }
        }
        
        //
        
        private void GameWin()
        {
            ProcessManager.Instance.ToState(ProcessState.Result);
        }

        private void GameLose()
        {
            
        }

        //
        
        public override void Init(params object[] values)
        {
            // [0] gamePlayDuration
            _gamePlayDuration = (float)values[0];
            gamePlayTimer = _gamePlayDuration;
            
            // [1] resultIsAlwaysGameWin
            _isAlwaysGameWin = (bool)values[1];
            
            // _
            _isTimerReduce = false;
            gameObject.SetActive(true);
        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            // value[0] = selectLevel
            var selectLevel = (int)values[0];
            
            // _
            _isTimerReduce = true;
            
            // _
            while (gamePlayTimer > 0.0f)
            {
                if (_isTimerReduce)
                {
                    gamePlayTimer 
                        = Mathf.Clamp(gamePlayTimer - Time.deltaTime, 0.0f, _gamePlayDuration);
                }

                yield return null;
            }
            
            // clear
            Completed();
        }
    }
}
