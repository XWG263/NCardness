using GameFramework.ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace CCx
{
    public class SkillSlotComponent : GameFrameworkComponent
    {
        [SerializeField]
        private GameObject skillSlot = null;

        [SerializeField]
        private Transform m_HPBarInstanceRoot = null;

        [SerializeField]
        private int m_InstancePoolCapacity = 16;

        private IObjectPool<SkillSlotObject> m_SkillSlotItemObjectPool = null;
        private List<CardVisual> m_ActiveSkillSlotItems = null;
        [SerializeField]
        private Canvas m_CachedCanvas = null;

        public TimelineGraphProcessor timelineGraphProcessor;

        public float time;
        private void Start()
        {
            if (m_HPBarInstanceRoot == null)
            {
                return;
            }


            m_SkillSlotItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<SkillSlotObject>("SkillSlotItem", m_InstancePoolCapacity);
            m_ActiveSkillSlotItems = new List<CardVisual>();
        }

        private void OnDestroy()
        {
        }

        private void Update()
        {
            if (timelineGraphProcessor != null)
            {
                time += Time.deltaTime;
                timelineGraphProcessor.Sample(time);
            }

            for (int i = m_ActiveSkillSlotItems.Count - 1; i >= 0; i--)
            {
                CardVisual hpBarItem = m_ActiveSkillSlotItems[i];
                hpBarItem.OnUpdate();
                //if ()
                //{
                //    continue;
                //}

                //HideHPBar(hpBarItem);
            }
        }

        public void ShowSkillSlot(Entity entity)
        {
            if (entity == null)
            {
                Log.Warning("Entity is invalid.");
                return;
            }

            CardVisual skillSlot  = CreateSkillSlotItem();
            m_ActiveSkillSlotItems.Add(skillSlot);
            HorizontalCardHolder.instance.AddCard(skillSlot);


            skillSlot.Init(entity, m_CachedCanvas);
        }

        private void HideSkillSlot(CardVisual card)
        {
            GameEntry.Entity.HideEntity(card.Owner);
            m_ActiveSkillSlotItems.Remove(card);
            m_SkillSlotItemObjectPool.Unspawn(card.transform.parent.gameObject);
            card.Reset();
        }

        private CardVisual GetActiveSkillSlotItem(Entity entity)
        {
            if (entity == null)
            {
                return null;
            }

            for (int i = 0; i < m_ActiveSkillSlotItems.Count; i++)
            {
                if (m_ActiveSkillSlotItems[i].Owner == entity)
                {
                    return m_ActiveSkillSlotItems[i];
                }
            }

            return null;
        }

        private CardVisual CreateSkillSlotItem()
        {
            GameObject obj = null;
            SkillSlotObject hpBarItemObject = m_SkillSlotItemObjectPool.Spawn();
            if (hpBarItemObject != null)
            {
                obj = (GameObject)hpBarItemObject.Target;
            }
            else
            {
                obj = Instantiate(skillSlot, m_HPBarInstanceRoot);
                m_SkillSlotItemObjectPool.Register(SkillSlotObject.Create(obj), true);
            }
            obj.SetActive(true);
            return obj.GetComponentInChildren<CardVisual>();
        }

        public void UseSkillCard(CardVisual card)
        {
            if (card.Owner == null)
            {
                throw new System.Exception($"{card.name}:Owner NULL");
            }
            switch (card.Owner)
            {
                case SkillEntity:
                    var tt = (SkillEntity)card.Owner;
                    tt.TryAttack();
                    break;
            }
            HideSkillSlot(card);
        }
        int entityId = 0;
        public void RefreshSkillSlot(Card card)
        {
            HideSlot();

            card.AddSkill(new SkillData(GameEntry.Entity.GenerateSerialId(), ++entityId, 10000, CampType.Hero, (1111).ToString()));
            card.AddSkill(new SkillData(GameEntry.Entity.GenerateSerialId(), ++entityId, 10000, CampType.Hero, (1112).ToString()));
            card.AddSkill(new SkillData(GameEntry.Entity.GenerateSerialId(), ++entityId, 10000, CampType.Hero, (1113).ToString()));
            card.AddSkill(new SkillData(GameEntry.Entity.GenerateSerialId(), ++entityId, 10000, CampType.Hero, (1111).ToString()));
            card.AddSkill(new SkillData(GameEntry.Entity.GenerateSerialId(), ++entityId, 10000, CampType.Hero, (1112).ToString()));
            card.AddSkill(new SkillData(GameEntry.Entity.GenerateSerialId(), ++entityId, 10000, CampType.Hero, (1113).ToString()));
            card.AddSkill(new SkillData(GameEntry.Entity.GenerateSerialId(), ++entityId, 10000, CampType.Hero, (1114).ToString()));
        }

        public void HideSlot()
        {
            for (int i = m_ActiveSkillSlotItems.Count - 1; i >= 0; --i)
            {
                HideSkillSlot(m_ActiveSkillSlotItems[i]);
            }
        }
    }
}

