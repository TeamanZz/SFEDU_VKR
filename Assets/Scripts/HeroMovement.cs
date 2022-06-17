using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HeroMovement : MonoBehaviour
{
    public HeroController controller;
    public float runSpeed = 40f;

    private float horizontalMove = 0f;
    private bool jump = false;

    private PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!view.IsMine)
            return;
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetKeyDown(KeyCode.W))
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        if (!view.IsMine)
            return;
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }
}