using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XBL.Core.Tools
{
    public class ParticleTools : MonoBehaviour
    {
        
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
        /// <summary>
        /// 缩放粒子
        /// </summary>
        /// <param name="scale">绽放系数</param>
        public void ScaleParticleSystem(float scale)
        {
            var hasParticleObj = false;
            var particles = GetComponentsInChildren<ParticleSystem>(true);
            var max = particles.Length;
            for (int idx = 0; idx < max; idx++)
            {
                var particle = particles[idx];
                if(particle==null) continue;
                hasParticleObj = true;
                particle.startSize *= scale;
                particle.startSpeed *= scale;
                particle.startRotation *= scale;
                particle.transform.localScale *= scale;
            }
            if (hasParticleObj)
            {
                transform.localScale = new Vector3(scale, scale, 1);
            }
        }
    }

}
