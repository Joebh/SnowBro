using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bg_scroller : MonoBehaviour {

    public float scrollSpeed = 1f;
    private Vector2 savedOffset;
    private Vector2 usedOffset;
    private Renderer myRenderer;

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<Renderer>();
        savedOffset = myRenderer.sharedMaterial.GetTextureOffset("_MainText");
        usedOffset = new Vector2(savedOffset.x, savedOffset.y);
    }

    void Update()
    {
        if (this.enabled)
        {
            float y = Mathf.Repeat(Time.deltaTime * scrollSpeed, 1);
            usedOffset.y = (usedOffset.y - y) % -1;
            myRenderer.sharedMaterial.SetTextureOffset("_MainTex", usedOffset);
        }
        
    }

    void OnDisable()
    {
       //  myRenderer.sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }
}
