using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCx
{
    public class HeroData : TargetableObjectData
    {
        [SerializeField]
        private List<SkillData> skillData = new List<SkillData>();

        public HeroData(int entityId, int typeId, CampType camp) : base(entityId, typeId, camp)
        {
            if (camp == CampType.Enemy)
            {
                // AddSkillData(new SkillData(GameEntry.Entity.GenerateSerialId(), ++entityId, Id, camp,"1"));
                //AddSkillData(new SkillData(GameEntry.Entity.GenerateSerialId(), ++entityId, Id, camp,"2"));
                //AddSkillData(new SkillData(GameEntry.Entity.GenerateSerialId(), ++entityId, Id, camp,"3"));
                AddSkillData(new SkillData(GameEntry.Entity.GenerateSerialId(), ++entityId, Id, camp,"1111"));
                AddSkillData(new SkillData(GameEntry.Entity.GenerateSerialId(), ++entityId, Id, camp,"1115"));
                AddSkillData(new SkillData(GameEntry.Entity.GenerateSerialId(), ++entityId, Id, camp,"1113"));
            }
            //AddSkillData(new SkillData(GameEntry.Entity.GenerateSerialId(), entityId, Id, camp, SkillATKEnum.CRASH.ToString()));

        }

        public SkillData AddSkillData(SkillData data)
        {
            if (skillData == null)
            {
                skillData = new List<SkillData>();
            }
            if (skillData.Contains(data))
            {
                return null;
            }

            skillData.Add(data);
            return data;
        }

        public List<SkillData> GetSkillData
        {
            get
            {
                return skillData;
            }
        }
    }
}

