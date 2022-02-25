using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkPiece : XRGrabInteractable
{
    public (int, int) Position { get; set; }
    public string Color { get; set; }
    public bool isCrowned = false;

    public bool CheckValidPlacement()
    {
        List<int> validPositions;
        bool tR, tL, bR, bL;
        int PositiveX = Position.Item1 + 1;
        int NegativeX = Position.Item1 - 1;
        int PositiveZ = Position.Item2 + 1;
        int NegativeZ = Position.Item2 - 1;

        if(PositiveX > 7 || PositiveZ > 7)
        {
            tL = false;
            tR = false;
            bR = false;
        }
        if(NegativeX < 0 || NegativeZ < 0)
        {
            tL = false;
            bL = false;
            bR = false;
        }



        (int, int) valid1 = (Position.Item1 + 1, Position.Item2 + 1);
        (int, int) valid2 = (Position.Item1 - 1, Position.Item2 + 1);

        
        Debug.Log("This piece can move to " + valid1 + " and " + valid2
            +" is Position Empty "+ FindObjectOfType<NetworkBoard>().sections[valid1.Item1, valid1.Item2].isEmpty);
        return true;
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
