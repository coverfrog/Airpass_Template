using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.MemoryMappedFiles;
using System.Threading;

public class SharedMemoryManager : Singleton<SharedMemoryManager>
{
    private MemoryMappedFile memoryMappedFile;
    private Mutex mutex;

    private void Awake()
    {
        memoryMappedFile = MemoryMappedFile.CreateOrOpen("MySharedMemory", 1024);
        mutex = new Mutex();
    }
    
    public void WriteToSharedMemory(bool data)
    {
        // TODO : 기록 성공 시 
        // TODO : 저장 할 것 인가? 저장 - true
        
        mutex.WaitOne();

        memoryMappedFile.CreateViewAccessor().Write(0, data);

        mutex.ReleaseMutex();
    }
}
