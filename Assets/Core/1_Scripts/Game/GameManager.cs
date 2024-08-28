using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoverFrog
{
    public abstract class GameManager : MonoBehaviour
    {
        public bool IsPlaying = false;
        const int GameMaxSecond = 120;
        [SerializeField] Button _Back_btn;
        [SerializeField] RectTransform _back_Select_rect;
        [SerializeField] Button _Back_select_Level_btn;
        [SerializeField] Button _Back_select_Activity_btn;
        [SerializeField] Button _Back_Select_Back_btn;

        public IEnumerator _GameTimer;
        public float _GameTime = 0;
        IEnumerator _cGameTimer()
        {
            while (_GameTime < GameMaxSecond)
            {
                _GameTime += 1;
                yield return new WaitForSecondsRealtime(1.0f);
                yield return null;
            }
            ProcessManager.Instance.OnQuit();
        }
        void _Back_btn_evt()
        {
            if (_back_Select_rect.gameObject.activeSelf)
            {
                _back_Select_rect.gameObject.SetActive(false);
                //
                Time.timeScale = 1;
            }
            else
            {
                _back_Select_rect.gameObject.SetActive(true);
                //
                Time.timeScale = 0;
            }
        }
        IEnumerator _Back_select_Level_btn_evt()
        {
            Init();
            _GameTime = 0;
            Time.timeScale = 1;
            StopAllCoroutines();
            ProcessManager.Instance.ToState(ProcessState.LevelSelect);
            _back_Select_rect.gameObject.SetActive(false);
            yield return null;

            
        }
        IEnumerator _Back_select_Activity_btn_evt()
        {
            Init();
            _GameTime = 0;
            Time.timeScale = 1;
            StopAllCoroutines();
            ProcessManager.Instance.ToState(ProcessState.Narration);
            _back_Select_rect.gameObject.SetActive(false);
            yield return null;
            
        }
        public virtual void Init() 
        {
            IsPlaying = false;
            _Back_btn.gameObject.SetActive(false);
            _back_Select_rect.gameObject.SetActive(false);
        }
        public virtual void Play(int selectLevel)
        {
            IsPlaying = true;

            _BackBtnCahing();
            //
            _GameTime = 0;
            _GameTimer = _cGameTimer();
            StartCoroutine(_GameTimer);


        }
        void _BackBtnCahing()
        {
            //
            IsPlaying = true;
            //
            _Back_btn.gameObject.SetActive(true);
            _Back_btn.onClick.RemoveAllListeners();
            _Back_btn.onClick.AddListener(_Back_btn_evt);
            _Back_select_Level_btn.onClick.RemoveAllListeners();
            _Back_select_Level_btn.onClick.AddListener(() => StartCoroutine(_Back_select_Level_btn_evt())); ;
            _Back_select_Activity_btn.onClick.RemoveAllListeners();
            _Back_select_Activity_btn.onClick.AddListener(() => StartCoroutine(_Back_select_Activity_btn_evt()));
            _Back_Select_Back_btn.onClick.RemoveAllListeners();
            _Back_Select_Back_btn.onClick.AddListener(_Back_btn_evt);
        }
        private IEnumerator _coCompleted;
        protected abstract IEnumerator CoCompleted(bool isWin);        
        public void Completed(bool isWin)
        {
            _coCompleted = CoCompleted(isWin);
            StartCoroutine(_coCompleted);
        }

#if UNITY_EDITOR


        
        private void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 50;
            style.normal.textColor = Color.red;

            GUI.Label(new Rect(5, 5, Screen.width, 20), "GameTime(s) : " + _GameTime, style);           
        }
#endif
        protected void Update()
        {
            if (Input.anyKey)
            {
                _GameTime = 0;
            }




        }




    }
}
