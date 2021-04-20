using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CustomJobSystem : MonoBehaviour
{
    /*
     *         interface IJob
        {
            public enum State { Idle, Working, Done } //the 3 states of a job system when they are before working / working / finished
            public State CurrentState 
            {
                get; 
            }
            public object Result 
            { 
                get; 
            }
            public abstract void Execute();
            public delegate void JobDoneCallBack(object result); //A delegate is a reference type variable that holds the reference to a method & we also take in a object for the jobs result
            public JobDoneCallBack JobDone //making a set & get for the function ptr // think of this as a variable for the previous method
            {
                get; 
                set; 
            }

        }
        */


    public GameObject[] arrayGO;

    public enum JobState
    {
        Idle,
        Working,
        Done
    } //the 3 states of a job system when they are before working / working / finished

    public delegate void JobDoneCallBack(object result); //A delegate is a reference type variable that holds the reference to a method & we also take in a object for the jobs result

    public interface IJob
    {
        JobState CurrentState
        {
            get;
        }

        object Result
        {
            get;
        }
        void Execute();

        JobDoneCallBack JobDone //making a set & get for the function ptr // think of this as a variable for the previous method
        {
            get;
            set;
        }
    }

    public class PrintHP : IJob
    {

        CustomJobSystem cJS;
        
        //IJob.State currentState;
        //public IJob.State CurrentState
        //

        JobState currentState;
        public JobState CurrentState
        {
            get
            {
                return currentState;
            }
            private set
            {
                currentState = value;
            }
        }

        int result;
        public object Result
        {
            get
            {
                return result;
            }
            private set
            {
                result = (int)value;
            }
        }

        //IJob.JobDoneCallBack jobDone;
        //public IJob.JobDoneCallBack JobDone
        JobDoneCallBack jobDone;
        public JobDoneCallBack JobDone
        {
            get
            {
                return jobDone;
            }
            set
            {
                jobDone = value;
            }

        }
        
        public void Execute()
        {
            CurrentState = JobState.Working;//CurrentState = IJob.JobState.Working;
            int runTimes = 10;
            //int runTimes = cJS.arrayGO.Length;
            for (int i = 0; i < runTimes; i++)
            {
                Debug.Log("PRINT HP TEST");
                //cJS.arrayGO[i].transform.Rotate(1, 1, 1);
                Thread.Sleep(100);
            }
            JobDone(100);
            CurrentState = JobState.Done;//CurrentState = IJob.JobState.Done;
        }

        /*
        public void Execute()
        {
            CurrentState = JobState.Working;//CurrentState = IJob.JobState.Working;
            int runTimes = 10;
            for (int i = 0; i < runTimes; i++)
            {
                Debug.Log("PRINT HP TEST");
                Thread.Sleep(100);
            }
            JobDone(100);
            CurrentState = JobState.Done;//CurrentState = IJob.JobState.Done;
        }
        */
    }

    /*
    class PrintDMG : IJob
    {
        IJob.State currentState;
        public IJob.State CurrentState
        {
            get
            {
                return currentState;
            }
            private set
            {
                currentState = value;
            }
        }

        int result;
        public object Result
        {
            get
            {
                return result;
            }
            private set
            {
                result = (int)value;
            }
        }

        public IJob.JobDoneCallBack jobDone;
        public IJob.JobDoneCallBack JobDone
        {
            get
            {
                return jobDone;
            }
            set
            {
                jobDone = value;
            }

        }

        public void Execute()
        {
            CurrentState = IJob.State.Working;
            int runTimes = 5;
            for (int i = 0; i < runTimes; i++)  //the amount of times for this to execute example here we have 5 times.
            {
                Console.WriteLine("PRINT DMG DONE");
                Thread.Sleep(100);
            }
            JobDone(3000);    //this is from the delegate function of JobDone & we give it a object in this game just 100
            CurrentState = IJob.State.Done;
        }

    }
    */

    static int jobId = 0;
    static ConcurrentDictionary<int, IJob> jobs = new ConcurrentDictionary<int, IJob>();

    static int AddThenExecuteJob(IJob job)
    {
        int tries = 10;

        while (tries >= 0)
        {
            if (jobs.TryAdd(jobId, job))
            {
                ThreadPool.QueueUserWorkItem(o => job.Execute());
                jobId++;
                return jobId - 1;
            }

            Thread.Sleep(10);
            tries--;
            Debug.Log("Cant add job, " + tries + " is the # of tries left to add it again");

        }

        return -1;
    }


    static void OnJobDone(object result)
    {
        //Console.WriteLine(result);
        Debug.Log(result);
    }

    // Start is called before the first frame update
    void Start()
    {
        //JOB = PRINT HP CLASS
        PrintHP jHP = new PrintHP();
        jHP.JobDone += OnJobDone; //add onto JobDone the static function above with the result.

        int myJobId1 = AddThenExecuteJob(jHP);

        /*
        //JOB = PRINT DMG CLASS
        PrintDMG jDMG = new PrintDMG();
        jDMG.JobDone += OnJobDone; //add onto JobDone the static function below with the result.
        */

        //int myJobId2 = AddThenExecuteJob(jDMG);
    }

    //terminating

    // Update is called once per frame
    void Update()
    {
        
    }
}
