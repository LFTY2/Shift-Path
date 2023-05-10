using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float speed = 5f;

    public GameManager.PathColor color;

    private bool isAllowedSwiping = true;
    public bool isTouching;

    public Path path;

    private new Transform transform;

    Renderer objectRenderer;
    private Material objectsMaterial;
    [SerializeField] private Material rainbowMaterial;


    private Vector2 fingerDownPosition;
    private Vector2 fingerMovePosition;

    [SerializeField]private float minDistanceForSwipe = 100f;

    [SerializeField] private float distanceToTeleport;
    [SerializeField] private float rotationCooldown;

    private enum SwipeDirection
    {
        up = 1,
        down = 2,
        left = 3,
        right = 4        
    }

    private void Awake()
    {
        transform = GetComponent<Transform>();
        objectRenderer = GetComponent<Renderer>();
        objectsMaterial = objectRenderer.material;
        distanceToTeleport = 3f;
    }

    void Update()
    {
        if (!isTouching) return;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                fingerDownPosition = touch.position;
                fingerMovePosition = touch.position;
            }

            if(touch.phase == TouchPhase.Moved)
            {
                fingerMovePosition = touch.position;
                float distance = Vector2.Distance(fingerDownPosition, fingerMovePosition);
                if (distance > minDistanceForSwipe)
                {
                    CheckSwipe();
                    fingerDownPosition = touch.position;
                    
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                isTouching = false;
            }
        }
    }

    void CheckSwipe()
    {
        if (!isAllowedSwiping) return;
        Vector2 direction = fingerMovePosition - fingerDownPosition;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                CalculateDirection(SwipeDirection.right);
            }
            else
            {
                CalculateDirection(SwipeDirection.left);
            }
        }
        else
        {
            if (direction.y > 0)
            {
                CalculateDirection(SwipeDirection.up);
            }
            else
            {
                CalculateDirection(SwipeDirection.down);
            }
        }
        isAllowedSwiping = false;
        StartCoroutine(RotationCooldown());
    }

    void CalculateDirection(SwipeDirection swipeDirection)
    {
        float deltaX = transform.position.x - path.transform.position.x;
        float deltaZ = transform.position.z - path.transform.position.z;

        SwipeDirection firstSwipeCondition = deltaX > 0 ? SwipeDirection.up : SwipeDirection.down;
        SwipeDirection secondSwipeCondition = deltaZ > 0 ? SwipeDirection.left : SwipeDirection.right;

        

        bool shouldChangeRotatation = swipeDirection == firstSwipeCondition || swipeDirection == secondSwipeCondition;
        path.Rotate(shouldChangeRotatation);
    }

    public void MoveToPos(Transform targetTransform)
    {
        if (Vector3.Distance(transform.position, targetTransform.position) > distanceToTeleport)
        {
            StartCoroutine(Teleport(targetTransform));
        }
            
        StartCoroutine(Move(targetTransform));
    }

    private IEnumerator Move(Transform targetTransform)
    {
        yield return new WaitForSeconds(0.1f);
        

        while (Vector3.Distance(transform.position, targetTransform.position) > 0.1f)
        {
            Vector3 direction = targetTransform.position - transform.position;
            direction.Normalize();
            transform.position += direction * speed * Time.deltaTime;

            yield return null;
        }
    }
    public void ChangePath(Path path)
    {
        this.path = path;
        if(path.isNullPath)
        {
            objectRenderer.material = rainbowMaterial;
        }
        else
        {
            objectRenderer.material = objectsMaterial;
        }
    }

    IEnumerator RotationCooldown()
    {
        isAllowedSwiping = false;
        yield return new WaitForSeconds(rotationCooldown);
        isAllowedSwiping = true;
    }

    IEnumerator Teleport(Transform targetTransform)
    {

        float fadeInTime = 0.25f; float waitTime = 0.1f; float fadeOutTime = 0.25f;

        Renderer renderer = GetComponent<Renderer>();
        Color startColor = renderer.material.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // fade in to full opacity
        float elapsedTime = 0f;

        // Fade in
        while (elapsedTime < fadeInTime)
        {
            renderer.material.color = Color.Lerp(startColor, endColor, elapsedTime / fadeInTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Wait
        transform.position = targetTransform.position;
        yield return new WaitForSeconds(waitTime);

        // Fade out
        startColor = renderer.material.color;
        endColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // fade out to transparent
        elapsedTime = 0f;
        while (elapsedTime < fadeOutTime)
        {
            renderer.material.color = Color.Lerp(startColor, endColor, elapsedTime / fadeOutTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
