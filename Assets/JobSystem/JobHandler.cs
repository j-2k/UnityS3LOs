using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

public class JobHandler : MonoBehaviour
{
    public static int jobId = 0;
    public static ConcurrentDictionary<int, Job> jobs = new ConcurrentDictionary<int, Job>();

    public static int AddThenExecuteJob(Job job, int tries = 10)
    { 
        while (tries >= 0)
        {
            if (jobs.TryAdd(jobId, job))
            {
                ThreadPool.QueueUserWorkItem(o => job.Execute());
                job.ThisJobID = jobId;
                jobId++;
                return jobId - 1;
            }
            Thread.Sleep(10);
            tries--;
            Debug.Log("Cant add job, " + tries + " is the # of tries left to add it again, if " + tries + " < 0 this will terminate & return -1");
        }
        Debug.Log("Adding Job Failed Returning -1");
        return -1;
    }
}
