using UnityEngine;
using System.Collections;

public class ScrollingUV : MonoBehaviour
{

    public float m_SpeedU = 0.1f;
    public float m_SpeedV = -0.1f;

    private Renderer cachedRenderer;

    void Start()
    {
        cachedRenderer = this.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float newOffsetU = Time.time * m_SpeedU;
        float newOffsetV = Time.time * m_SpeedV;

        if (cachedRenderer)
        {
            cachedRenderer.material.mainTextureOffset = new Vector2(newOffsetU, newOffsetV);
        }
    }
}