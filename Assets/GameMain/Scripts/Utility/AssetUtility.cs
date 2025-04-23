using GameFramework;
namespace CCx
{
    public enum SpineActionEnum
    {
        ATK,
        HIT,
        Skill,
        LANDING
    }

    public enum VFXPosEnum
    {
        Enemy,
        CURRENT
    }

    public enum TurnRound
    {
        Enemy,
        CURRENT
    }

    public class AssetUtility
    {
        public static string GetEntityAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Entities/{0}.prefab", assetName);
        }
    }
}

