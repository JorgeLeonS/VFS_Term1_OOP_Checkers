using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabNetworkInteractable : XRGrabInteractable
{
    private PhotonView photonView;
    public static bool isBeingGrabbed = false;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingGrabbed)
        {
            //Debug.Log(transform.position);
        }
    }


    //protected override void OnSelectExited(SelectExitEventArgs args)
    //{
    //    Debug.Log("args.interactableObject.coll " + args.interactableObject.colliders.);
    //    //.GetComponent<NetworkPiece>().MovePiece();
    //    NetworkManager.DeleteAllObjectsWithTag("TeleportPoint");
    //    NetworkManager.DeleteAllObjectsWithTag("PlaceHolderPiece");

    //    isBeingGrabbed = false;

    //    base.OnSelectExited(args);
    //}
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        gameObject.GetComponent<NetworkPiece>().MovePiece();
        NetworkManager.DeleteAllObjectsWithTag("TeleportPoint");
        NetworkManager.DeleteAllObjectsWithTag("PlaceHolderPiece");

        isBeingGrabbed = false;
        base.OnSelectExited(interactor);
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        Debug.Log("Player Grabbed:" + " int: " + interactor + " GO: " + gameObject);
        gameObject.GetComponent<NetworkPiece>().CheckValidPlacement();
        //gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        photonView.RequestOwnership();
        isBeingGrabbed = true;
        base.OnSelectEntered(interactor);
    }

}
