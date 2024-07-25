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
    }
    
    public enum InProcessState
    {
        InActive,
        Playing,
        Pause,
        UnPauseWait,
        Completed,
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

            ToState(ProcessState.Title);
        }

        public void ToState(ProcessState nextState)
        {
            ToState(currentState, currentState = nextState);
        }

        private void ToState(ProcessState prevState, ProcessState nextState)
        {
            Processes[prevState].Stop();
            Processes[nextState].Play();
        }
    }
}
