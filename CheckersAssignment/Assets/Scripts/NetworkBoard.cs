using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkBoard : MonoBehaviourPunCallbacks
{
    public NetworkPiece[,] pieces = new NetworkPiece[8, 8];
    public GameObject redPiece;
    public GameObject blackPiece;

    float RedsZPosition = 0.75f;
    float RedsXPosition = 1.58f;

    float BlacksZPosition = -0.75f;
    float BlacksXPosition = -1.58f;

    public void GenerateRedPieces()
    {
        for (int z = 0; z < 3; z++)
        {
            bool oddOrEven = (z % 2 != 0);
            for (int x = 0; x < 8; x+=2)
            {
                AssignRedPiecesPosition(x, z);
                RedsXPosition -= 0.88f;
            }
            RedsXPosition = oddOrEven ? 1.58f : 1.13f;
            RedsZPosition += 0.4f;
        }
    }

    public void GenerateBlackPieces()
    {
        for (int z = 0; z < 3; z++)
        {
            bool oddOrEven = (z % 2 != 0);
            for (int x = 0; x < 8; x += 2)
            {
                AssignBlackPiecesPosition(x, z);
                BlacksXPosition -= 0.88f;
            }
            BlacksXPosition = oddOrEven ? 1.58f : 1.13f;
            BlacksZPosition += 0.4f;
        }
    }

    private void AssignRedPiecesPosition(int x, int z)
    {
        GameObject newPiece = PhotonNetwork.Instantiate("NetworkRedPiece", new Vector3(RedsXPosition, 1.3f, RedsZPosition), transform.rotation);
        //GameObject newPiece = Instantiate(redPiece, new Vector3(RedsXPosition, 1.3f, RedsZPosition), transform.rotation);
        //newPiece.transform.SetParent(transform);
        NetworkPiece p = newPiece.GetComponent<NetworkPiece>();
        pieces[x, z] = p;
    }

    private void AssignBlackPiecesPosition(int x, int z)
    {
        GameObject newPiece = PhotonNetwork.Instantiate("NetworkBlackPiece", new Vector3(RedsXPosition, 1.3f, RedsZPosition), transform.rotation);
        //GameObject newPiece = Instantiate(redPiece, new Vector3(RedsXPosition, 1.3f, RedsZPosition), transform.rotation);
        //newPiece.transform.SetParent(transform);
        NetworkPiece p = newPiece.GetComponent<NetworkPiece>();
        pieces[x, z] = p;
    }

    // Start is called before the first frame update
    void Start()
    {
        //GenerateBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
