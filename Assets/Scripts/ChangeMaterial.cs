using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [SerializeField] private Renderer mat;
    [SerializeField] private Color newColour;
    
    private Color origColour;

    void Start()
    {
        origColour = mat.material.color;
    }

    public void ChangeColor(bool useNew)
    {
        mat.material.color = useNew ? newColour : origColour;
    }
}
