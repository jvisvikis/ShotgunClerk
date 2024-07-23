using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance {get; private set;}
    private Controls playerControls;
    
    void Awake()
    {        
        if(instance != null && instance != this)
        {            
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        playerControls = new Controls();
        
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Move.ReadValue<Vector2>();
    } 

    public Vector2 GetMouseDelta()
    {
        return playerControls.Player.Look.ReadValue<Vector2>();
    } 

    public bool PlayerJumped()
    {
        return playerControls.Player.Jump.triggered;
    }

    public bool PlayerFired()
    {
        return playerControls.Player.Fire.triggered;
    }

    public bool PlayerPaused()
    {
        return playerControls.Player.Pause.triggered;
    }
}
