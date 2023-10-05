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
    private Vector2 groundCheckPoint;
    public GameObject feet;
    private float dir;
    public float speed = 6f;
    public bool grounded = true;
    public LayerMask layer;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        PlayerInput input = GetComponent<PlayerInput>();
        input.actions["AD"].started += Run;
        input.actions["AD"].canceled += Run;
        input.actions["Space"].started += Jump;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (grounded)
        {
            anim.SetBool("walking", false);
            body.velocityY = speed;
        }
    }

    private void Run(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<float>();
        if (dir != 0)
        {
            if (grounded)
            {
                anim.SetBool("walking", true);
            }
            Vector3 flip = transform.localScale;
            flip.x = dir;
            transform.localScale = flip;
        }
        else
        {
            anim.SetBool("walking", false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        groundCheckPoint = feet.transform.position;
        grounded = Physics2D.OverlapCircle(groundCheckPoint, .2f, layer);
        body.velocityX = dir * speed;
    }
}
