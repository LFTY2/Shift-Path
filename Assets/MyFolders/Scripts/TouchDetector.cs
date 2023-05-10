using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetector : MonoBehaviour
{
    void Update()
    {
        if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if(hit.collider.CompareTag("Ball"))
                    {
                        hit.collider.gameObject.GetComponent<Ball>().isTouching = true;
                    }

                    if (hit.collider.CompareTag("Connector"))
                    {
                        Connector connector = hit.collider.gameObject.GetComponent<Connector>();
                        connector.isTouching = true;
                        connector.fingerDownPosition = touch.position;
                    }
                }
            }
        }
    }
}
