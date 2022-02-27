using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    public GameObject placeHolderPiece;
    public (int, int) Position { get; set; }
    public Vector3 Location { get; set; }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (XRGrabNetworkInteractable.isBeingGrabbed)
    //    {
    //        Debug.Log("Keep on position");
    //    }
    //    else
    //    {
    //        Debug.Log("Other shit");
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(placeHolderPiece, gameObject.transform.position, Quaternion.identity);
        //Debug.Log(other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject[] placeHolderPieces;
        placeHolderPieces = GameObject.FindGameObjectsWithTag("PlaceHolderPiece");
        foreach (var item in placeHolderPieces)
        {
            Destroy(item);
        }
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
