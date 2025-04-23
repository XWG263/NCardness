using Spine;
using Spine.Unity;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace CCx
{

    [ExecuteInEditMode]
    public class SkeletonPlayer : MonoBehaviour
    {
        private IEditorSkeletonWrapper _skeletonWrapper;
        private TrackEntry _trackEntry;
        private string _oldAnimationName;
        private bool _oldLoop;
        private double _oldTime;

        public float delTime = 0;

        [DidReloadScripts]
        private static void OnReloaded()
        {
            //force awake when scripts are reloaded
            var editorSpineAnimations = FindObjectsOfType<SkeletonPlayer>();

            foreach (var editorSpineAnimation in editorSpineAnimations)
            {
                editorSpineAnimation.Awake();
            }
        }

        private void Awake()
        {
            if (Application.isPlaying) return;

            if (_skeletonWrapper == null)
            {
                if (TryGetComponent<SkeletonAnimation>(out var skeletonAnimation))
                {
                    _skeletonWrapper = new SkeletonAnimationWrapper(skeletonAnimation);
                }
                else if (TryGetComponent<SkeletonGraphic>(out var skeletonGraphic))
                {
                    _skeletonWrapper = new SkeletonGraphicWrapper(skeletonGraphic);
                }
            }

            _oldTime = EditorApplication.timeSinceStartup;
            EditorApplication.update += EditorUpdate;
        }

        private void OnDestroy()
        {
            EditorApplication.update -= EditorUpdate;
        }

        private void EditorUpdate()
        {
            if (Application.isPlaying) return;
            if (_skeletonWrapper == null) return;
            if (_skeletonWrapper.State == null) return;

            if (_oldAnimationName != _skeletonWrapper.AnimationName || _oldLoop != _skeletonWrapper.Loop)
            {
                _trackEntry = _skeletonWrapper.State.SetAnimation(0, _skeletonWrapper.AnimationName, _skeletonWrapper.Loop);
                _oldAnimationName = _skeletonWrapper.AnimationName;
                _oldLoop = _skeletonWrapper.Loop;
            }

            if (_trackEntry != null)
            {
                _trackEntry.TimeScale = _skeletonWrapper.Speed;
            }

            float deltaTime = (float)(EditorApplication.timeSinceStartup - _oldTime);
            _skeletonWrapper.Update(deltaTime);
            _oldTime = EditorApplication.timeSinceStartup;

            //force repaint to update animation
            EditorApplication.QueuePlayerLoopUpdate();
        }

        private class SkeletonAnimationWrapper : IEditorSkeletonWrapper
        {
            private SkeletonAnimation _skeletonAnimation;

            public SkeletonAnimationWrapper(SkeletonAnimation skeletonAnimation)
            {
                _skeletonAnimation = skeletonAnimation;
                _skeletonAnimation.loop = false;
            }

            public string AnimationName
            {
                get { return _skeletonAnimation.AnimationName; }
            }

            public bool Loop
            {
                get { return _skeletonAnimation.loop; }
            }

            public float Speed
            {
                get { return _skeletonAnimation.timeScale; }
            }

            public Spine.AnimationState State
            {
                get { return _skeletonAnimation.state; }
            }

            public void Update(float deltaTime)
            {
                _skeletonAnimation.Update(deltaTime);

            }
        }

        private class SkeletonGraphicWrapper : IEditorSkeletonWrapper
        {
            private SkeletonGraphic _skeletonGraphic;

            public SkeletonGraphicWrapper(SkeletonGraphic skeletonGraphic)
            {
                _skeletonGraphic = skeletonGraphic;
            }

            public string AnimationName
            {
                get { return _skeletonGraphic.startingAnimation; }
            }

            public bool Loop
            {
                get { return _skeletonGraphic.startingLoop; }
            }

            public float Speed
            {
                get { return _skeletonGraphic.timeScale; }
            }

            public Spine.AnimationState State
            {
                get { return _skeletonGraphic.AnimationState; }
            }

            public void Update(float deltaTime)
            {
                _skeletonGraphic.Update(deltaTime);
            }
        }

        private interface IEditorSkeletonWrapper
        {
            string AnimationName { get; }

            bool Loop { get; }

            float Speed { get; }

            Spine.AnimationState State { get; }

            void Update(float deltaTime);
        }
    }
}