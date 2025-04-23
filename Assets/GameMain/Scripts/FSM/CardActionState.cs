using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<CCx.Card>;
using UnityGameFramework.Runtime;
using System.Collections.Generic;
using System;

namespace CCx
{
    public class CardState
    {
        public List<FsmState<Card>> state = new List<FsmState<Card>>();
        public CardState()
        {
            state.Add(new CardActionState());
            state.Add(new AtkActionState());
            state.Add(new HitActionState());
        }
    }

    public class CardActionState :FsmState<Card>
    {
        float time = 0;
        protected override void OnInit(ProcedureOwner fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(ProcedureOwner fsm)
        {
            base.OnEnter(fsm);
        }

        protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            Card owner = fsm.Owner;
            Buff[] buff = owner.GetData().m_Buff;
            for (int i = 0; i < buff.Length; i++)
            {
                if (buff[i] == null) continue;
                owner.SetBuff(true);
                if ( buff[i].name.Equals("fire") && buff[i].triggerTime + buff[i].time > Time.time)
                {
                   
                    time += Time.deltaTime;
                    if (time > 1)
                    {
                        owner.GetData().HP -= 2;
                        owner.SetHPRate();
                        time = 0;
                    }
                }
                if (buff[i].triggerTime + buff[i].time <= Time.time)
                {
                    buff[i].Reset();
                }
            }
            if (owner.skills == null || owner.skills.Count == 0 || !owner.isMyTurn || owner.GetData().Camp == CampType.Hero)
            {
                return;
            }//TODO：出招判断

            if (!AIUtility.CardAtkInterval(owner)) return;

            owner.GetData().LastAttackTime = Time.time + owner.GetData().AttackInterval;
            ChangeState<AtkActionState>(fsm);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

        }

        protected override void OnDestroy(ProcedureOwner fsm)
        {
            base.OnDestroy(fsm);
        }
    }

    public class AtkActionState : FsmState<Card>
    {
        protected override void OnInit(ProcedureOwner fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(ProcedureOwner fsm)
        {
            base.OnEnter(fsm);
            Card currentCard = (Card)(fsm.Owner);

            foreach (var item in currentCard.skills)
            {
                if (item.TryAttack())
                {
                    return;
                }
            }
        }

        protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            ChangeState<CardActionState>(fsm);

        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            //ChangeState<CardActionState>(fsm);
        }

        protected override void OnDestroy(ProcedureOwner fsm)
        {
            base.OnDestroy(fsm);
        }
    }

    public class HitActionState : FsmState<Card>
    {
        protected override void OnInit(ProcedureOwner fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(ProcedureOwner fsm)
        {
            base.OnEnter(fsm);
            Card currentCard = (Card)fsm.Owner;
        }

        protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }

        protected override void OnDestroy(ProcedureOwner fsm)
        {
            base.OnDestroy(fsm);
        }
    }

}
