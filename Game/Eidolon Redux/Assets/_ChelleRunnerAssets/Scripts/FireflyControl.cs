using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyControl : MonoBehaviour
{
    Transform parentTrans;

    Transform mainCam;

    public float moveSpeed = 1f;
    public float movementRadius = 0.5f;

    Vector3 currentTarget;

    public GameObject vanishParticles;

    // Start is called before the first frame update
    void Start()
    {
        parentTrans = transform.parent;
        mainCam = Camera.main.transform;

        currentTarget = (Random.insideUnitSphere * movementRadius) /*+ parentTrans.position*/;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.localPosition.x - currentTarget.x) < 0.05f &&
            Mathf.Abs(transform.localPosition.y - currentTarget.y) < 0.05f &&
            Mathf.Abs(transform.localPosition.z - currentTarget.z) < 0.05f)
        {
            currentTarget = (Random.insideUnitSphere * movementRadius) /*+ parentTrans.position*/;
        }

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, currentTarget, moveSpeed * Time.deltaTime);

        transform.LookAt(mainCam);
    }

    public void Disappear()
    {
        Instantiate(vanishParticles, transform.position, Quaternion.identity);
        Destroy(parentTrans.gameObject);
    }
}
