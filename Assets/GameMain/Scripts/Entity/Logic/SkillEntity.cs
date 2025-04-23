using GameFramework;
using System;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace CCx
{
    public class SkillEntity : Entity
    {
        private const string AttachPoint = "Weapon Point";

        [SerializeField]
        private SkillData skillData = null;

        private Skill skill;

        private Card ownerCard;

        public string SkillName => skillData?.skillName ?? string.Empty;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            skillData = userData as SkillData;
            if (skillData == null)
            {
                Log.Error("Weapon data is invalid.");
                return;
            }
            skill = SkillComponentSystem.SkillDic[skillData.skillName];
            GameEntry.Entity.AttachEntity(Entity, skillData.OwnerId, AttachPoint);
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
            ownerCard = (Card)parentEntity;
            CachedTransform.localPosition = Vector3.zero;
            if (ownerCard.GetData().Camp == CampType.Enemy) return;

            GameEntry.SkillSlot.ShowSkillSlot(this);
        }

        public bool TryAttack()
        {
            if (!AIUtility.SkillCondition(this)) return false;
            TargetableObjectData owner = ownerCard.GetData();
            skill.UseSkill(owner, this);
            skillData.LastAttackTime = Time.time + skillData.m_CD;


            GameEntry.SkillSlot.time = 0;
            SkillConfig skillConfig = new SkillConfig
            {
                EventName = SkillName
            };
            GameEntry.SkillSlot.timelineGraphProcessor = new TimelineGraphProcessor(skillConfig.EventConfig);
            GameEntry.SkillSlot.timelineGraphProcessor.Play(this);
            return true;
        }

        public SkillData GetData()
        {
            return skillData;
        }

        public Card GetOwner()
        {
            return ownerCard;
        }


    }
}
