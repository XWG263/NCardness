using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NBC.ActionEditor;
using Spine.Unity;

namespace CCx
{
    [CommandProcessor(typeof(SpineAction))]
    public class SpineToProcessor : ClipProcessor<SpineAction>
    {
        public SpineAction data;
        private SkeletonAnimation currentSklt;
        public override void Enter()
        {
            data = TData;

            data.spineObj = ((SkillEntity)owner).GetOwner().GetComponentInChildren<SkeletonAnimation>();

            currentSklt = data.spineObj;

            switch (data.action)
            {
                case SpineActionEnum.ATK:
                    currentSklt.AnimationState.SetAnimation(0,"attack" , false);
                    break;
                case SpineActionEnum.HIT:
                    currentSklt.AnimationState.SetAnimation(0, "hit", false);
                    break;
                case SpineActionEnum.Skill:
                    currentSklt.AnimationState.SetAnimation(0, "skill", false);
                    break;
                case SpineActionEnum.LANDING:
                    currentSklt.AnimationState.SetAnimation(0, "landing", false);
                    break;
                default:
                    break;
            }
            currentSklt.AnimationState.AddAnimation(0,"idle",true,0);
        }
    }
}
