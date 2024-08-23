using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;

namespace CoverFrog
{
    #region > Datas
    public enum ProcessState
    {
        Title,
        LevelSelect,
        Fade,
        GamePlay,
        ConceptVideo,
        Narration,
        Result,
    }
    
    public enum InProcessState
    {
        InActive,
        Playing,
        Pause,
        UnPauseWait,
        Completed,
    }

    public enum ResultScoreType
    {
        Text,
        Int,
        Float
    }

    [Serializable]
    public class ProcessData
    {
        [Header("# --- Title --- #")]
        [SerializeField, Range(1.0f, 10.0f)] public float titleWaitDuration = 3.0f;

        [Header("# --- Game Play --- #")] 
        [SerializeField] public bool gamePlayIsAlwaysGameWin;
        [SerializeField] public float gamePlayDuration = 120.0f;

        [Header("# --- Level Select --- #")] 
        [SerializeField] public Sprite levelSelectPreviewSprite;
        [SerializeField] public VideoClip levelSelectVideo;
        [SerializeField, Range(3, 4)] public int levelSelectMaxCount = 3;
        [SerializeField] public int levelSelectFontSize = 50;
        [SerializeField] public AudioName levelSelectAudioName;
        [SerializeField, TextArea] public string levelSelectText;
        [Space]
        
        [Header("# --- Concept Video --- #")]
        [SerializeField] public Sprite conceptVideoSprite;
        [SerializeField] public VideoClip conceptVideo;

        [Space] [Header("# --- Narration --- #")] 
        [SerializeField] public Sprite narrationPreviewSprite;
        [SerializeField] public VideoClip narrationVideoClip;
        [SerializeField] private List<ProcessNarrationData> narrationDatas;
        
        public IReadOnlyList<ProcessNarrationData> NarrationAudioNames
            => narrationDatas.OrderBy(an => an.index).ToList();

        [Header("# --- Result --- #")] 
        [SerializeField] public int resultTextSize = 100;
        [SerializeField, TextArea] public string resultScoreFormat;
    }
    #endregion

    [RequireComponent(typeof(SharedMemoryManager))]
    [RequireComponent(typeof(PlatformProtocol))]
    public class ProcessManager : Singleton<ProcessManager>
    {
        [Header("[ Option ]")]
        [SerializeField] private bool playWhenOnStart = false;
        
        [Header("[ Debug Value ]")]
        [SerializeField] private ProcessState currentState;
        [SerializeField] public int selectedLevel;
        
        [Header("[ Data ]")] 
        [SerializeField] private ProcessData processData;

        private object _resultValue;
        
        #region > Processes
        private Dictionary<ProcessState, Process> _processes;

        private Dictionary<ProcessState, Process> Processes => _processes ??= InitProcess();

        private Dictionary<ProcessState, Process> InitProcess()
        {
            var newProcesses = new Dictionary<ProcessState, Process>();
            
            if (Enum.GetValues(typeof(ProcessState)) is not ProcessState[] stateArray)
            {
                Debug.Assert(false, "InitProcess : Don't Convert 'ProcessState' Array");
                return newProcesses;
            }

            foreach (Transform chideTr in transform)
            {
                var childProcess = chideTr.GetComponent<Process>();
                if (!childProcess)
                {
                    continue;
                }

                var childProcessName = childProcess.GetType().Name;
                foreach (var state in stateArray)
                {
                    if (!string.Equals(childProcessName, $"Process{state}")) 
                        continue;
                    
                    if (newProcesses.TryAdd(state, childProcess))
                    {
                        
                    }
                }
            }

            return newProcesses;
        }
        #endregion

        #region > Instances

        private AudioManager _audioIns;
        private AudioManager AudioIns => 
            _audioIns ??= AudioManager.Instance;
        
        private PopupManager _popupIns;
        private PopupManager PopupIns =>
            _popupIns ??= PopupManager.Instance;
        
        #endregion

        #region > Unity
        public void Awake()
        {
            _ = Processes;
        }

        private void Start()
        {
            if (!playWhenOnStart)
                return;

            ActiveAll(false);
            ToState(ProcessState.Title);
            
            AudioIns.Play(AudioType.Bgm, AudioName.Bgm);
            AudioIns.SetLoop(AudioType.Bgm, true);
        }
        #endregion

