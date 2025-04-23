using System;
using UnityGameFramework.Runtime;

namespace CCx
{
    /// <summary>
    /// TOTO:数据的重新接入
    /// </summary>
    public static class EntityExtension
    {
        // 关于 EntityId 的约定：
        // 0 为无效
        // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
        // 负值用于本地生成的临时实体（如特效、FakeObject等）
        private static int s_SerialId = 0;

        public static Entity GetGameEntity(this EntityComponent entityComponent, int entityId)
        {
            UnityGameFramework.Runtime.Entity entity = entityComponent.GetEntity(entityId);
            if (entity == null)
            {
                return null;
            }

            return (Entity)entity.Logic;
        }

        public static void HideEntity(this EntityComponent entityComponent, Entity entity)
        {
            entityComponent.HideEntity(entity.Entity);
        }

        public static void AttachEntity(this EntityComponent entityComponent, Entity entity, int ownerId, string parentTransformPath = null, object userData = null)
        {
            entityComponent.AttachEntity(entity.Entity, ownerId, parentTransformPath, userData);
        }

        public static void ShowSkill(this EntityComponent entityComponent, SkillData data)
        {
            entityComponent.ShowEntity(typeof(SkillEntity), "Skill", 10, data, "SkillEntity");
        }

        public static void ShowHero(this EntityComponent entityComponent, HeroData data)
        {
            entityComponent.ShowEntity(typeof(HeroCard), "Hero", 10, data,"Hero");
        }
        public static void ShowEnemy(this EntityComponent entityComponent, HeroData data)
        {
            entityComponent.ShowEntity(typeof(EnemyCard), "Enemy", 10, data,"Enemy");
        }

        private static void ShowEntity(this EntityComponent entityComponent, Type logicType, string entityGroup, int priority, EntityData data,string assetName = "")
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            entityComponent.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(assetName), entityGroup, priority, data);
        }

        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return --s_SerialId;
        }
    }
}
