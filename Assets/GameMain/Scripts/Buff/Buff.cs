using System;
namespace CCx
{
    [Serializable]
    public class Buff
    {
        public string name = string.Empty;
        public float time = 0;//持续时间
        public float triggerTime = 0;//触发时间

        public void Reset()
        {
            name = string.Empty;
            time = 0;
            triggerTime = 0;
        }

        public Buff()
        {
            Reset();
        }
    }
}
