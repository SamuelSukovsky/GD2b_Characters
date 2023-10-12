using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    private CapsuleCollider2D collider;
    private Animator anim;
    private Vector2 groundCheckPoint;
    public GameObject feet;
    public AudioSource audio;
    public AudioClip jumpSound;
    public AudioClip walkSound;

    private float dir;
    private Vector2 colliderSize;
    public float speed = 6f;
    public bool crouch = false;
    public bool grounded = true;
    public LayerMask layer;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
        colliderSize = collider.size;
        anim = GetComponent<Animator>();
        PlayerInput input = GetComponent<PlayerInput>();
        input.actions["AD"].started += Run;
        input.actions["AD"].canceled += Run;
        input.actions["Space"].started += Jump;
        input.actions["WS"].started += Crouch;
        input.actions["WS"].canceled += Crouch;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (grounded)
        {
            anim.SetBool("walking", false);
            body.velocityY = speed;
        }
    }
    private void Crouch(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() < 0)
        {
            Vector3 flip = transform.localScale;
            flip.y = .5f;
            transform.localScale = flip; 
            crouch = true;
        }
        else
        {
            Vector3 flip = transform.localScale;
            flip.y = 1f;
            transform.localScale = flip; 
            crouch = false;
        }
    }

    private void Run(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<float>();
        if (dir != 0)
        {
            if (!crouch)
            {
                if (grounded)
                {
                    anim.SetBool("walking", true);
                }
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

    private void PlaySound(AudioSource clip)
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        groundCheckPoint = feet.transform.position;
        grounded = Physics2D.OverlapCircle(groundCheckPoint, .2f, layer);
        body.velocityX = dir * speed;
    }
}
