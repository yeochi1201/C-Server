using ServerCore;

namespace Server
{
    struct JobTimerElement : IComparable<JobTimerElement>
    {
        public int execTick;
        public Action action;
        public int CompareTo(JobTimerElement other)
        {
            return other.execTick - execTick;
        }
    }

    public class JobTimer
    {
        PriorityQueue<JobTimerElement> _priorityQueue = new PriorityQueue<JobTimerElement>();
        object _lock = new object();

        public static JobTimer Instance { get; } = new JobTimer();

        public void Push(Action action, int tick = 0)
        {
            JobTimerElement job;
            job.execTick = System.Environment.TickCount + tick;
            job.action = action;

            lock (_lock)
            {
                _priorityQueue.Push(job);
            }
        }

        public void Flush()
        {
            while (true)
            {
                int now = System.Environment.TickCount;
                JobTimerElement job;
                lock (_lock)
                {
                    if(_priorityQueue.Count == 0)
                    {
                        break;
                    }

                    job = _priorityQueue.Peek();
                    if(job.execTick > now)
                    {
                        break;
                    }

                    _priorityQueue.Pop();
                }

                job.action.Invoke();
            }
        }
    }
}