        private void ActiveAll(bool isActive)
        {
            foreach (var value in Processes.Values)
                value.SetActive(isActive);
        }

        public void OnLevelSelected(int selectLevel)
        {
            selectedLevel = selectLevel;
        }

        public void OnGamePlayComplete(object value)
        {
            _resultValue = value;
        }

        public void OnQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        #region > To State
        public void ToState(ProcessState nextState)
        {
            if (Processes[ProcessState.Fade] is not ProcessFade fade)
                return;

            var nextProcess = Processes[nextState];
            
            switch (nextProcess)
            {
                case ProcessTitle title:
                    ToTitle(title);
                    break;
                case ProcessLevelSelect levelSelect:
                    ToLevelSelect(levelSelect, fade);
                    break;
                case ProcessGamePlay gamePlay:
                    ToGamePlay(gamePlay);
                    break;
                case ProcessConceptVideo conceptVideo:
                    ToConceptVideo(conceptVideo, fade);
                    break;
                case ProcessNarration narration:
                    ToNarration(narration, fade);
                    break;
                case ProcessResult result:
                    ToResult(result);
                    break;
            }
        }
        
        private void ToTitle(ProcessTitle title)
        {
            PopupIns.ToTitle();
            title.Init();
            title.Play(processData.titleWaitDuration);
        }
        
        private void ToConceptVideo(ProcessConceptVideo conceptVideo, ProcessFade fade)
        {
            fade.BeTweenPlayWithCallback(() =>
            {
                Processes[ProcessState.LevelSelect].SetActive(false);
                
                conceptVideo.Init(
                    processData.conceptVideoSprite, 
                    processData.conceptVideo);
                
            }, () =>
            {
                conceptVideo.Play();
            });
        }

        private void ToLevelSelect(ProcessLevelSelect levelSelect, ProcessFade fade)
        {
            fade.BeTweenPlayWithCallback(() =>
            {
                PopupIns.ActiveAll(false);

                Processes[ProcessState.ConceptVideo].SetActive(false);
                Processes[ProcessState.Title].SetActive(false);
                Processes[ProcessState.Result].SetActive(false);
                Processes[ProcessState.GamePlay].Init(
                    processData.gamePlayDuration,
                    processData.gamePlayIsAlwaysGameWin);

                levelSelect.Init(
                    processData.levelSelectMaxCount, 
                    processData.levelSelectText,
                    processData.levelSelectFontSize,
                    processData.levelSelectPreviewSprite,
                    processData.levelSelectVideo);
                
            }, () =>
            {
                levelSelect.Play(processData.levelSelectAudioName);
            });
        }

      

        private void ToNarration(ProcessNarration narration, ProcessFade fade)
        {
            var narrationData = processData.NarrationAudioNames[selectedLevel];
            
            Processes[ProcessState.Title].SetActive(false);
            Processes[ProcessState.LevelSelect].SetActive(false);
            Processes[ProcessState.GamePlay].Init(
                processData.gamePlayDuration,
                processData.gamePlayIsAlwaysGameWin);
                
                
            narration.Init(
                narrationData.descriptionText,
                narrationData.fontSize,
                processData.narrationPreviewSprite,
                processData.narrationVideoClip);
            
            narration.Play(narrationData.audioName);
            
            // fade.BeTweenPlayWithCallback(() =>
            // {
            //     Processes[ProcessState.ConceptVideo].SetActive(false);
            //     
            //     narration.Init(
            //         narrationData.descriptionText,
            //         narrationData.fontSize,
            //         processData.narrationPreviewSprite,
            //         processData.narrationVideoClip);
            //     
            // }, () =>
            // {
            //     narration.Play(narrationData.audioName);
            // });
        }

        private void ToGamePlay(ProcessGamePlay gamePlay)
        {
            AudioIns.Stop(AudioType.Narration);
            
            Processes[ProcessState.Narration].SetActive(false);
            
            gamePlay.Play(
                selectedLevel);
        }

        private void ToResult(ProcessResult result)
        {
            result.Play(
                _resultValue, 
                processData.resultScoreFormat, 
                processData.resultTextSize);
        }
        #endregion

    }
}
