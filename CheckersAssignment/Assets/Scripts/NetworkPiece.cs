using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkPiece : XRGrabInteractable
{
    public (int, int) Position { get; set; }
    protected (int, int) tempPosition;
    public string PieceColor { get; set; }
    public bool isCrowned = false;

    GameObject currentCollision;

    List<(int, int)> eatablePieces;

    bool isRedsTurn = true;

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
        if (PieceColor == "Red" && !isCrowned)
        {
            validPositions.Remove(bR);
            validPositions.Remove(bL);
        }
        if (PieceColor == "Black" && !isCrowned)
        {
            validPositions.Remove(tR);
            validPositions.Remove(tL);
        }

        // Remove positions if diagonal section has a same colored piece
        for (int i = validPositions.Count-1; i >= 0; i--)
        {
            (int, int) validPos = (validPositions[i].Item1, validPositions[i].Item2);
            //Debug.Log("Removing piece in " + validPos.Item1 + "," + validPos.Item2);
            try
            {
                if (!FindObjectOfType<NetworkBoard>().sections[validPos.Item1, validPos.Item2].isEmpty
                && PieceColor == FindObjectOfType<NetworkBoard>().sections[validPos.Item1, validPos.Item2].piece.PieceColor)
                {
                    validPositions.Remove(validPos);
                }
            }
            catch (System.Exception)
            {
                continue;
            }
            
        }

        // Add new positions if the piece can eat
        eatablePieces = new List<(int, int)>();
        validPositions = GetEatablePositions(validPositions);

        // Debug 
        foreach (var validPosition in validPositions)
        {
            //if (!FindObjectOfType<NetworkBoard>().sections[validPosition.Item1, validPosition.Item2].isEmpty)
            //{
            //    Debug.Log("Position "+ validPosition.Item1 +", "+ validPosition.Item2 + " Is empty");
            //}
            Debug.Log("The valid positions for the piece in " +Position+" and has the color: "+PieceColor+ ", are: " + validPosition);
        }

        // Display Teleport points
        FindObjectOfType<NetworkBoard>().DisplayValidMovement(validPositions);

        return true;
    }

    List<(int, int)> GetEatablePositions(List<(int, int)> validPositions)
    {
        // = validPositions;
        for (int i = validPositions.Count - 1; i >= 0; i--)
        {
            (int, int) validPos = (validPositions[i].Item1, validPositions[i].Item2);
            try
            {
                if (PieceColor != FindObjectOfType<NetworkBoard>().sections[validPos.Item1, validPos.Item2].piece.PieceColor)
                {
                    int nextX = validPos.Item1 - Position.Item1;
                    int nextZ = validPos.Item2 - Position.Item2;

                    nextX = (nextX > 0) ? validPos.Item1 + 1 : validPos.Item1 - 1;
                    nextZ = (nextZ > 0) ? validPos.Item2 + 1 : validPos.Item2 - 1;

                    if (nextX > 7 || nextX < 0 || nextZ > 7 || nextZ < 0
                        || !FindObjectOfType<NetworkBoard>().sections[nextX, nextZ].isEmpty)
                    {
                        validPositions.Remove(validPos);
                        continue;
                    }
                    else
                    {
                        validPositions.Remove(validPos);
                        eatablePieces.Add(validPos);
                        validPositions.Add((nextX, nextZ));
                    }
                }
            }
            catch (System.Exception)
            {
                continue;
            }
        }
        return validPositions;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "TeleportPoint" && XRGrabNetworkInteractable.isBeingGrabbed)
        {
            currentCollision = other.gameObject;
            Debug.Log("Piece on " + tempPosition + " is being moved to: " + Position);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "TeleportPoint" && XRGrabNetworkInteractable.isBeingGrabbed)
        {
            currentCollision = null;
            Debug.Log("Piece was moved back to: " + Position);
        }
    }

    public void MovePiece()
    {
        if(currentCollision == null)
        {
            gameObject.transform.position = FindObjectOfType<NetworkBoard>().sections[Position.Item1, Position.Item2].Location;
            gameObject.transform.rotation = Quaternion.identity;
        }
        else
        {
            (int, int) positionToMove = currentCollision.GetComponent<TeleportPoint>().Position;

            int nextX = positionToMove.Item1 - Position.Item1;
            int nextZ = positionToMove.Item2 - Position.Item2;

            if (nextX > 1) nextX = positionToMove.Item1 - 1;
            else if (nextX < -1) nextX = positionToMove.Item1 + 1;

            if (nextZ > 1) nextZ = positionToMove.Item2 - 1;
            if (nextZ < -1) nextZ = positionToMove.Item2 + 1;

            if(eatablePieces.Count > 0 && eatablePieces.Contains((nextX, nextZ)))
            {
                Destroy(FindObjectOfType<NetworkBoard>().sections[nextX, nextZ].piece.gameObject);
                FindObjectOfType<NetworkBoard>().sections[nextX, nextZ].isEmpty = true;
                FindObjectOfType<NetworkBoard>().sections[nextX, nextZ].piece = null;
            }

            //Debug.Log("Piece in array: " + FindObjectOfType<NetworkBoard>().pieces[Position.Item1, Position.Item2]);

            FindObjectOfType<NetworkBoard>().sections[Position.Item1, Position.Item2].isEmpty = true;
            FindObjectOfType<NetworkBoard>().sections[Position.Item1, Position.Item2].piece = null;
            FindObjectOfType<NetworkBoard>().sections[positionToMove.Item1, positionToMove.Item2].isEmpty = false;
            FindObjectOfType<NetworkBoard>().sections[positionToMove.Item1, positionToMove.Item2].piece = gameObject.GetComponent<NetworkPiece>();

            Position = positionToMove;

            gameObject.transform.position = FindObjectOfType<NetworkBoard>().sections[Position.Item1, Position.Item2].Location;
            gameObject.transform.rotation = Quaternion.identity;

            // Crown Pieces
            if (Position.Item2 == 7 && PieceColor == "Red")
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.magenta;
                isCrowned = true;
            }else if(Position.Item2 == 0 && PieceColor == "Black")
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
                isCrowned = true;
            }
        }
    }
}
