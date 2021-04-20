using UnityEngine;
using System.Collections.Concurrent;

public class ExecuteJob : Job
{
    public bool terminateThreads;

    Job job;

    public ConcurrentQueue<ObjectJob> queueOfObjs = new ConcurrentQueue<ObjectJob>();
    public ObjectJob objectToThread;


    public override void Execute()
    {
        CurrentState = Job.JobState.Working;
        
        while (true)
        {
            if(terminateThreads == true)
            {
                //if(JobHandler.jobs.TryRemove(ThisJobID, out job))
                break;
            }


            //LATECHECK / NULLCHECK

            /*
            if (objectToThread != null)
            {
                objectToThread.ComplexGarbage();
            }
            */

            if(queueOfObjs.Count > 0)
            {
                queueOfObjs.TryDequeue(out objectToThread);
                objectToThread.ComplexGarbage();
                queueOfObjs.Enqueue(objectToThread);
            }
        }
        
        //JobDone(100);
        //Debug.Log("done");
        CurrentState = Job.JobState.Done;
    }
}
