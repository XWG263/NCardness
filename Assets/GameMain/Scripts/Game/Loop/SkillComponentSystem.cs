using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCx
{
    #region 技能效果
    public abstract class SkillEffect
    {
        public abstract void Run(TargetableObjectData owner,TargetableObjectData aim,SkillEntity skill);
    }

    public class SkillEffect_Demage : SkillEffect
    {
        public Func<TargetableObjectData, TargetableObjectData, float, float> demage { get; set; }//释法，受击，等级

        public override void Run(TargetableObjectData owner, TargetableObjectData aim, SkillEntity skill)
        {
            float value = demage(owner,aim,1);
            bool isBlock = false; ;
            for (int i = 0; i < aim.m_Buff.Length; i++)
            {
                if (aim.m_Buff[i].name.Equals("block"))
                {
                    isBlock = true;
                    continue;
                }
            }
            if (isBlock)
            {
                value = -1;
                LogManager.ADDBuff(aim, "block", false, 0);
            }
            else
            {
                aim.HP -= (int)value;
                LogManager.ADDDamage(aim, value);
            }
        }
    }
    public class SkillEffect_Treat : SkillEffect
    {
        public Func<TargetableObjectData, TargetableObjectData, float, float> demage { get; set; }//释法，受击，等级

        public override void Run(TargetableObjectData owner, TargetableObjectData aim, SkillEntity skill)
        {
            float value = demage(owner, aim, 1);
            aim.HP += (int)value;
            LogManager.ADDDamage(aim, value);
        }
    }

    public class SkillEffect_Buff : SkillEffect
    {
        public Func<TargetableObjectData, TargetableObjectData, float, float> demage { get; set; }//释法，受击，等级
        public string buffName;
        public float buffTime;
        public override void Run(TargetableObjectData owner, TargetableObjectData aim, SkillEntity skill)
        {
            float value = demage(owner, aim, 1);
            aim.HP -= (int)value;
            LogManager.ADDDamage(aim, value);
            LogManager.ADDBuff(aim, buffName, true, buffTime);
        }
    }
    public class SkillEffect_Buff1 : SkillEffect
    {
        public Func<TargetableObjectData, TargetableObjectData, float, float> demage { get; set; }//释法，受击，等级
        public override void Run(TargetableObjectData owner, TargetableObjectData aim, SkillEntity skill)
        {
            LogManager.ADDBuff(owner, string.Empty, true,0f);
        }
    }
    #endregion

    #region 技能动作
    public delegate void RefAction< T2, T3>(T2 arg2, ref T3 arg3);
    /// <summary>
    /// 技能
    /// </summary>
    public class SkillAction
    {
        public bool hit { get; set; }//命中
        public bool hp { get; set; }//治疗
        public bool dp { get; set; }//伤害

        public List<SkillEffect> skillEffectList;
        public RefAction<TargetableObjectData, List<TargetableObjectData>> preAim { set; get; }
    }
    public class Skill
    {
        /// <summary>
        /// 释法预判条件
        /// </summary>
        public Func<bool> determination { get; set; } = ()=> true;

        public List<SkillAction> skillList { get; set; }

        public bool HP
        {
            get
            {
                foreach (var item in skillList)
                {
                    if (item.hp)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool DP
        {
            get
            {
                foreach (var item in skillList)
                {
                    if (item.dp)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public virtual void UseSkill(TargetableObjectData owner,SkillEntity skill)
        {
            //Debug.Log(owner.TypeId);
            GameEntry.Event.Fire(this, ReleaseSkillEventArgs.Create(new SkillEvent(owner, skill)));
            LogManager.UseSkill(owner,skill.GetData());
            List<TargetableObjectData> aimList = new List<TargetableObjectData>();
            foreach (var item in skillList)
            {
                item.preAim(owner,ref aimList);

                foreach (var aim in aimList)
                {
                    foreach (var itemxx in item.skillEffectList)
                    {
                        itemxx.Run(owner, aim, skill);
                    }
                }
            }
        }
    }

    public class Skill1 : Skill
    {
        public float CD = 0;
    }
    #endregion

    #region 敌人预载


    public class Aim
    {
        public static Dictionary<string, List<TargetableObjectData>> TargetableObjectDic = new Dictionary<string, List<TargetableObjectData>>();

        public static void Enemy(TargetableObjectData owner, ref List<TargetableObjectData> aimList)
        {
            foreach (var item in SkillComponentSystem.All)
            {
                if (item.Key != owner.Camp)
                {
                    aimList = item.Value;
                    return;
                }
            }
        }
        public static void Self(TargetableObjectData owner, ref List<TargetableObjectData> aimList)
        {
            foreach (var item in SkillComponentSystem.All)
            {
                if (item.Key == owner.Camp)
                {
                    aimList = item.Value;
                    return;
                }
            }
        }
        public static void SetData(string key,TargetableObjectData data)
        {
            if (TargetableObjectDic == null)
            {
                TargetableObjectDic = new Dictionary<string, List<TargetableObjectData>>();
            }
            if (!TargetableObjectDic.ContainsKey(key))
            {
                TargetableObjectDic.Add(key, new List<TargetableObjectData>());
            }
        }
    }
    #endregion
    public class SkillComponentSystem
    {
        public static Dictionary<CampType, List<TargetableObjectData>> All = new Dictionary<CampType, List<TargetableObjectData>>();

        public static void Add(TargetableObjectData owner)
        {
            if (!All.ContainsKey(owner.Camp))
            {
                All.Add(owner.Camp, new List<TargetableObjectData>());
            }
            All[owner.Camp].Add(owner);
        }

        public static void Remove(TargetableObjectData owner)
        {
            if (!All[owner.Camp].Contains(owner))
            {
                return;
            }
            All[owner.Camp].Remove(owner);
        }

        public static Dictionary<string, Skill> SkillDic = new Dictionary<string, Skill>()
        {
            [SkillATKEnum.CRASH.ToString()] = new Skill1
            {
                CD = 2f,
                skillList = new List<SkillAction>()
                {
                    new SkillAction()
                    {
                        preAim = Aim.Enemy,
                        skillEffectList = new List<SkillEffect>()
                        {
                            new SkillEffect_Demage()
                            {
                                demage =(atk,hit,level) => atk.AtkValue * level,
                            }
                        }
                    }
                }
            },
            ["1111"] = new Skill1
            {
                CD = 2f,
                skillList = new List<SkillAction>()
                {
                    new SkillAction()
                    {
                        preAim = Aim.Enemy,
                        skillEffectList = new List<SkillEffect>()
                        {
                            new SkillEffect_Demage()
                            {
                                demage =(atk,hit,level) => atk.AtkValue * level,
                            }
                        }
                    }
                }
            }
            ,
            ["1112"] = new Skill1
            {
                CD = 2f,
                skillList = new List<SkillAction>()
                {
                    new SkillAction()
                    {
                        preAim = Aim.Enemy,
                        skillEffectList = new List<SkillEffect>()
                        {
                            new SkillEffect_Treat()
                            {
                                demage =(atk,hit,level) => atk.AtkValue * level,
                            }
                        }
                    }
                }
            }
            ,
            ["1113"] = new Skill1
            {
                CD = 2f,
                skillList = new List<SkillAction>()
                {
                    new SkillAction()
                    {
                        preAim = Aim.Enemy,
                        skillEffectList = new List<SkillEffect>()
                        {
                            new SkillEffect_Buff()
                            {
                                buffName = "fire",
                                buffTime = 10,
                                demage =(atk,hit,level) => atk.AtkValue * level,
                            }
                        }
                    }
                }
            }
            ,
            ["1114"] = new Skill1
            {
                CD = 2f,
                skillList = new List<SkillAction>()
                {
                    new SkillAction()
                    {
                        preAim = Aim.Self,
                        skillEffectList = new List<SkillEffect>()
                        {
                            new SkillEffect_Buff1()
                            {
                                demage =(atk,hit,level) => atk.AtkValue * level,
                            }
                        }
                    }
                }
            }
            ,
            ["1115"] = new Skill1
            {
                CD = 2f,
                skillList = new List<SkillAction>()
                {
                    new SkillAction()
                    {
                        preAim = Aim.Self,
                        skillEffectList = new List<SkillEffect>()
                        {
                            new SkillEffect_Buff()
                            {
                                demage =(atk,hit,level) => atk.AtkValue * level,
                                buffName = "block",
                                buffTime = 10,
                            }
                        }
                    }
                }
            }
            ,
            ["1116"] = new Skill1
            {
                CD = 2f,
                skillList = new List<SkillAction>()
                {
                    new SkillAction()
                    {
                        preAim = Aim.Enemy,
                        skillEffectList = new List<SkillEffect>()
                        {
                            new SkillEffect_Demage()
                            {
                                demage =(atk,hit,level) => atk.AtkValue * level,
                            }
                        }
                    }
                }
            }
        };
    }

}
