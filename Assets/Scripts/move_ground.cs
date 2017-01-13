using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class move_ground : MonoBehaviour {

    public GameObject firstPiece;
    public GameObject secondPiece;
    
    // Use this for initialization
    void Start () {
        Vector3 newVector = new Vector3(0, 50f, 0);
        Vector3 offset = firstPiece.transform.TransformVector(newVector);

        secondPiece.transform.position = new Vector3(0, 
            firstPiece.transform.position.y - offset.y, 
            firstPiece.transform.position.z - offset.z);
        
    }
    
	
	// Update is called once per frame
	void Update () {
        if (firstPiece.transform.position.y > 50f)
        {
            Vector3 newVector = new Vector3(0, 50f, 0);
            Vector3 offset = firstPiece.transform.TransformVector(newVector);

            firstPiece.transform.position = new Vector3(0,
                secondPiece.transform.position.y - offset.y,
                secondPiece.transform.position.z - offset.z);

            Vector3 piece = firstPiece.GetComponent<AutoMoveAndRotate>().moveUnitsPerSecond.value;
            piece = new Vector3(0, piece.y + 1, 0);
        }
        if (secondPiece.transform.position.y > 50f)
        {
            Vector3 newVector = new Vector3(0, 50f, 0);
            Vector3 offset = firstPiece.transform.TransformVector(newVector);

            secondPiece.transform.position = new Vector3(0,
                firstPiece.transform.position.y - offset.y,
                firstPiece.transform.position.z - offset.z);

            if (firstPiece.transform.parent.transform.localEulerAngles.x < 20)
            {
                firstPiece.transform.parent.transform.localEulerAngles = new Vector3(
                    firstPiece.transform.parent.transform.localEulerAngles.x - 3f, 0, 0);
            }

            Vector3 piece = firstPiece.GetComponent<AutoMoveAndRotate>().moveUnitsPerSecond.value;

            piece = new Vector3(0, piece.y + 1, 0);
        }
    }
}
