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
        if (Vector3.Distance(initPos, this.transform.position) >= radiusAttack - 0.4f)
        {
            if (attacker.tag.Equals("Player"))
            {
                attacker.GetComponent<PlayerController>().HittedTarget();
            }
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.layer == (int)CONSTANT.Layer.Enemy)
        {
            Debug.Log(collision.gameObject.name);
            if (attacker != null)
            {
                if (attacker.tag.Equals("Player"))
                {
                    attacker.GetComponent<PlayerController>().HittedTarget();
                }
                attacker.GetComponent<PlayerController>().ScaleCharacter();
            }
            
            gameObject.SetActive(false);
            if (!collision.gameObject.tag.Equals("Player"))
            {
                TextManager.Instance.UpdateAliveEnemy();
                collision.gameObject.GetComponent<PlayerController>().IsDead = true;
            }
        }
    }
}
