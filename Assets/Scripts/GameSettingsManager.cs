using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameSettingsManager : MonoBehaviour
{
    bool isHovering;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void FixedUpdate()
    {
        if(isHovering)
        {
            transform.rotation *= new Quaternion(0, 0, 1.1f, 0);
        }
    }

    void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        Debug.Log("keke");
    }

    void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}
