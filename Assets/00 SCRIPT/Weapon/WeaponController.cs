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
    bool isReturning = false;
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
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                GameObject weaponEnabling = gameObject.transform.GetChild(i).gameObject;
                switch (weaponEnabling.name)
                {
                    case "Knife":
                    case "Ice-cream Cone":
                    case "Arrow":
                        {
                            OnceAttack();
                            break;
                        }
                    default:
                        {
                            OnceAttack();
                            if (gameObject.activeInHierarchy)
                            {
                                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
                            }
                            break;
                        }
                }
            }
        }
        
    }

    void OnceAttack()
    {
        if (attacker && Vector3.Distance(initPos, this.transform.position) >= radiusAttack - 0.5f * attacker.GetComponent<PlayerController>().CurrScale)
        {
            if (attacker.tag.Equals("Player"))
            {
                attacker.GetComponent<PlayerController>().HittedTarget();
            }
            gameObject.SetActive(false);
        }
    }

    void DoubleAttack()
    {
        if (!isReturning && attacker && Vector3.Distance(initPos, this.transform.position) >= radiusAttack - 0.5f * attacker.GetComponent<PlayerController>().CurrScale)
        {
            StartCoroutine(DoubleAttackCoroutine());
        }
    }

    IEnumerator DoubleAttackCoroutine()
    {
        isReturning = true;
        Vector3 targetPos = attacker.transform.position;

        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        if (attacker.tag.Equals("Player"))
        {
            attacker.GetComponent<PlayerController>().HittedTarget();
        }
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.layer == (int)CONSTANT.Layer.Enemy)
        {
            if (attacker != null)
            {
                attacker.gameObject.GetComponentInChildren<ParticleSystemController>().StartLevelUpParticle();
                attacker.GetComponent<PlayerController>().ScaleCharacter();
            }
            
            
            //if (!collision.gameObject.tag.Equals("Player"))
            //{
                InGameUIManager.Instance.UpdateAliveEnemy();
                collision.gameObject.GetComponentInChildren<ParticleSystemController>().StartDeathParticle();
                collision.gameObject.GetComponent<PlayerController>().IsDead = true;
            //}
            if (collision.gameObject.tag.Equals("Player"))
            {
                GameManager.Instance.EndGame();
                EndGameUIManager.Instance.SetKillerName(attacker.gameObject.GetComponent<PlayerController>().NameDisplay.GetName());
                EndGameUIManager.Instance.SetRank(InGameUIManager.Instance.AliveEnemy);
            }
        }
        if (attacker && attacker.tag.Equals("Player"))
        {
            attacker.GetComponent<PlayerController>().HittedTarget();
        }
        gameObject.SetActive(false);
    }
}
