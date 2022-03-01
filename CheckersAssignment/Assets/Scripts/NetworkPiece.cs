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

    GameObject currentCollision;

    public void SetToPosition()
    {
        //gameObject.transform.position
    }

    public bool CheckValidPlacement()
    {
        int PositiveX = Position.Item1 + 1;
        int NegativeX = Position.Item1 - 1;
        int PositiveZ = Position.Item2 + 1;
        int NegativeZ = Position.Item2 - 1;

        (int, int) tR, tL, bR, bL;
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

        // Remove positions if diagonal section has a same colored piece
        for (int i = validPositions.Count-1; i >= 0; i--)
        {
            (int, int) validPos = (validPositions[i].Item1, validPositions[i].Item2);
            Debug.Log("Removing piece in " + validPos.Item1 + "," + validPos.Item2);
            if (!FindObjectOfType<NetworkBoard>().sections[validPos.Item1, validPos.Item2].isEmpty 
                && Color == FindObjectOfType<NetworkBoard>().pieces[validPos.Item1, validPos.Item2].Color)
            {
                validPositions.Remove(validPos);
            }
        }

        // Debug 
        foreach (var validPosition in validPositions)
        {
            //if (!FindObjectOfType<NetworkBoard>().sections[validPosition.Item1, validPosition.Item2].isEmpty)
            //{
            //    Debug.Log("Position "+ validPosition.Item1 +", "+ validPosition.Item2 + " Is empty");
            //}
            Debug.Log("The valid positions for the piece in " +Position+" and has the color: "+Color+ ", are: " + validPosition);
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
        //tempPosition = Position;
        if(other.gameObject.tag == "TeleportPoint" && XRGrabNetworkInteractable.isBeingGrabbed)
        {
            currentCollision = other.gameObject;
            //Position = other.GetComponent<TeleportPoint>().Position;
            Debug.Log("Piece on " + tempPosition + " is being moved to: " + Position);
        }
        //transform.position = other.GetComponent<TeleportPoint>().Location;
    }

    public void MovePiece()
    {

        Debug.Log("Piece on " + Position + " is being moved");

        (int, int) positionToMove = currentCollision.GetComponent<TeleportPoint>().Position;
        //Debug.Log("Piece CurrentPos in array: "+FindObjectOfType<NetworkBoard>().pieces[Position.Item1, Position.Item2].Position);

        FindObjectOfType<NetworkBoard>().sections[Position.Item1, Position.Item2].isEmpty = true;
        FindObjectOfType<NetworkBoard>().sections[positionToMove.Item1, positionToMove.Item2].isEmpty = false;

        Position = positionToMove;

        gameObject.transform.position = FindObjectOfType<NetworkBoard>().sections[Position.Item1, Position.Item2].Location;
        gameObject.transform.rotation = Quaternion.identity;

        //Debug.Log("Piece NewPos in array: " + FindObjectOfType<NetworkBoard>().pieces[Position.Item1, Position.Item2].Position);

        //FindObjectOfType<NetworkBoard>().sections[positionToMove.Item1, positionToMove.Item2]
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "TeleportPoint" && XRGrabNetworkInteractable.isBeingGrabbed)
        {
            currentCollision = null;
            Position = tempPosition;
            Debug.Log("Piece was moved back to: " + Position);
        }
    }
}
