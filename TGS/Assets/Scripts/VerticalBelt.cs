﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalBelt : MonoBehaviour {

    public float scrollSpeed = 0.5F;
    public Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void FixedUpdate()
    {
        float offset = Time.time * scrollSpeed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }
}
