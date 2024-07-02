using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    Vector3 initPos = Vector3.zero;
    // Start is called before the first frame update
    float radiusAttack;
    void Start()
    {
        
    }
    void OnEnable()
    {
        initPos = transform.position;
        radiusAttack = CameraController.Instance.player.GetComponent<PlayerController>().RadiusAttack;
    }

    void OnDisable()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(initPos, this.transform.position) >= radiusAttack - 0.4f)
        {

            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.layer == (int)CONSTANT.Layer.Enemy)
        {
            Debug.Log(collision.gameObject.name);
            gameObject.SetActive(false);
            Destroy(collision.gameObject);
        }
    }
}
