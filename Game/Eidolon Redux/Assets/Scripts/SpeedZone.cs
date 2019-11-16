using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonEngine.Runtime
{
    public class SpeedZone : MonoBehaviour
    {
        private TPController controller;
        [SerializeField] float zoneSpeed = 1;
        [SerializeField] float lerpTime = 2;
        bool lerp = false;
        float currentTime = 0;
        float enteranceSpeed = 0;

        // Start is called before the first frame update
        void Start()
        {
            controller = GameObject.FindObjectOfType<TPController>();
        }

        // Update is called once per frame
        void Update()
        {
            //print(controller.zoneMovementSpeed);
            if ((currentTime < lerpTime) && (lerp))
            {
                currentTime += Time.deltaTime;
                controller.zoneMovementSpeed = Mathf.Lerp(enteranceSpeed, zoneSpeed, currentTime / lerpTime);
            }
            else
            {
                lerp = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            currentTime = 0;
            enteranceSpeed = controller.zoneMovementSpeed;
            if (controller.zoneMovementSpeed != zoneSpeed)
            {
                lerp = true;
            }
        }

    }

}

