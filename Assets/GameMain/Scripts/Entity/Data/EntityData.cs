using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMath = System.Single;

namespace CCx
{
    [Serializable]
    public class EntityData
    {
        [SerializeField]
        private int m_Id = 0;

        [SerializeField]
        private int m_TypeId = 0;

        [SerializeField]
        private Vector3 m_Position = Vector3.zero;

        [SerializeField]
        private FMath attackInterval = 1.23f;

        [SerializeField]
        private FMath lastAttackTime = 0;

        public EntityData(int entityId, int typeId)
        {
            m_Id = entityId;
            m_TypeId = typeId;
        }

        /// <summary>
        /// 实体编号。
        /// </summary>
        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 实体类型编号。
        /// </summary>
        public int TypeId
        {
            get
            {
                return m_TypeId;
            }
        }

        /// <summary>
        /// 实体位置。
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }
        /// <summary>
        /// 间隔
        /// </summary>
        public FMath AttackInterval
        {
            get
            {
                return attackInterval;
            }
            set
            {
                attackInterval = value;
            }
        }

        public FMath LastAttackTime
        {
            get
            {
                return lastAttackTime;
            }
            set
            {
                lastAttackTime = value;
            }
        }
    }
}

