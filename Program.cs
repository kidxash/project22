using System;
using System.Collections.Generic;

class Program
{  
    class HRRN
    {
        public int Id { get; set; }
        public int ArrivalTime { get; set; }
        public int BurstTime { get; set; }
        public int CompletionTime { get; set; }
        public int TurnaroundTime { get; set; }
        public int WaitingTime { get; set; }
        public bool IsCompleted { get; set; }
    }

    class SRTF
    {
        public int Id { get; set; }
        public int ArrivalTime { get; set; }
        public int BurstTime { get; set; }
        public int RemainingTime { get; set; }
        public int CompletionTime { get; set; }
        public int TurnaroundTime { get; set; }
        public int WaitingTime { get; set; }
    }

    static void Main(string[] args)
    {

        List<SRTF> processes = new List<SRTF>
        {
            new SRTF { Id = 1, ArrivalTime = 0, BurstTime = 8 },
            new SRTF { Id = 2, ArrivalTime = 1, BurstTime = 4 },
            new SRTF { Id = 3, ArrivalTime = 2, BurstTime = 9 },
            new SRTF { Id = 4, ArrivalTime = 3, BurstTime = 5 }
        };
        
        foreach (var SRTF in processes)
        {
            SRTF.RemainingTime = SRTF.BurstTime;
        }

        int currentTime = 0;
        int completed = 0;
        int n = processes.Count;

        while (completed != n)
        {
            SRTF shortest = null;

            foreach (var SRTF in processes)
            {
                if (SRTF.ArrivalTime <= currentTime && SRTF.RemainingTime > 0)
                {
                    if (shortest == null || SRTF.RemainingTime < shortest.RemainingTime)
                    {
                        shortest = SRTF;
                    }
                }
            }

            if (shortest == null)
            {
                currentTime++;
                continue;
            }

            shortest.RemainingTime--;
            currentTime++;

            if (shortest.RemainingTime == 0)
            {
                completed++;
                shortest.CompletionTime = currentTime;
                shortest.TurnaroundTime = shortest.CompletionTime - shortest.ArrivalTime;
                shortest.WaitingTime = shortest.TurnaroundTime - shortest.BurstTime;
            }
        }
        Console.WriteLine("SRTF Scheduling:");
        Console.WriteLine("ID\tArrival\tBurst\tCompletion\tTurnaround\tWaiting");
        foreach (var process in processes)
        {
            Console.WriteLine($"{process.Id}\t{process.ArrivalTime}\t{process.BurstTime}\t{process.CompletionTime}\t\t{process.TurnaroundTime}\t\t{process.WaitingTime}");
        }
        Console.WriteLine("\nAverage Turnaround Time: " + (double)processes.Sum(p => p.TurnaroundTime) / n);
        Console.WriteLine("Average Waiting Time: " + (double)processes.Sum(p => p.WaitingTime) / n);
        Console.WriteLine("CPU Utilization: " + (double)processes.Sum(p => p.BurstTime) / processes.Sum(p => p.CompletionTime) * 100 + "%");
        Console.WriteLine("Throughput: " + (double)n / processes.Sum(p => p.CompletionTime) * 1000); // Assuming time unit is in milliseconds
        
        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine("HRRN Scheduling:");

        // HRRN Scheduling
          List<HRRN> list = new List<HRRN>
        {
            new HRRN { Id = 1, ArrivalTime = 0, BurstTime = 8 },
            new HRRN { Id = 2, ArrivalTime = 1, BurstTime = 4 },
            new HRRN { Id = 3, ArrivalTime = 2, BurstTime = 9 },
            new HRRN { Id = 4, ArrivalTime = 3, BurstTime = 5 }
        };

        int Time = 0;
        int Finished = 0;
        int count = list.Count;

        while (Finished != count)
        {
            HRRN highestResponseRatioProcess = null;
            double highestResponseRatio = -1;

            foreach (var HRRN in list)
            {
                if (!HRRN.IsCompleted && HRRN.ArrivalTime <= Time)
                {
                    int waitingTime = Time - HRRN.ArrivalTime;
                    double responseRatio = (double)(waitingTime + HRRN.BurstTime) / HRRN.BurstTime;

                    if (responseRatio > highestResponseRatio)
                    {
                        highestResponseRatio = responseRatio;
                        highestResponseRatioProcess = HRRN;
                    }
                }
            }

            if (highestResponseRatioProcess == null)
            {
                Time++;
                continue;
            }

            Time += highestResponseRatioProcess.BurstTime;
            highestResponseRatioProcess.CompletionTime = Time;
            highestResponseRatioProcess.TurnaroundTime = highestResponseRatioProcess.CompletionTime - highestResponseRatioProcess.ArrivalTime;
            highestResponseRatioProcess.WaitingTime = highestResponseRatioProcess.TurnaroundTime - highestResponseRatioProcess.BurstTime;
            highestResponseRatioProcess.IsCompleted = true;
            Finished++;
        }

        Console.WriteLine("ID\tArrival\tBurst\tCompletion\tTurnaround\tWaiting");
        foreach (var HRRN in list)
        {
            Console.WriteLine($"{HRRN.Id}\t{HRRN.ArrivalTime}\t{HRRN.BurstTime}\t{HRRN.CompletionTime}\t\t{HRRN.TurnaroundTime}\t\t{HRRN.WaitingTime}");
        }
        Console.WriteLine("\nAverage Turnaround Time: " + (double)list.Sum(p => p.TurnaroundTime) / count);
        Console.WriteLine("Average Waiting Time: " + (double)list.Sum(p => p.WaitingTime) / count);
        Console.WriteLine("CPU Utilization: " + (double)list.Sum(p => p.BurstTime) / list.Sum(p => p.CompletionTime) * 100 + "%");
        Console.WriteLine("Throughput: " + (double)n / list.Sum(p => p.CompletionTime) * 1000); // Assuming time unit is in milliseconds
    }
}


    
