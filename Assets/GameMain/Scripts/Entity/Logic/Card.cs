using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
using UnityEngine.UI;
using GameFramework.Event;

namespace CCx
{
    public class Card : TargetableObject
    {
        [SerializeField] HeroData heroData = null;
        
        public List<SkillEntity> skills = new List<SkillEntity>();

        [SerializeField] GameObject hpBar;
        [SerializeField] GameObject buff;
        [SerializeField] GameObject block;

        public bool isMyTurn;

        private IFsm<Card> fsm;

        private static int SERIAL_ID = 0;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            heroData = userData as HeroData;
            if (heroData == null)
            {
                return;
            }
            List<SkillData> heroDatas = heroData.GetSkillData;
            for (int i = 0; i < heroDatas.Count; i++)
            {
                GameEntry.Entity.ShowSkill(heroDatas[i]);
            }
            hpBar = this.transform.Find("HPBar").gameObject;
            buff = this.transform.Find("fire").gameObject;
            block = this.transform.Find("block").gameObject;
            fsm = GameEntry.Fsm.CreateFsm((SERIAL_ID++).ToString(), this, new CardState().state);
            fsm.Start<CardActionState>();

            SetHPRate();
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);

            if (childEntity is SkillEntity)
            {
                skills.Add((SkillEntity)childEntity);
                return;
            }

           
        }

        protected override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);

            if (childEntity is SkillEntity)
            {
                skills.Remove((SkillEntity)childEntity);
                return;
            }

        }

        protected override void OnDead(Entity attacker)
        {
            base.OnDead(attacker);

            //GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), m_AircraftData.DeadEffectId)
            //{
            //    Position = CachedTransform.localPosition,
            //});
            //GameEntry.Sound.PlaySound(m_AircraftData.DeadSoundId);
        }

        public HeroData GetData()
        {
            return heroData;
        }


        public void AddSkill(SkillData skillData)
        {
            var data = heroData.AddSkillData(skillData);
            if (data == null) return;
            GameEntry.Entity.ShowSkill(data);
        }

        public void SetHPRate()
        {
            hpBar.transform.localScale = new Vector3(Mathf.Max(heroData.HPRatio,0), 1, 1);
        }

        public void SetBuff(bool isOn)
        {
            buff.gameObject.SetActive(false);
            block.gameObject.SetActive(false);

            for (int i = 0; i < heroData.m_Buff.Length; i++)
            {
                if (heroData.m_Buff[i].name.Equals("fire"))
                {
                    buff.gameObject.SetActive(true);
                }
                if (heroData.m_Buff[i].name.Equals("block"))
                {
                    block.gameObject.SetActive(true);
                }
            }
        }
    }
}

