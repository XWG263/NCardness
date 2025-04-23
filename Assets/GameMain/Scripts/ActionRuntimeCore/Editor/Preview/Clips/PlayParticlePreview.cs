using NBC.ActionEditor;

using UnityEditor;
using UnityEngine;

namespace CCx
{
    /// <summary>
    /// 普通粒子预览
    /// </summary>
    [NBC.ActionEditor.CustomPreview(typeof(PlayParticle))]
    public class PlayParticlePreview : ProcessorBase<PlayParticle>
    {
        private GameObject _effectObj;
        public ParticleSystem particles;
        private ParticleSystem.EmissionModule em;

        public override void Update(float time, float previousTime)
        {
            if (_effectObj == null)
            {
                return;
            }

            if (_effectObj != null && !_effectObj.activeSelf)
            {
                _effectObj.SetActive(true);
            }

            UpdateParticle(time);
        }

        private void UpdateParticle(float time)
        {
            if (!Application.isPlaying)
            {
                if (particles != null)
                {
                    em.enabled = time < clip.GetLength();
                    particles.Simulate(time);
                    particles.transform.position = clip.pos;
                }
            }
        }

        public override void Enter()
        {
            CreateEffect();

            Play(_effectObj);
        }

        public override void Exit()
        {
            if (_effectObj != null)
            {
                _effectObj.gameObject.SetActive(false);
            }
        }

        protected void Play(GameObject effectObj)
        {
            if (particles == null)
            {
                particles = effectObj.GetComponentInChildren<ParticleSystem>();
            }

            if (!particles.isPlaying && particles.useAutoRandomSeed)
            {
                particles.useAutoRandomSeed = false;
            }

            em = particles.emission;
            em.enabled = true;
            particles.Play();
        }


        private void CreateEffect()
        {
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(clip.resPath);
            if (obj != null)
            {
                _effectObj = Object.Instantiate(obj);
             
            }
        }
    }
}