using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadWriteCSV;

public class Measure : MonoBehaviour {


    // the head mounted display - Camera (head) in [CameraRig] prefab
    public GameObject hmd;

    // Controller (left) and Controller(right) - give both, but only 1 will be active
    // TODO: Replace with Manus Gloves
    public GameObject rightHand;
    public GameObject leftHand;

    // Calibration values
    private float armLen, shoulderHeight, leftReachLocation, rightReachLocation, fullHeight;

    private Vector3 standingPosition;

    // Use this for initialization
    void Start () {
        standingPosition = hmd.transform.position;
        armLen = 0f;
        shoulderHeight = 0f;
        leftReachLocation = 0f;
        rightReachLocation = 0f;
        fullHeight = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        
        // - - - - FORWARD REACH CALIBRATION - - - -
        if (Input.GetMouseButtonUp(0))
        {
            CaptureArmLengthAndHeight();
        }

        // - - - - SIDE REACH CALIBRATION - - - -
        Vector3 posn3;

        // get controller position
        if (rightHand.activeInHierarchy)
        {
            posn3 = rightHand.transform.position;
        }
        else
        {
            posn3 = leftHand.transform.position;
        }

        // check to see if it is a new maximum
        if (posn3.x > rightReachLocation)
        {
            rightReachLocation = posn3.x;
        }
        else if (posn3.x < leftReachLocation)
        {
            leftReachLocation = posn3.x;
        }


    }

    // Capture the arm length and height for calibration. Called on mouse up.
    public void CaptureArmLengthAndHeight()
    {
        if (rightHand.activeInHierarchy)
        {
            // Add 10cm (0.1f) to this value to compensate for depth of hmd
            armLen = Mathf.Abs(rightHand.transform.position.z - hmd.transform.position.z) + 0.1f;
            shoulderHeight = rightHand.transform.position.y;
        }
        else
        {
            armLen = Mathf.Abs(leftHand.transform.position.z - hmd.transform.position.z) + 0.1f;
            shoulderHeight = leftHand.transform.position.y;
        }

        // standingposition is where the HMD is, currently over eyes
        standingPosition = hmd.transform.position;
        // Vertical distance from shoulder to eyes
        float shoulderToEyes = standingPosition.y - shoulderHeight;
        // Eyes to top of head is approximately 80% of vertical distance from shoulder to eyes
        float eyesToTopOfHead = shoulderToEyes * 0.8f;

        // Full height is the height of the eyes plus the distance from eyes to top of head
        fullHeight = standingPosition.y + eyesToTopOfHead;

    }

    public void OnDisable()
    {
        float rightReachDistance = Mathf.Abs(rightReachLocation - standingPosition.x);
        float leftReachDistance = Mathf.Abs(leftReachLocation - standingPosition.x);

        Debug.Log("Arm Length: " + (armLen * 100).ToString() + "cm");
        Debug.Log("Shoulder Height: " + (shoulderHeight * 100).ToString() + "cm");
        Debug.Log("Left Reach Distance: " + (leftReachDistance * 100).ToString() + "cm");
        Debug.Log("Right Reach Distance: " + (rightReachDistance * 100).ToString() + "cm");
        Debug.Log("Full Height: " + (fullHeight * 100).ToString() + "cm");


        // Write all entries in data list to file
        using (CsvFileWriter writer = new CsvFileWriter(@"Data/ParticipantID" + GlobalControl.Instance.participantID + ".csv"))
        {
            // write header
            CsvRow header = new CsvRow();
            header.Add("Arm Length (cm)");
            header.Add("Shoulder Height (cm)");
            header.Add("Left Reach Distance (cm)");
            header.Add("Right Reach Distance (cm)");
            header.Add("Full Height (cm)");
            writer.WriteRow(header);

            // write row
            CsvRow row = new CsvRow();
            row.Add((armLen * 100).ToString());
            row.Add((shoulderHeight * 100).ToString());
            row.Add((leftReachDistance * 100).ToString());
            row.Add((rightReachDistance * 100).ToString());
            row.Add((fullHeight * 100).ToString());
            writer.WriteRow(row);
        }
    }
}
