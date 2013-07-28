using System;
using System.Diagnostics.Contracts;

namespace SquidEyes.Generic
{
    public class ProgressArgs<T> : EventArgs
    {
        public ProgressArgs(int totalSteps, int stepCount, T details)
        {
            Contract.Requires(totalSteps > 0);
            Contract.Requires(stepCount <= totalSteps);
            Contract.Requires(!details.IsDefault());

            TotalSteps = totalSteps;
            StepCount = stepCount;
            Details = details;
        }

        public int TotalSteps { get; private set; }
        public int StepCount { get; private set; }
        public T Details { get; private set; }

        public bool IsFinished
        {
            get
            {
                return (StepCount == TotalSteps);
            }
        }

        public int Progress
        {
            get
            {
                return (int)((StepCount /
                    (double)TotalSteps) * 100.0);
            }
        }
    }
}
