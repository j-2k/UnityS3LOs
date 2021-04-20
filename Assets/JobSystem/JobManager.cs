using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class JobManager : MonoBehaviour
{
    public static JobManager i;
    public bool isMultithreading;

    ExecuteJob jobThread1;

    // Start is called before the first frame update
    void Awake()
    {
        if (i == null)
        {
            i = this;
        }

        if(isMultithreading == true)
        {
            CreateThread();
        }
    }

    public void CreateThread()
    {
        jobThread1 = new ExecuteJob();
        JobHandler.AddThenExecuteJob(jobThread1);
    }

    public void AddObjectToJob(ObjectJob obj)
    {
        //jobThread1.objectToThread = obj;
        jobThread1.queueOfObjs.Enqueue(obj);
    }

    public void OnDestroy()
    {
        if (isMultithreading == true)
        {
            jobThread1.terminateThreads = true;
            jobThread1 = null;
        }
    }

    /*
    private void Update()
    {
            //jobThread1 = GetComponent<ExecuteJob>();
            public System.Numerics.Vector3 rot;
            public System.Numerics.Vector3 pos;
            transform.position += new Vector3(pos.X,pos.Y,pos.Z) * Time.deltaTime;//pos * Time.deltaTime;
            transform.Rotate(new Vector3(rot.X, rot.Y, rot.Z) * Time.deltaTime);
    }
    */
}
