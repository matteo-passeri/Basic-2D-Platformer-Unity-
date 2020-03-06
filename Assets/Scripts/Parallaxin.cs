using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxin : MonoBehaviour
{
    public Transform[] backgrounds; // Array with all the backgrounds to be parallax
    private float[] parallaxScales; // The proportion of the camera's movement to move the background by
    public float smoothing = 1f; // How smooth the parallax is going to be (has to be over 0)

    private Transform cam; // reference to the main camera transform
    private Vector3 previousCamPos; // the position of the camera in the previous frame

    // before start() - great for references
    void Awake() {
        // setup camera reference
        cam = Camera.main.transform;
    }

    // Start is called before the first frame update
    void Start() {
        //  the previous frame had the current frame's camera position
        previousCamPos = cam.position;

        // new array with the length of the background array
        parallaxScales = new float[backgrounds.Length];
        // assigning corrisponding parallax scales
        for (int i = 0; i < backgrounds.Length; i++) {
            parallaxScales[i] = backgrounds[i].position.z*-1;
        }
    }

    // Update is called once per frame
    void Update() {
        // for each background
        for (int i = 0; i < backgrounds.Length; i++) {
            // the parallax is the opposite of the camera movement because the previous frame multiple by the scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            // set a target x position which is the current position plus the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // create a target position which is the background's current position with its target x position
            Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            // fade between current position and the target position using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        // set previous cam position to the camera's position at the end of the frame
        previousCamPos = cam.position;
    }
}
