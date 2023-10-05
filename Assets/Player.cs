using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    private float dir;
    public float speed = 2f;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        PlayerInput input = GetComponent<PlayerInput>();
        input.actions["AD"].performed += Run;
    }

    private void Run(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<float>();
    }

    // Update is called once per frame
    void Update()
    {
        body.velocityX = dir * speed;
    }
}
