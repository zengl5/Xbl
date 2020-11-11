using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_ScaleParticles : MonoBehaviour
    {
        public float ParticleScale = 1.0f;
        public bool AlsoScaleGameobject = true;

        private List<float> initialSizes = new List<float>();

        void Awake()
        {
            if (AlsoScaleGameobject)
                transform.localScale = new Vector3(ParticleScale, ParticleScale, ParticleScale);

            ScaleLegacySystems(ParticleScale);
            ScaleShurikenSystems(ParticleScale);
            ScaleTrailRenderers(ParticleScale);
        }

        private void ScaleShurikenSystems(float scaleFactor)
        {
            //get all shuriken systems we need to do scaling on
            ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem system in systems)
            {
                system.startSpeed *= scaleFactor;
                system.startSize *= scaleFactor;
                system.gravityModifier *= scaleFactor;
            }
        }

        private void ScaleLegacySystems(float scaleFactor)
        {
            //get all emitters we need to do scaling on
            //ParticleEmitter[] emitters = GetComponentsInChildren<ParticleEmitter>();
            ////get all animators we need to do scaling on
            //ParticleAnimator[] animators = GetComponentsInChildren<ParticleAnimator>();
            ////apply scaling to emitters
            //foreach (ParticleEmitter emitter in emitters)
            //{
            //    emitter.minSize *= scaleFactor;
            //    emitter.maxSize *= scaleFactor;
            //    emitter.worldVelocity *= scaleFactor;
            //    emitter.localVelocity *= scaleFactor;
            //    emitter.rndVelocity *= scaleFactor;
            //}

            ////apply scaling to animators
            //foreach (ParticleAnimator animator in animators)
            //{
            //    animator.force *= scaleFactor;
            //    animator.rndForce *= scaleFactor;
            //}
        }

        private void ScaleTrailRenderers(float scaleFactor)
        {
            //get all animators we need to do scaling on
            TrailRenderer[] trails = GetComponentsInChildren<TrailRenderer>();

            //apply scaling to animators
            foreach (TrailRenderer trail in trails)
            {
                trail.startWidth *= scaleFactor;
                trail.endWidth *= scaleFactor;
            }
        }
    }
}