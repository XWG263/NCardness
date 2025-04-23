using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NBC.ActionEditor;
namespace CCx
{
    public abstract class ClipProcessor<T> : ClipProcessor where T : Clip
    {
        protected T TData => directable as T;
    }
    public class ClipProcessor : IDirectable
    {
        public Entity owner;
        public IDirectable directable { get;  set; }
        public IDirector Root { get; }
        public IDirectable Parent { get; }
        public IEnumerable<IDirectable> Children { get; }

        public GameObject Actor { get; }
        public string Name { get; }

        public bool IsActive { get; set; }
        public bool IsCollapsed { get; set; }
        public bool IsLocked { get; set; }

        public float StartTime { get; }
        public float EndTime { get; }

        public float BlendIn { get; set; }
        public float BlendOut { get; set; }
        public bool CanCrossBlend { get; }


        public virtual void Enter()
        {
            
        }

        public virtual void Exit()
        {
            
        }

        public bool Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void OnAfterDeserialize()
        {
           
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void Reverse()
        {
           
        }

        public void ReverseEnter()
        {
           
        }

        public virtual void Update(float time, float previousTime)
        {
           
        }

        public void Validate(IDirector root, IDirectable parent)
        {
            
        }
    }

}
