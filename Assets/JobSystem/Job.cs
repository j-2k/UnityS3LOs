using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job : MonoBehaviour
{
    public enum JobState { Idle, Working, Done }
    public JobState CurrentState
    { 
        get; 
        protected set; 
    }
    public int ThisJobID
    {
        get;
        set;
    }
    public object Result
    { 
        get; 
        protected set; 
    }
    public abstract void Execute();
    public delegate void JobDoneCallBack(object result);
    public JobDoneCallBack JobDone 
    {
        get;
        set;
    }
}
