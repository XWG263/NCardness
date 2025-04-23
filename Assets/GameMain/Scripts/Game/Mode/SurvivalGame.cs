using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using Cysharp.Threading.Tasks;
using System;

namespace CCx
{
    public class SurvivalGame : GameBase
    {
        private float m_ElapseSeconds = 0f;

        public Card heroCard;

        public List<Card> enemyCard = new List<Card>();
        TimelineGraphProcessor tt;
        public float time = 0;



        public override GameMode GameMode
        {
            get
            {
                return GameMode.Survival;
            }
        }
        public override void Initialize()
        {
            base.Initialize();

            GameEntry.Entity.ShowHero(new HeroData(10000,10000,CampType.Hero));
            
            GameEntry.Entity.ShowEnemy(new HeroData(20000,20000,CampType.Enemy));

            new LogManager();
        }

        public override async void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);

            if (AIUtility.turnSkillCount == 3)
            {
                time += Time.deltaTime;
                if (time > 1)
                {
                    AIUtility.currentRound = AIUtility.currentRound == TurnRound.CURRENT ? TurnRound.Enemy : TurnRound.CURRENT;
                    AIUtility.turnSkillCount = 0;
                    if (AIUtility.currentRound == TurnRound.Enemy)
                    {
                        GameEntry.SkillSlot.HideSlot();
                    }
                    else
                    {
                        GameEntry.SkillSlot.RefreshSkillSlot(heroCard);
                    }
                    time = 0;
                }
               
            }
            heroCard.isMyTurn = AIUtility.currentRound == TurnRound.CURRENT;

            foreach (var item in enemyCard)
            {
                item.isMyTurn = AIUtility.currentRound == TurnRound.Enemy;
            }


            if (Input.GetMouseButtonDown(1))
            {
                AIUtility.turnSkillCount = 3;
            }

            if (!GameOver)
            {

            }
           // Debug.Log("OVER");
        }

        protected override void OnShowEntitySuccess(object sender,GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne.EntityLogicType == typeof(HeroCard))
            {
                heroCard = (Card)ne.Entity.Logic;
                GameEntry.SkillSlot.RefreshSkillSlot(heroCard);
            }
            if (ne.EntityLogicType == typeof(EnemyCard))
            {
                var tt = (Card)ne.Entity.Logic;
                enemyCard.Add(tt);
                tt.isMyTurn = false;
            }
        }
    }
}
