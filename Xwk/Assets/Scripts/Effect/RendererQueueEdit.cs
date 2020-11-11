using UnityEngine;
using System.Collections;

public class RendererQueueEdit : MonoBehaviour
{
	public int RenderQueue=2200;
	// Use this for initialization
	void Start () {
        SetRenderer();
    }

    public void SetRenderer()
    {
        Renderer AllRenderers = GetComponent<Renderer>();
        if (AllRenderers == null)
        {
            return;
        }
        foreach (Material m in AllRenderers.materials)
            m.renderQueue = RenderQueue;
    }

}
