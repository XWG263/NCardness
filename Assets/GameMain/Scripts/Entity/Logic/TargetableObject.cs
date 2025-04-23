using UnityEngine;
using UnityGameFramework.Runtime;
using System.Collections.Generic;
using GameFramework.Event;

namespace CCx
{
    public class TargetableObject : Entity
    {
        [SerializeField]
        private TargetableObjectData m_TargetableObjectData = null;

        public bool IsDead
        {
            get
            {
                return m_TargetableObjectData.HP <= 0;
            }
        }

        public void ApplyDamage(Entity attacker, int damageHP)
        {
            //float fromHPRatio = m_TargetableObjectData.HPRatio;
            //m_TargetableObjectData.HP -= damageHP;
            //float toHPRatio = m_TargetableObjectData.HPRatio;
            //if (fromHPRatio > toHPRatio)
            //{
            //    GameEntry.HPBar.ShowHPBar(this, fromHPRatio, toHPRatio);
            //}

            //if (m_TargetableObjectData.HP <= 0)
            //{
            //    OnDead(attacker);
            //}
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_TargetableObjectData = userData as TargetableObjectData;
            if (m_TargetableObjectData == null)
            {
                Log.Error("Targetable object data is invalid.");
                return;
            }
            SkillComponentSystem.Add(m_TargetableObjectData);
        }

        protected virtual void OnDead(Entity attacker)
        {
            SkillComponentSystem.Remove(m_TargetableObjectData);
            GameEntry.Entity.HideEntity(this);
        }
    }
}

