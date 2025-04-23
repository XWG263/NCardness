using NBC.ActionEditor;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace CCx
{
    [NBC.ActionEditor.CustomPreview(typeof(SpineAction))]
    public class SpineActionToPreview : ProcessorBase<SpineAction>
    {
        private SkeletonAnimation target;

        float time = 0;
        public override void Enter()
        {
            GameObject tt = null;
            tt = Transform.FindObjectOfType<SkeletonAnimation>()?.gameObject ?? null;
            if (target == null && tt == null)
            {
                tt = UnityEngine.Object.Instantiate(clip.spineObj.gameObject);
                tt.AddComponent<SkeletonPlayer>(); 
            }
            if (tt.TryGetComponent<SkeletonAnimation>(out SkeletonAnimation result))
            {
                target = result;
            }
            if (target == null) return;
            switch (clip.action)
            {
                case SpineActionEnum.ATK:
                    target.state.SetAnimation(0, "attack", false);
                    break;
                case SpineActionEnum.HIT:
                    target.state.SetAnimation(0, "hit", false);
                    break;
                case SpineActionEnum.Skill:
                    target.state.SetAnimation(0, "skill", false);
                    break;
                case SpineActionEnum.LANDING:
                    target.state.SetAnimation(0, "landing", false);
                    break;
                default:
                    break;
            }
             
        }
    }
}
