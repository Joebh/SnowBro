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

    void CreateTrees(GameObject piece)
    {

    }

    void UpdatePositionAndSpeed(GameObject pieceToMove, GameObject otherPiece)
    {
        Vector3 offset = otherPiece.transform.TransformVector(new Vector3(0, 50f, 0));

        pieceToMove.transform.position = new Vector3(0,
            otherPiece.transform.position.y - offset.y,
            otherPiece.transform.position.z - offset.z);

        // change angle of parent
        if (otherPiece.transform.parent.transform.localEulerAngles.x < 20)
        {
            otherPiece.transform.parent.transform.localEulerAngles = new Vector3(
                otherPiece.transform.parent.transform.localEulerAngles.x - 1.5f, 0, 0);
        }

        // increase speed
        AutoMoveAndRotate pieceToMoveScript = pieceToMove.GetComponent<AutoMoveAndRotate>();
        pieceToMoveScript.moveUnitsPerSecond.value = new Vector3(0, pieceToMoveScript.moveUnitsPerSecond.value.y + 1, 0);

        AutoMoveAndRotate otherMoveScript = otherPiece.GetComponent<AutoMoveAndRotate>();
        otherMoveScript.moveUnitsPerSecond.value = new Vector3(0, otherMoveScript.moveUnitsPerSecond.value.y + 1, 0);

        CreateTrees(pieceToMove);
    }
    
	
	// Update is called once per frame
	void Update () {
        if (firstPiece.transform.position.y > 50f)
        {
            UpdatePositionAndSpeed(firstPiece, secondPiece);
        }
        if (secondPiece.transform.position.y > 50f)
        {
            UpdatePositionAndSpeed(secondPiece, firstPiece);
        }
    }
}
