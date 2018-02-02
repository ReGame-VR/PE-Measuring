using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;
using UnityEngine.SceneManagement;

/// <summary>
/// Holds functions for responding to and recording preferences on menu.
/// </summary>
public class MenuController : MonoBehaviour {

    // boolean that keeps track whether a participant ID has been entered at least once. This
    // script disallows moving to next scene if it hasn't
    private bool enteredID = false;

    /// <summary>
    /// Records an alphanumeric participant ID. Hit enter to record. May be entered multiple times
    /// but only last submission is used.
    /// </summary>
    /// <param name="arg0"></param>
    public void RecordID(string arg0)
    {
        GlobalControl.Instance.participantID = arg0;
        enteredID = true;
    }


    /// <summary>
    /// Loads next scene if wii is connected and participant ID was entered.
    /// </summary>
    public void NextScene()
    {
        SceneManager.LoadScene("Measuring Scene");      
    }

	/// <summary>
    /// Disable VR for menu scene and hide warning text until needed.
    /// </summary>
	void Start () {
        // disable VR settings for menu scene
        UnityEngine.XR.XRSettings.enabled = false;
	}


    /// <summary>
    /// Re-enable VR when this script is disabled (since it is disabled on moving into next scene).
    /// </summary>
    void OnDisable()
    {
        UnityEngine.XR.XRSettings.enabled = true;
    }
}
