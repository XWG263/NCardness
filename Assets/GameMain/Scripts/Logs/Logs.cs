using GameFramework.Event;
using GameFramework.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using Spine.Unity;

namespace CCx
{
    public class Logs
    {

    }

    public class EffectLos
    {
        public TargetableObjectData target;
    }

    public abstract class EffectLog: Logs
    {
        public abstract void Do(EffectLos log);
    }//表现
    public class DamageEffectLog : EffectLos
    {
        public float vlaue;
    }

    public class BuffEffectLog : EffectLos 
    {
        public string name;
        public bool isADD;
        public float intervalTime;
    }


    public class SkillEffectLog : EffectLog
    {
        public TargetableObjectData target;

        public List<EffectLos> effectList = new List<EffectLos>();
        public override void Do(EffectLos log)
        {
            effectList.Add(log);
        }
    }

    public class LogManager
    {
        public LogManager()
        {
            GameEntry.Event.Subscribe(AddDemageEventArgs.EventId, OnFinalSuccess);
        }

        static Stack<EffectLog> effectStack = new Stack<EffectLog>();
        static EffectLog currentEffect;
        public static void ADDDamage(TargetableObjectData TargetableObjectData,float value)
        {
            var log = new DamageEffectLog();
            log.target = TargetableObjectData;
            log.vlaue = value;

            currentEffect.Do(log);
        }

        public static void ADDBuff(TargetableObjectData TargetableObjectData, string value, bool add, float interval)
        {
            var log = new BuffEffectLog();
            log.target = TargetableObjectData;
            log.name = value;
            log.isADD = add;
            log.intervalTime = interval;

            currentEffect.Do(log);
        }

        public static void UseSkill(TargetableObjectData TargetableObjectData,SkillData skill)
        {
            var skillE = new SkillEffectLog();
            skillE.target = TargetableObjectData;
            currentEffect = skillE;
            effectStack.Push(skillE);
        }

        public static void OnFinalSuccess(object sender, GameEventArgs e)
        {
            var currentEffet = (effectStack.Pop());
           
            foreach (var item in ((SkillEffectLog)currentEffet).effectList)
            {
                int id = item.target.Id;
                var entity = GameEntry.Entity.GetEntity(id);
                Card owner = entity.Logic as Card;

                if (item is DamageEffectLog)
                {
                    owner.SetHPRate();
                }
                if (item is BuffEffectLog)
                {
                    var t = (BuffEffectLog)item;
                    if (string.IsNullOrEmpty(t.name))
                    {
                        t.target.ResetBuff();
                    }
                    else if(t.isADD)
                    {
                        t.target.AddBuff(t.name,t.intervalTime);
                    }
                    else
                    {
                        t.target.RemoveBuff(t.name);
                        if (t.name.Equals("block"))
                        {
                            owner.GetComponentInChildren<SkeletonAnimation>().AnimationState.SetAnimation(0, "landing", false); ;
                            owner.GetComponentInChildren<SkeletonAnimation>().AnimationState.AddAnimation(0, "idle", true, 0);
                        }
                    }
                }
            }
         
        }
    }
}

