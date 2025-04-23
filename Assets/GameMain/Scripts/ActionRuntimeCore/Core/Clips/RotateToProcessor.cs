using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NBC.ActionEditor;
namespace CCx
{
    [CommandProcessor(typeof(RotateTo))]
    public class RotateToProcessor :ClipProcessor<RotateTo>
    {
        public override void Update(float time, float previousTime)
        {
            base.Update(time, previousTime);
        }
        public override void Enter()
        {
            base.Enter();
            var tt = (RotateTo)TData;
        }
    }

}
