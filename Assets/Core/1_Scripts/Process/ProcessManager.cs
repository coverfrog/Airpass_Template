using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public enum ProcessState
    {
        Title,
        LevelSelect,
        Fade,
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
        [SerializeField, Range(3, 4)] public int levelSelectMaxCount = 3;
    }

    [RequireComponent(typeof(SharedMemoryManager))]
    [RequireComponent(typeof(PlatformProtocol))]
    public class ProcessManager : Singleton<ProcessManager>
    {
        [SerializeField] protected bool playWhenOnStart = false;
        [SerializeField] private ProcessState currentState;

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

            currentState = ProcessState.Title;
            Processes[currentState].Play();
        }

        #region > To State
        public void ToStateBetweenFade(ProcessState nextState)
        {
            if (Processes[ProcessState.Fade] is not ProcessFade fade)
                return;

            var nextProcess = Processes[nextState];
            
            if(nextProcess is ProcessLevelSelect levelSelect)
                ToLevelSelect(levelSelect, fade, 3);
        }

        private void ToLevelSelect(ProcessLevelSelect levelSelect, ProcessFade fade, int levelMaxCount)
        {
            fade.BeTweenPlayWithCallback(() =>
            {
                Processes[ProcessState.Title].SetActive(false);
                Processes[ProcessState.LevelSelect].SetActive(true);
                
            }, () =>
            {
                
            });
        }

        #endregion
    }
}
