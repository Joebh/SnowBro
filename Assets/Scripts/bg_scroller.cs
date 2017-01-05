using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bg_scroller : MonoBehaviour {

    public float scrollSpeed = 1f;
    private Vector2 savedOffset;
    private Renderer myRenderer;

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<Renderer>();
        savedOffset = myRenderer.sharedMaterial.GetTextureOffset("_MainText");
    }

    void Update()
    {
        float y = Mathf.Repeat(Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2(savedOffset.x, -1 * y);
        myRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }

    void OnDisable()
    {
        myRenderer.sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }
}
