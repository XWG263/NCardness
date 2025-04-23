using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NBC.ActionEditor;
using UnityEditor;

namespace CCx
{
    [CommandProcessor(typeof(PlayParticle))]
    public class PlayParticleProcessor : ClipProcessor<PlayParticle>
    {
        private GameObject _effectObj;
        public ParticleSystem particles;
        private ParticleSystem.EmissionModule em;
        private PlayParticle clip;
        private GameObject obj;
        public override void Update(float time, float previousTime)
        {
            base.Update(time, previousTime);
        }

        public override void Enter()
        {
            clip = TData;
            CreateEffect();
        }

        private void CreateEffect()
        {
            obj = AssetDatabase.LoadAssetAtPath<GameObject>(clip.resPath);
            if (obj != null)
            {
                _effectObj = Object.Instantiate(obj);
                if (AIUtility.currentRound == TurnRound.Enemy)//ÁÙÊ±ÐÞ¸Ä
                {
                    _effectObj.transform.position = new Vector3(-clip.pos.x, clip.pos.y, clip.pos.z);
                }
                else
                {
                    _effectObj.transform.position = clip.pos;
                }

            }
        }

        public override void Exit()
        {
            
        }
    }

}
