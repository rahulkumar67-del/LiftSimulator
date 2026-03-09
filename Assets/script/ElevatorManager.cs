using System.Collections.Generic;
using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    [SerializeField] private List<Elevator> allElevators = new List<Elevator>();

    // 1. A queue for when all lifts are busy
    private Queue<int> pendingRequests = new Queue<int>();
    [SerializeField] public Transform[] FloorCheckPoints;
    public static ElevatorManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void RequestElevator(int floorCall, Direction intendedDir)
    {
        Elevator bestLift = GetBestElevator(floorCall);
        Debug.Log("Best Lift" + bestLift);
        if (bestLift != null && !bestLift.isMoving)
        {
            bestLift.AddRequest(floorCall);
        }
        else
        {
            // 2. Logic for busy elevators: Add to a queue
            if (!pendingRequests.Contains(floorCall))
                pendingRequests.Enqueue(floorCall);
        }
    }



    public void LiftCall(int floor)
    {
        Elevator Lift = GetBestElevator(floor);

    }

    public void CallSpecificLift(int liftIndex, int floor, Direction dir)
    {
        if (liftIndex >= 0 && liftIndex < allElevators.Count)
        {
            allElevators[liftIndex].AddRequest(floor);
        }
    }
    private Elevator GetBestElevator(int targetFloor)
    {
        Elevator selectedLift = null;
        float lowestScore = float.MaxValue;

        if (allElevators.Count == 0) return null;

        foreach (Elevator lift in allElevators)
        {
            //float score = lift.GetScore(targetFloor);
            float score = Mathf.Abs(targetFloor- lift.currentFloor);
            if (score < lowestScore)
            {
                Debug.Log("Score for " + lift.name + ": " + score);
                lowestScore = score;
                selectedLift = lift;
            }
        }
        return selectedLift;
    }

  
    // Update is used here to check if we can process queued requests
    private void Update()
    {
        if (pendingRequests.Count > 0)
        {
            // Try to assign the next request in line
            int nextFloor = pendingRequests.Peek();
            Elevator availableLift = GetBestElevator(nextFloor);

            // If the best lift is idle, take it out of the queue and move
            if (availableLift != null && !availableLift.isMoving)
            {
                pendingRequests.Dequeue();
                availableLift.AddRequest(nextFloor);
            }
        }
    }
}