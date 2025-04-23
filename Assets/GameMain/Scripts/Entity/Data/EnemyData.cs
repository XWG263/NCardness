using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCx
{
    [SerializeField]
    public class EnemyData : TargetableObjectData
    {
        public EnemyData(int entityId, int typeId, CampType camp) : base(entityId, typeId, camp)
        {
        }
    }

}
