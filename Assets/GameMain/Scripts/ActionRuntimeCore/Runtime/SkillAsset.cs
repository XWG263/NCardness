using System;
using NBC.ActionEditor;

namespace CCx
{
    [Name("角色技能")]
    [Serializable]
    public class SkillAsset : Asset
    {
        public int count => groups.Count;
    }
}
