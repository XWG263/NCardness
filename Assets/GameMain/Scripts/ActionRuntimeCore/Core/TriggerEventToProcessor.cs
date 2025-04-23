using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NBC.ActionEditor;
namespace CCx
{
    /// <summary>
    /// [OptionSort(0)] public const int None = 0;
    ///[MenuName("测试事件")] public const int Test = 1;
    ///[MenuName("触发击中")] public const int Hit = 2;
    ///[MenuName("必杀检测")] public const int Kill = 3;
    /// </summary>
    [CommandProcessor(typeof(TriggerEvent))]
    public class TriggerEventToProcessor : ClipProcessor<TriggerEvent>
    {
        public override void Enter()
        {
            Debug.Log($"命中事件{TData.eventName}");
            switch (TData.eventName)
            {
                case 1:
                    break;
                case 2:
                    GameEntry.Event.Fire(this, AddDemageEventArgs.Create((SkillEntity)owner));
                    break;
                case 3:
                    break;
            }
        }
    }

}
