using NBC.ActionEditor;
using Spine.Unity;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace CCx
{
    [Name("Spine动画")]
    [Color(0.0f, 1f, 1f)]
    [Attachable(typeof(SpineTrack))]
    public class SpineAction : Clip
    {
        [MenuName("Spine对象")] public SkeletonAnimation spineObj = null;
        [MenuName("动作")] public SpineActionEnum action = SpineActionEnum.ATK;

        public override string Info => $"动作";

        public override float Length
        {
            get => length;
            set => length = value;
        }


        public override bool IsValid => true;

        private ActionTrack Track => (ActionTrack)Parent;
    }

}
