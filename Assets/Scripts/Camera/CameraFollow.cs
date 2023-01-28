using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Camera {

    public class CameraFollow : MonoBehaviour {

        [SerializeField] private float smoothSpeed = 1f;
        
        private UnityEngine.Camera mainCamera;

        private Transform _mainCameraTransform;
        private Transform _transform;
        
        private void Awake() {
            mainCamera = UnityEngine.Camera.main;
            _mainCameraTransform = mainCamera.transform;
            _transform = transform;
        }

        private void LateUpdate() {

            Vector3 cameraPos = _mainCameraTransform.position;
            Vector3 playerPos = _transform.position;
            
            Vector3 destination = new Vector3(playerPos.x, playerPos.y, cameraPos.z);

            //_mainCameraTransform.position = Vector3.Lerp(cameraPos, destination, smoothSpeed);

            _mainCameraTransform.position = destination;

        }
        
    }

}
