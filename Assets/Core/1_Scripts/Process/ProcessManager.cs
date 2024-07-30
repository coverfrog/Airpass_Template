using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;

namespace CoverFrog
{
    public enum ProcessState
    {
        Title,
        LevelSelect,
        Fade,
        GamePlay,
        ConceptVideo,
        Narration,
        Clear
    }
    
    public enum InProcessState
    {
        InActive,
        Playing,
        Pause,
        UnPauseWait,
        Completed,
    }

    [Serializable]
    public class ProcessData
    {
        [Header("# --- Title --- #")]
        [SerializeField, Range(1.0f, 10.0f)] public float titleWaitDuration = 3.0f;

        [Header("# --- Game Play --- #")] 
        [SerializeField] public float gamePlayDuration = 120.0f;
        
        [Header("# --- Level Select --- #")]
        [SerializeField, Range(3, 4)] public int levelSelectMaxCount = 3;
        [SerializeField] public int levelSelectFontSize = 50;
        [SerializeField] public AudioName levelSelectAudioName;
        [SerializeField, TextArea] public string levelSelectText;
        [Space]
        
        [Header("# --- Concept Video --- #")]
        [SerializeField] public Sprite conceptVideoSprite;
        [SerializeField] public VideoClip conceptVideo;
        [Space] 
        
        [Header("# --- Narration --- #")] 
        [SerializeField] private List<ProcessNarrationData> narrationDatas;

        public IReadOnlyList<ProcessNarrationData> NarrationAudioNames
            => narrationDatas.OrderBy(an => an.index).ToList();
    }

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

        public ProcessState CurrentState => currentState;
        
        #endregion

        public override void Awake()
        {
            base.Awake();
            _ = Processes;
        }

        private void Start()
        {
            if (!playWhenOnStart)
                return;
            
            foreach (var value in Processes.Values)
                value.gameObject.SetActive(false);

            ToState(ProcessState.Title);
            
            AudioManager.Instance.Play(AudioType.Bgm, AudioName.Bgm);
            AudioManager.Instance.SetLoop(AudioType.Bgm, true);
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
                    ToTile(title);
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
            }
                
        }

        private void ToTile(ProcessTitle title)
        {
            title.Play(processData.titleWaitDuration);
        }

        private void ToLevelSelect(ProcessLevelSelect levelSelect, ProcessFade fade)
        {
            fade.BeTweenPlayWithCallback(() =>
            {
                Processes[ProcessState.Title].SetActive(false);
                Processes[ProcessState.GamePlay].Init(processData.gamePlayDuration);

                levelSelect.Init(
                    processData.levelSelectMaxCount, 
                    processData.levelSelectText,
                    processData.levelSelectFontSize);
                
            }, () =>
            {
                levelSelect.Play(processData.levelSelectAudioName);
            });
        }

        private void ToConceptVideo(ProcessConceptVideo conceptVideo, ProcessFade fade)
        {
            fade.BeTweenPlayWithCallback(() =>
            {
                Processes[ProcessState.LevelSelect].SetActive(false);
                
                conceptVideo.Init(processData.conceptVideoSprite, processData.conceptVideo);
                
            }, () =>
            {
                conceptVideo.Play();
            });
        }

        private void ToNarration(ProcessNarration narration, ProcessFade fade)
        {
            var narrationData = processData.NarrationAudioNames[selectedLevel];
            
            fade.BeTweenPlayWithCallback(() =>
            {
                Processes[ProcessState.ConceptVideo].SetActive(false);
                
                narration.Init(narrationData.descriptionText);
                
            }, () =>
            {
                narration.Play(narrationData.audioName);
            });
        }

        private void ToGamePlay(ProcessGamePlay gamePlay)
        {
            AudioManager.Instance.Stop(AudioType.Narration);
            
            Processes[ProcessState.Narration].SetActive(false);
            
            gamePlay.Play();
        }

        #endregion

        public void OnLevelSelected(int selectLevel)
        {
            selectedLevel = selectLevel;
        }
    }
}
