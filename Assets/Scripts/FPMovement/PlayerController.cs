using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private Transform camTransform;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;    
    private InputManager inputManager;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.instance;
    }

    void Update()
    {
           
        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = camTransform.forward * move.z + camTransform.right * move.x;
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);

        if(inputManager.PlayerFired())
        {
            
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        
        controller.Move(playerVelocity * Time.deltaTime);
        
    }

}
