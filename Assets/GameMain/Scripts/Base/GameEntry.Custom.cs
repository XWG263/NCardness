

using UnityEngine;

namespace CCx
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        //public static BuiltinDataComponent BuiltinData
        //{
        //    get;
        //    private set;
        //}

        public static SkillSlotComponent SkillSlot
        {
            get;
            private set;
        }

        private static void InitCustomComponents()
        {
            // BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
            SkillSlot = UnityGameFramework.Runtime.GameEntry.GetComponent<SkillSlotComponent>();
        }
    }
}
