using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCx
{

    public enum SkillATKEnum
    {
        CRASH,
    }

    public class SkillData : AccessoryObjectData
    {
        public string skillName;
        public float m_CD = 5f;

        public SkillData(int entityId, int typeId, int ownerId, CampType ownerCamp,string name) : base(entityId, typeId, ownerId, ownerCamp)
        {
            skillName = name;
        }
    }
}

