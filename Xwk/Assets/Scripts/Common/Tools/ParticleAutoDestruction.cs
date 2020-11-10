using UnityEngine;
using System.Collections;
public class ParticleAutoDestruction : MonoBehaviour
{

    //特效执行次数
    private ParticleSystem[] particleSystems;

    void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();

        foreach (var p in particleSystems)
        {
            p.loop = false;
        }
    }

    void Update()
    {
        bool allStopped = true;

        foreach (ParticleSystem ps in particleSystems)
        {
            if (!ps.isStopped)
            {
                allStopped = false;
            }
        }

        if (allStopped)
            GameObject.Destroy(gameObject);
    }
}
