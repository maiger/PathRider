﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private Rigidbody2D rb;

    private void Update()
    {
        if(Input.GetButtonDown("Play"))
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}