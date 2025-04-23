using NBC.ActionEditor;
namespace CCx
{
    public interface IDirectableTimePointerProcessor
    {
        ClipProcessor target { get; }
        float time { get; }
        void TriggerForward(float currentTime, float previousTime);
        void TriggerBackward(float currentTime, float previousTime);
        void Update(float currentTime, float previousTime);
    }

    public struct StartTimePointerProcessor : IDirectableTimePointerProcessor
    {
        private bool triggered;
        private float lastTargetStartTime;
        public ClipProcessor target { get; private set; }
        float IDirectableTimePointerProcessor.time => target.directable.StartTime;


        public StartTimePointerProcessor(ClipProcessor target)
        {
            this.target = target;
            triggered = false;
            lastTargetStartTime = target.directable.StartTime;
        }
        void IDirectableTimePointerProcessor.TriggerForward(float currentTime, float previousTime)
        {
            if (currentTime >= target.directable.StartTime)
            {
                if (!triggered)
                {
                    triggered = true;
                    target.Enter();
                    target.Update(target.ToLocalTime(currentTime), 0);
                }
            }
        }

        void IDirectableTimePointerProcessor.Update(float currentTime, float previousTime)
        {
            if (currentTime >= target.directable.StartTime && currentTime < target.directable.EndTime &&
                currentTime > 0)
            {
                var deltaMoveClip = target.directable.StartTime - lastTargetStartTime;
                var localCurrentTime = target.ToLocalTime(currentTime);
                var localPreviousTime = target.ToLocalTime(previousTime + deltaMoveClip);

                target.Update(localCurrentTime, localPreviousTime);
                lastTargetStartTime = target.directable.StartTime;
            }
        }

        void IDirectableTimePointerProcessor.TriggerBackward(float currentTime, float previousTime)
        {
            if (currentTime < target.directable.StartTime || currentTime <= 0)
            {
                if (triggered)
                {
                    triggered = false;
                    target.Update(0, target.ToLocalTime(previousTime));
                    target.Reverse();
                }
            }
        }
    }

    public struct EndTimePointerProcessor : IDirectableTimePointerProcessor
    {
        private bool triggered;
        public ClipProcessor target { get; private set; }
        float IDirectableTimePointerProcessor.time => target.directable.EndTime;

        public EndTimePointerProcessor(ClipProcessor target)
        {
            this.target = target;
            triggered = false;
        }

        void IDirectableTimePointerProcessor.TriggerForward(float currentTime, float previousTime)
        {
            if (currentTime >= target.directable.EndTime)
            {
                if (!triggered)
                {
                    triggered = true;
                    target.Update(target.GetLength(), target.ToLocalTime(previousTime));
                    target.Exit();
                }
            }
        }


        void IDirectableTimePointerProcessor.Update(float currentTime, float previousTime)
        {

        }


        void IDirectableTimePointerProcessor.TriggerBackward(float currentTime, float previousTime)
        {
            if (currentTime < target.directable.EndTime || currentTime <= 0)
            {
                if (triggered)
                {
                    triggered = false;
                    target.ReverseEnter();
                    target.Update(target.ToLocalTime(currentTime), target.GetLength());
                }
            }
        }
    }
}