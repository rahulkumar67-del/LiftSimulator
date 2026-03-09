using UnityEngine;

public class LiftButtonHelper : MonoBehaviour
{
    public int liftIndex; // 0 for Lift A, 1 for Lift B, 2 for Lift C
    public int floorIndex;
    public Direction intendedDirection;

    // Use THIS function in your Button's OnClick()
    public void TriggerCall()
    {
        ElevatorManager.Instance.CallSpecificLift(liftIndex, floorIndex, intendedDirection);
    }
}