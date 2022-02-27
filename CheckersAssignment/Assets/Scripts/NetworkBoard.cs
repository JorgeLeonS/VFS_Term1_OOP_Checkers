using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkBoard : MonoBehaviourPunCallbacks
{
    public NetworkPiece[,] pieces = new NetworkPiece[8, 8];
    public BoardSection[,] sections = new BoardSection[8, 8];
    public GameObject redPiece;
    public GameObject blackPiece;

    float initialXPosition = 1.58f;
    float initialZPosition = 1.55f;

    public GameObject TeleportPoint;

    private float oldDistance = 9999;

    private void CheckNearestObject(GameObject teleportPoint)
    {
        float dist = Vector3.Distance(gameObject.transform.position, teleportPoint.transform.position);
        if (dist < oldDistance)
        {
            //closetsObject = teleportPoint;
            oldDistance = dist;
        }
    }

    public void DisplayValidMovement(List<(int, int)> validPositions)
    {
        foreach (var valid in validPositions)
        {
            Vector3 location = sections[valid.Item1, valid.Item2].Location;
            (int, int) position = sections[valid.Item1, valid.Item2].Position;
            GameObject newTeleportPoint = Instantiate(TeleportPoint, location, transform.rotation);
            TeleportPoint tP = newTeleportPoint.GetComponent<TeleportPoint>();
            tP.Position = position;
            tP.Location = location;
            tP.transform.position = tP.transform.position + new Vector3(0, -0.1f, 0);
            tP.transform.Rotate(-90, 0, 0);
        }
    }

    [PunRPC]
    public void CreatePiece(BoardSection section, GameObject piece)
    {
        GameObject newPiece = Instantiate(piece, section.Location, transform.rotation);
        NetworkPiece p = newPiece.GetComponent<NetworkPiece>();
        p.Position = section.Position;
        bool isRed = piece.name == "NetworkRedPiece";
        p.Color = isRed ? "Red" : "Black";
        section.isEmpty = false;
        pieces[section.Position.Item1, section.Position.Item2] = p;
    }
    [PunRPC]
    public void CreateBoardSections()
    {
        for (int z = 0; z < 8; z++)
        {
            for (int x = 0; x < 8; x++)
            {
                sections[x, z] = new BoardSection
                {
                    Position = (x, z),
                    Location = new Vector3(initialXPosition, 1.3f, initialZPosition),
                    isEmpty = true
                };
                initialXPosition -= 0.44f;
            }
            initialXPosition = 1.58f;
            initialZPosition -= 0.45f;
        }
    }

    [PunRPC]
    public void GenerateRedPieces()
    {
        for (int z = 0; z < 3; z++)
        {
            bool oddOrEven = (z % 2 == 0);
            if (oddOrEven)
            {
                for (int x = 0; x < 8; x += 2)
                {
                    CreatePiece(sections[x, z], redPiece);
                }
            }
            else
            {
                for (int x = 1; x < 8; x += 2)
                {
                    CreatePiece(sections[x, z], redPiece);
                }
            }
        }
    }
    [PunRPC]
    public void GenerateBlackPieces()
    {
        for (int z = 7; z > 4; z--)
        {
            bool oddOrEven = (z % 2 == 0);
            if (oddOrEven)
            {
                for (int x = 0; x < 8; x += 2)
                {
                    CreatePiece(sections[x, z], blackPiece);
                }
            }
            else
            {
                for (int x = 1; x < 8; x += 2)
                {
                    CreatePiece(sections[x, z], blackPiece);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //CreateBoardSections();
        //GenerateRedPieces();
        //GenerateBlackPieces();

        //int pieceCount = 0;

        //for (int i = 0; i < 8; i++)
        //{
        //    for (int j = 0; j < 8; j++)
        //    {
        //        if (pieces[i, j] != null)
        //        {
        //            if(pieces[i,j].name == "NetworkRedPiece(Clone)")
        //            {
        //                Debug.Log("Red piece on "+ i + " - " + j);
        //            }else if(pieces[i, j].name == "NetworkBlackPiece(Clone)")
        //            {
        //                Debug.Log("Black piece on " + i + " - " + j);
        //            }
        //        }
        //    }
        //}

        //foreach (var piece in pieces)
        //{
        //    if (piece != null)
        //    {
        //        pieceCount++;
        //        Debug.Log(piece.name + " position: " + piece.transform.position +
        //            " X: " + piece.xPosition + ", Z: " + piece.zPosition);
        //    }
        //}
        //Debug.Log(pieceCount);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateMouseOver()
    {

    }
}
