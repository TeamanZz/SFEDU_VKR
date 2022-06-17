using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Photon.Pun;

public class HeroController : MonoBehaviour
{
    public Animator animator;
    public Transform targetTransform;
    public Vector3 targetPos;
    public bool m_FacingRight = true;

    [SerializeField] private float m_JumpForce = 400f;
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_CeilingCheck;

    const float k_GroundedRadius = .2f; 
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    private PhotonView view;
    private bool m_Grounded;           

    private Camera camera;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        camera = Camera.main;
    }

    private void Update()
    {
        if (!view.IsMine)
            return;

        Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        targetTransform.position = new Vector3(mousePos.x, mousePos.y, -1f);


        targetPos = mousePos;

        if (mousePos.x > transform.position.x && !m_FacingRight)
        {
            FlipRight();
        }

        if (mousePos.x < transform.position.x && m_FacingRight)
        {
            FlipLeft();
        }
    }

    private void FixedUpdate()
    {
        if (!view.IsMine)
            return;
        m_Grounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
            }
        }
    }

    public void Move(float move, bool jump)
    {
        if (m_Grounded || m_AirControl)
        {
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
            if (move != 0)
            {
                animator.SetBool("IsRunning", true);
            }
            else
            {
                animator.SetBool("IsRunning", false);
            }
            float coefficient = 1;
            if (!m_FacingRight)
                coefficient *= -1;
            animator.SetFloat("RunningSpeed", move * coefficient);

        }
        if (m_Grounded && jump)
        {
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    private void FlipLeft()
    {
        m_FacingRight = false;
        transform.DORotate(new Vector3(0, -90, 0), 0.5f);
    }

    private void FlipRight()
    {
        m_FacingRight = true;
        transform.DORotate(new Vector3(0, 90, 0), 0.5f);
    }
}