using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    Vector3 initPos = Vector3.zero;
    Vector3 sourceScale;
    // Start is called before the first frame update
    float radiusAttack;
    [SerializeField] GameObject attacker;
    [SerializeField] float rotationSpeed = 720f;
    public GameObject Attacker
    {  
        get { return attacker; }  
        set { attacker = value; }
    }
    private void Awake()
    {
        sourceScale = transform.localScale;
    }
    void Start()
    {
        
    }
    void OnEnable()
    {
        initPos = transform.position;

        if (attacker != null)
        {
            if (attacker.tag.Equals("Player"))
            {
                radiusAttack = attacker.GetComponent<PlayerController>().RadiusAttack;
            }
            else
            {
                radiusAttack = attacker.GetComponent<EnemyController>().RadiusAttack;
            }
        }
    }

    void OnDisable()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.localScale = sourceScale;
  
    }
    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }

        if (attacker && Vector3.Distance(initPos, this.transform.position) >= radiusAttack - 0.4f * attacker.GetComponent<PlayerController>().CurrScale)
        {
            if (attacker.tag.Equals("Player"))
            {
                attacker.GetComponent<PlayerController>().HittedTarget();
            }
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.layer == (int)CONSTANT.Layer.Enemy)
        {
            Debug.Log(collision.gameObject.name);
            if (attacker != null)
            {
                
                attacker.GetComponent<PlayerController>().ScaleCharacter();
            }
            
            
            if (!collision.gameObject.tag.Equals("Player"))
            {
                TextManager.Instance.UpdateAliveEnemy();
                collision.gameObject.GetComponent<PlayerController>().IsDead = true;
            }
        }
        if (attacker && attacker.tag.Equals("Player"))
        {
            attacker.GetComponent<PlayerController>().HittedTarget();
        }
        gameObject.SetActive(false);
    }
}
