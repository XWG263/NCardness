
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace CCx
{
    public abstract class GameBase
    {
        public abstract GameMode GameMode
        {
            get;
        }

        //protected ScrollableBackground SceneBackground
        //{
        //    get;
        //    private set;
        //}

        public bool GameOver
        {
            get;
            protected set;
        }

        //private MyAircraft m_MyAircraft = null;
        private HeroCard heroCard;
        public virtual void Initialize()
        {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);

            //SceneBackground = Object.FindObjectOfType<ScrollableBackground>();
            //if (SceneBackground == null)
            //{
            //    Log.Warning("Can not find scene background.");
            //    return;
            //}

            //SceneBackground.VisibleBoundary.gameObject.GetOrAddComponent<HideByBoundary>();
            //GameEntry.Entity.ShowMyAircraft(new MyAircraftData(GameEntry.Entity.GenerateSerialId(), 10000)
            //{
            //    Name = "My Aircraft",
            //    Position = Vector3.zero,
            //});
            //GameOver = false;
            //m_MyAircraft = null;
        }

        public virtual void Shutdown()
        {
           // GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
           // GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
        }

        public virtual void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (heroCard != null && heroCard.IsDead)
            {
                GameOver = true;
            }
        }

        protected virtual void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            //ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            //if (ne.EntityLogicType == typeof(MyAircraft))
            //{
            //    m_MyAircraft = (MyAircraft)ne.Entity.Logic;
            //}
        }

        protected virtual void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
            Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
        }
    }
}
