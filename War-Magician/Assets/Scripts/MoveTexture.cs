using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTexture : MonoBehaviour {

    public float speed;
    public Renderer _renderer;

    float offset = 0.0f;

	// Use this for initialization
	void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {

        offset += Time.deltaTime;

        _renderer.materials[0].SetTextureOffset("_MainTex", new Vector2(offset,0.0f));
        _renderer.materials[0].SetTextureOffset("_MKGlowTex", new Vector2(offset,0.0f));

        if (offset >= 1.0f)
        {
            offset = 0.0f;
        }
    }
}
