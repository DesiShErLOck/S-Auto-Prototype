﻿/*
 * Authored by Matt Shubert
 * www.symbioticstudios.net
 * Using additional code found at https://unity3d.com/learn/tutorials/modules/beginner/platform-specific/pinch-zoom 
 * Accessed on August 31 2015 
*/
using UnityEngine;
using System.Collections;

public class PinchZoom : MonoBehaviour {
	

	public float perspectiveZoomSpeed = 0.25f;        // The rate of change of the field of view in perspective mode.
	public float orthoZoomSpeed = 0.25f;  			  // The rate of change of the orthographic size in orthographic mode.

	Vector2 initialMidpoint = new Vector2();
	Vector2 initialSceenMidPoint = new Vector2();


	void Update()
	{
		// If there are two touches on the device...
		if (Input.touchCount == 2)
		{
			// Store both touches.
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);
			
			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			
			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			/*==================================================================================================
			 * 
			 *==================================================================================================*/

			//Get the overall Scale of the camera.
			float cameraScale = ((GetComponent<Camera>().orthographicSize)/((float)Screen.height/2));

			//Get the Camera positions (this just prevents you from having to call "GetComptenent<Camera>().tra. . ." repeatedlly) 
			float cameraPositionX = GetComponent<Camera>().transform.position.x;
			float cameraPositionY = GetComponent<Camera>().transform.position.y;

			//Checking to see if either of the touches began
			if(touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began){


				//Get the inital midpoint when you touch your fingers.  This will be in "Screen" coordinates
				initialMidpoint = new Vector2(((touchOne.position.x + touchZero.position.x)/2)-Screen.width/2,
				                               ((touchOne.position.y + touchZero.position.y)/2)-Screen.height/2);

				//Converts the inital midpoint from "Screen" to "scene" coordinates
				initialSceenMidPoint = new Vector2((cameraPositionX+(initialMidpoint.x*cameraScale)),
				                                    (cameraPositionY+(initialMidpoint.y*cameraScale)));
			}


			//Get the current midpoint of the touches in "Screen" coordinates
			Vector2 midpoint = new Vector2(((touchOne.position.x + touchZero.position.x)/2)-Screen.width/2,
			                               ((touchOne.position.y + touchZero.position.y)/2)-Screen.height/2);

			//Moves the camera depending on how far off your current midpoint is from the initial midpoint
			//This means that if you don't change the midpoint while you zoom that point will stay in the same x,y location on the screen
			GetComponent<Camera>().transform.position = new Vector3(initialSceenMidPoint.x - (midpoint.x*cameraScale),
				                                                        initialSceenMidPoint.y - (midpoint.y*cameraScale),
				                                                        0);


			/*==================================================================================================
			 * 
			 *==================================================================================================*/
			
			// If the camera is orthographic...
			if (GetComponent<Camera>().orthographic)
			{
				// ... change the orthographic size based on the change in distance between the touches.
				GetComponent<Camera>().orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

				
				// Make sure the orthographic size never drops below zero.
				GetComponent<Camera>().orthographicSize = Mathf.Max(GetComponent<Camera>().orthographicSize, 0.1f);

			}
			else
			{
				// Otherwise change the field of view based on the change in distance between the touches.
				GetComponent<Camera>().fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
				
				// Clamp the field of view to make sure it's between 0 and 180.
				GetComponent<Camera>().fieldOfView = Mathf.Clamp(GetComponent<Camera>().fieldOfView, 0.1f, 179.9f);
			}

		}
	}
}


