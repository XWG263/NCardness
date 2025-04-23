using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace CCx 
{
    public class SkillSlotObject : ObjectBase
    {
        public static SkillSlotObject Create(object target)
        {
            SkillSlotObject SkillSlotObject = ReferencePool.Acquire<SkillSlotObject>();
            SkillSlotObject.Initialize(target);
            return SkillSlotObject;
        }

        protected override void Release(bool isShutdown)
        {
            GameObject skillSlot = (GameObject)Target;
            if (skillSlot == null)
            {
                return;
            }

            Object.Destroy(skillSlot.gameObject);
        }
    }
}


