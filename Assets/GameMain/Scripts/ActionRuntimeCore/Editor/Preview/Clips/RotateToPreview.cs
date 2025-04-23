using NBC.ActionEditor;

using UnityEngine;

namespace CCx
{
    /// <summary>
    /// 旋转角度预览
    /// </summary>
    [CustomPreview(typeof(RotateTo))]
    public class RotateToPreview : ProcessorBase<RotateTo>
    {
        private Vector3 originalRot;

        public override void Update(float time, float previousTime)
        {
            var target = originalRot + clip.targetRotation;
            ModelSampler.EditModel.transform.localEulerAngles =
                Easing.Ease(clip.interpolation, originalRot, target, time / clip.Length);
        }

        public override void Enter()
        {
            if (ModelSampler.EditModel != null)
            {
                originalRot = ModelSampler.EditModel.transform.localEulerAngles;
            }
        }
    }
}