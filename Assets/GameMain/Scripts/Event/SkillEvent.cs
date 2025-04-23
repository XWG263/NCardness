using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCx
{
    public class SkillEvent
    {
        public EntityData owner;
        public SkillEntity skill;
        public SkillEvent(EntityData _EntityData, SkillEntity _SkillEntity)
        {
            skill = _SkillEntity;
            owner = _EntityData;
        }
    }

    public class ReleaseSkillEventArgs : GameEventArgs
    {
        public static int EventId => typeof(ReleaseSkillEventArgs).GetHashCode();
        public override int Id => EventId;
        public SkillEvent CustomData { get; set; }
        public ReleaseSkillEventArgs() => CustomData = null;

        public static ReleaseSkillEventArgs Create(object customData)
        {
            var args = ReferencePool.Acquire<ReleaseSkillEventArgs>();
            args.CustomData = (SkillEvent)customData;
            return args;
        }

        public override void Clear()
        {
            CustomData = null;
        }
    }

    public class AddDemageEventArgs : GameEventArgs
    {
        public static int EventId => typeof(AddDemageEventArgs).GetHashCode();
        public override int Id => EventId;
        public SkillEvent CustomData { get; set; }
        public AddDemageEventArgs() => CustomData = null;

        public static AddDemageEventArgs Create(object customData)
        {
            var args = ReferencePool.Acquire<AddDemageEventArgs>();
            args.CustomData = (SkillEvent)customData;
            return args;
        }

        public override void Clear()
        {
            CustomData = null;
        }
    }
}
