using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCx 
{
    public class AIUtility
    {
        public static TurnRound currentRound = TurnRound.CURRENT;

        public static int turnSkillCount = 0;

        public static bool SkillCondition(SkillEntity skill) 
        {
            bool isOn = false;
            if (Time.time <= skill.GetData().LastAttackTime)
            {
                isOn = false;
            }
            else
            {
                ++turnSkillCount; 
                isOn = true;
            }
            return isOn;
        }

        public static bool CardAtkInterval(Card  card)
        {
            bool isOn = false;
            if (Time.time <= card.GetData().LastAttackTime)
            {
                isOn = false;
            }
            else
            {
                isOn = true;
            }
            return isOn;
        }
    }
}



