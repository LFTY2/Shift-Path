using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float speed = 5f;

    public GameManager.PathColor color;

    private Vector3 startPosition;
    private float swipeThreshold = 50f;

    private bool hasSwiped = false;

    public Path path;

    private new Transform transform;

    Renderer objectRenderer;
    private Material objectsMaterial;
    [SerializeField] private Material rainbowMaterial;

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
    }
    private void OnMouseDown()
    {
        startPosition = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        if (hasSwiped) return;
        if (Vector3.Distance(Input.mousePosition, startPosition) > swipeThreshold)
        {
            Vector3 direction = Input.mousePosition - startPosition;

            if (direction.x > 0)
            {
                StartCoroutine(StopSwiping());
                CalculateDirection(SwipeDirection.right);
                return;
            }
            else if (direction.x < 0)
            {
                StartCoroutine(StopSwiping());
                CalculateDirection(SwipeDirection.left);
                return;
            }          
        }
    }

    IEnumerator StopSwiping()
    {
        startPosition = Input.mousePosition;
        hasSwiped = true;
        yield return new WaitForSeconds(1f);
        hasSwiped = false;
    }

    void CalculateDirection(SwipeDirection swipeDirection)
    {
        float deltaX =  transform.position.x - path.transform.position.x ;

        SwipeDirection firstSwipeCondition;

        
        if (deltaX > 0)
            firstSwipeCondition = SwipeDirection.right;
        else
            firstSwipeCondition = SwipeDirection.left;

        if (swipeDirection == firstSwipeCondition)
        {
            path.Rotate(true);
        }
        else
        {
            path.Rotate(false);
        }
    }
    public void MoveToPos(Vector3 targetPosition)
    {
        StartCoroutine(Move(targetPosition));
    }
    public void MoveToPos(Transform targetTransform)
    {
        StartCoroutine(Move(targetTransform));
    }

    private IEnumerator Move(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.Normalize();

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position += direction * speed * Time.deltaTime;

            yield return null;
        }
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
}
