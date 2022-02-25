using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabNetworkInteractable : XRGrabInteractable
{
    private PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnHoverEntered(XRBaseInteractor interactor)
    {
        Debug.Log("Player hovered");
        base.OnHoverEntered(interactor);
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        Debug.Log("Player Grabbed:" + " int: " + interactor + " GO: " + gameObject);
        gameObject.GetComponent<NetworkPiece>().CheckValidPlacement();
        //gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        photonView.RequestOwnership();
        base.OnSelectEntered(interactor);
    }

}
