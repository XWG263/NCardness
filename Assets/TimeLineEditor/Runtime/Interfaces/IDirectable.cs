using System.Collections.Generic;
using UnityEngine;

namespace NBC.ActionEditor
{
    public interface IDirectable : IData , IDirectableProcessor
    {
        IDirector Root { get; }
        IDirectable Parent { get; }
        IEnumerable<IDirectable> Children { get; }

        GameObject Actor { get; }
        string Name { get; }

        bool IsActive { get; set; }
        bool IsCollapsed { get; set; }
        bool IsLocked { get; set; }

        float StartTime { get; }
        float EndTime { get; }

        float BlendIn { get; set; }
        float BlendOut { get; set; }
        bool CanCrossBlend { get; }

        void Validate(IDirector root, IDirectable parent);
        bool Initialize();
        void OnBeforeSerialize();
        void OnAfterDeserialize();
    }

    public interface IDirectableProcessor
    {
        void Enter();
        void Exit();
        void ReverseEnter();
        void Reverse();
        void Update(float time, float previousTime);
    }


    public interface IClip : IDirectable
    {
        object AnimatedParametersTarget { get; }
    }
}