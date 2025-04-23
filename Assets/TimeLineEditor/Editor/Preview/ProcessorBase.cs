namespace NBC.ActionEditor
{
    public abstract class ProcessorBase<T> : ProcessorBase where T : IDirectable
    {
        public T clip => (T)directable;
    }

    public abstract class ProcessorBase
    {
        public IDirectable directable;

        public void SetTarget(IDirectable t)
        {
            directable = t;
        }

        public virtual void Enter()
        {
            if (directable == null) return;
            directable.Enter();
        }

        public virtual void Exit()
        {
            if (directable == null) return;
            directable.Exit();
        }

        public virtual void ReverseEnter()
        {
            if (directable == null) return;
            directable.ReverseEnter();
        }

        public virtual void Reverse()
        {
            if (directable == null) return;
            directable.Reverse();
        }


        public virtual void Update(float time, float previousTime)
        {
            if (directable == null) return;
            directable.Update(time,previousTime);
        }
    }
}