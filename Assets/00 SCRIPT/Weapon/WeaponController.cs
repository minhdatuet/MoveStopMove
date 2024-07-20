using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    Vector3 initPos = Vector3.zero;
    Vector3 sourceScale;
    float radiusAttack;
    bool hasRotatedTowardsAttacker = false;

    [SerializeField] GameObject attacker;
    [SerializeField] float rotationSpeed = 2000f;

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
        hasRotatedTowardsAttacker = false; 

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
                            if (!hasRotatedTowardsAttacker) 
                            {
                                RotateTowardsAttacker();
                                hasRotatedTowardsAttacker = true;
                            }
                            OnceAttack();
                            break;
                        }
                    case "Boomerang":
                    case "Z":
                        {
                            DoubleAttack();
                            if (gameObject.activeInHierarchy)
                            {
                                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
                            }
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

    void RotateTowardsAttacker()
    {
        if (attacker != null)
        {
            Vector3 directionToFace = attacker.transform.forward;
            Quaternion targetRotation = Quaternion.LookRotation(directionToFace);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void OnceAttack()
    {
        if (attacker && Vector3.Distance(initPos, this.transform.position) >= radiusAttack / 2)
        {
            if (attacker.tag.Equals("Player"))
            {
                Debug.Log(radiusAttack);
                attacker.GetComponent<PlayerController>().HittedTarget();
            }
            gameObject.SetActive(false);
        }
    }

    void DoubleAttack()
    {
        if (attacker && Vector3.Distance(initPos, this.transform.position) >= radiusAttack / 2)
        {
            StartCoroutine(DoubleAttackCoroutine());
        }
    }

    IEnumerator DoubleAttackCoroutine()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Vector3 targetPos = attacker.transform.position;
        Debug.Log("RETURNING");
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            targetPos = attacker.transform.position;
            Debug.Log(targetPos);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 4.0f);
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
        if (collision.gameObject.layer == (int)CONSTANT.Layer.Enemy && attacker.gameObject != collision.gameObject)
        {
            if (attacker != null)
            {
                attacker.gameObject.GetComponentInChildren<ParticleSystemController>().StartLevelUpParticle();
                attacker.GetComponent<PlayerController>().ScaleCharacter();
            }

            InGameUIManager.Instance.UpdateAliveEnemy();
            collision.gameObject.GetComponentInChildren<ParticleSystemController>().StartDeathParticle();
            collision.gameObject.GetComponent<PlayerController>().IsDead = true;

            if (collision.gameObject.tag.Equals("Player"))
            {
                if (GameManager.Instance.IsRevive)
                {
                    GameManager.Instance.ReviveGame();
                }
                else
                {
                    GameManager.Instance.EndGame();
                }
                Debug.Log(attacker.gameObject.GetComponent<PlayerController>().NameDisplay.GetName());
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
