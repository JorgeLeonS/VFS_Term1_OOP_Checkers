using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkPiece : XRGrabInteractable
{
    public (int, int) Position { get; set; }
    protected (int, int) tempPosition;
    public string Color { get; set; }
    public bool isCrowned = false;

    public void SetToPosition()
    {
        //gameObject.transform.position
    }

    public bool CheckValidPlacement()
    {
        (int, int) tR, tL, bR, bL;
        int PositiveX = Position.Item1 + 1;
        int NegativeX = Position.Item1 - 1;
        int PositiveZ = Position.Item2 + 1;
        int NegativeZ = Position.Item2 - 1;

        tR = (PositiveX, PositiveZ);
        tL = (NegativeX, PositiveZ);
        bR = (PositiveX, NegativeZ);
        bL = (NegativeX, NegativeZ);

        List<(int, int)> validPositions = new List<(int, int)> { tR, tL, bR, bL};

        // Remove positions if the values are not in the board
        if (PositiveX > 7)
        {
            validPositions.Remove(tR);
            validPositions.Remove(bR);
        }
        if (PositiveZ > 7)
        {
            validPositions.Remove(tR);
            validPositions.Remove(tL);
        }
        if (NegativeX < 0)
        {
            validPositions.Remove(tL);
            validPositions.Remove(bL);
        }
        if (NegativeZ < 0)
        {
            validPositions.Remove(bR);
            validPositions.Remove(bL);
        }

        // Remove positions depending on color and if they are crowned
        if (Color == "Red" && !isCrowned)
        {
            validPositions.Remove(bR);
            validPositions.Remove(bL);
        }
        if (Color == "Black" && !isCrowned)
        {
            validPositions.Remove(tR);
            validPositions.Remove(tL);
        }
        foreach (var validPosition in validPositions)
        {
            Debug.Log("The valid positions for the piece in " +Position+" and has the color: "+Color+ "are: " + validPosition);
        }

        // Display Teleport points
        FindObjectOfType<NetworkBoard>().DisplayValidMovement(validPositions);

        //(int, int) valid1 = (Position.Item1 + 1, Position.Item2 + 1);
        //(int, int) valid2 = (Position.Item1 - 1, Position.Item2 + 1);


        //Debug.Log("This piece can move to " + valid1 + " and " + valid2
        //    +" is Position Empty "+ FindObjectOfType<NetworkBoard>().sections[valid1.Item1, valid1.Item2].isEmpty);
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        tempPosition = Position;
        // IF collision is with another stuff it gets a null reference exception
        if (other.TryGetComponent(out TeleportPoint tP))
        {
            Position = tP.Position;
        }
        Debug.Log("Piece on " + tempPosition + " is being moved to: " + Position);
        //transform.position = other.GetComponent<TeleportPoint>().Location;
    }

    private void OnTriggerExit(Collider other)
    {
        Position = tempPosition;
        Debug.Log("Piece was moved back to: "+ Position);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
