using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public enum Direction { Up, Down, Idle }

public class Elevator : MonoBehaviour
{
    [Header("Floor References")]
    public List<Transform> floorWaypoints = new List<Transform>();
    [SerializeField] private TextMeshProUGUI Floor;
    [Header("Settings")]
    public float speed = 500f;
    public float waitTime = 2f;

    [Header("Current State")]
    public int currentFloor = 0;
    public Direction currentDirection = Direction.Idle;
    public List<int> destinationQueue = new List<int>();
    public bool isMoving = false;

    private void Awake()
    {
        
    }
    public void AddRequest(int floor)
    {
        if (!destinationQueue.Contains(floor))
        {
            destinationQueue.Add(floor);
            SortQueue();
            if (!isMoving) StartCoroutine(ProcessQueue());
        }
    }

    private void SortQueue()
    {
        if (currentDirection == Direction.Idle && destinationQueue.Count > 0)
            currentDirection = destinationQueue[0] > currentFloor ? Direction.Up : Direction.Down;

        if (currentDirection == Direction.Up)
            destinationQueue = destinationQueue.OrderBy(f => f).ToList();
        else
            destinationQueue = destinationQueue.OrderByDescending(f => f).ToList();
    }

    private IEnumerator ProcessQueue()
    {
        isMoving = true;
        while (destinationQueue.Count > 0)
        {
            int nextTarget = destinationQueue[0];
            float targetY = floorWaypoints[nextTarget].position.y;
            Vector3 targetPos = new Vector3(transform.position.x, targetY, transform.position.z);

            while (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPos;
            currentFloor = nextTarget;
            Floor.text = currentFloor.ToString();
            destinationQueue.RemoveAt(0);
            yield return new WaitForSeconds(waitTime);

            if (destinationQueue.Count == 0) currentDirection = Direction.Idle;
            else SortQueue();
        }
        isMoving = false;
    }
}