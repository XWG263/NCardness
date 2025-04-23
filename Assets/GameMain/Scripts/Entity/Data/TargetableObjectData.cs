using System;
using UnityEngine;

namespace CCx
{
    [Serializable]
    public class TargetableObjectData : EntityData
    {
        [SerializeField]
        private CampType m_Camp = CampType.Unknown;

        [SerializeField]
        private int m_HP = 100;

        [SerializeField]
        private float m_AtkValue = 10;

        [SerializeField]
        public Buff[] m_Buff;

        public TargetableObjectData(int entityId, int typeId, CampType camp)
            : base(entityId, typeId)
        {
            m_Camp = camp;
            m_HP = 100;
            m_Buff = new Buff[7];
            for (int i = 0; i < 7; i++)
            {
                m_Buff[i] = new Buff();
            }
        }

        /// <summary>
        /// 角色阵营。
        /// </summary>
        public CampType Camp
        {
            get
            {
                return m_Camp;
            }
        }

        /// <summary>
        /// 当前生命。
        /// </summary>
        public int HP
        {
            get
            {
                return m_HP;
            }
            set
            {
                m_HP = value;
            }
        }

        /// <summary>
        /// 最大生命。
        /// </summary>
        public int MaxHP { get; private set; } = 100;


        /// <summary>
        /// 生命百分比。
        /// </summary>
        public float HPRatio
        {
            get
            {
                return MaxHP > 0 ? (float)HP / MaxHP : 0f;
            }
        }

        public float AtkValue
        {
            get
            {
                return m_AtkValue;
            }
            set
            {
                m_AtkValue = value;
            }
        }

        public void ResetBuff()
        {
            for (int i = 0; i < m_Buff.Length; i++)
            {
                m_Buff[i].name = string.Empty;
                m_Buff[i].time = 0;
                m_Buff[i].triggerTime = 0;
            }
        }

        public void AddBuff(string name,float interval)
        {
            for (int i = 0; i < m_Buff.Length; i++)
            {
                if (m_Buff[i].name.Equals(string.Empty))
                {
                    m_Buff[i].name = name;
                    m_Buff[i].time = interval;
                    m_Buff[i].triggerTime = Time.time;
                    return;
                }
            }
        }

        public void RemoveBuff(string name)
        {
            for (int i = 0; i < m_Buff.Length; i++)
            {
                if (m_Buff[i].name.Equals(name))
                {
                    m_Buff[i].Reset();
                    return;
                }
            }
        }
    }
}

