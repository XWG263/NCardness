using NBC.ActionEditor;


namespace CCx
{
    /// <summary>
    /// VisibleTo预览
    /// </summary>
    [CustomPreview(typeof(VisibleTo))]
    public class VisibleToPreview : ProcessorBase<VisibleTo>
    {
        public override void Update(float time, float previousTime)
        {
            if (ModelSampler.EditModel != null)
            {
                ModelSampler.EditModel.SetActive(clip.visible);
            }
        }
    }
}