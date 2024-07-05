using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerState = CONSTANT.PlayerState;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] protected Rigidbody _rigi;
    [SerializeField] FloatingJoystick _joystick;
    [SerializeField] protected AnimationController _anim;
    [SerializeField] protected PlayerState _state = PlayerState.IDLE;
    [SerializeField] protected GameObject attackRange;
    [SerializeField] protected GameObject weaponPrefab;
    protected GameObject weapon;
    [SerializeField] protected GameObject weaponList;
    [SerializeField] protected GameObject weaponInHand;
    public GameObject WeaponInHand
    {
        get { return weaponInHand; }
    }
    [SerializeField] protected float speed = CONSTANT.PLAYER_SPEED;
    [SerializeField] protected float radiusAttack;
    public float RadiusAttack
    {
        get { return radiusAttack; }
        set { radiusAttack = value; }
    }
    Transform targetEnemy;
    [SerializeField] protected float attackForce = 2.0f;
    [SerializeField] protected float attackOffset = 0.7f; // Khoảng cách từ nhân vật tới vị trí xuất phát của đạn
    [SerializeField] protected float scaleRate = 1.1f;
    [SerializeField] protected float currScale = 1.0f;
    protected int level = 1;
    [SerializeField] protected GameObject levelDisplayPrefab;
    [SerializeField] protected GameObject levelDisplayList;
    protected LevelDisplay levelDisplay;
    protected Material bodyColor;

    protected bool canAttack = false;
    protected bool isDead = false;
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }
    protected LayerMask enemyLayer;

    void Start()
    {
        radiusAttack = attackRange.GetComponent<Renderer>().bounds.size.x * 0.5f;
        Debug.Log(radiusAttack);
        bodyColor = transform.GetChild(1).GetComponent<Renderer>().material;
        Debug.Log(bodyColor.color);
        enemyLayer = LayerMask.GetMask("Enemy");
        DisplayLevel();
    }

    void Update()
    {
        CheckDeath();
        if (!isDead)
        {
            Move();
            CheckForEnemiesAndAttack();
        }
        CheckState();
        _anim.UpdateAnimation(_state);
    }

    

    public virtual void Move()
    {
        Vector3 movement = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);

        if (movement != Vector3.zero)
        {
            Vector3 newPosition = transform.position + movement * speed * Time.deltaTime;

            //Kiểm tra để nhân vật không đi ra ngoài map
            newPosition.x = Mathf.Clamp(newPosition.x, -CONSTANT.MAP_SIZE / 2 + 0.5f, CONSTANT.MAP_SIZE / 2 - 0.5f);
            newPosition.z = Mathf.Clamp(newPosition.z, -CONSTANT.MAP_SIZE / 2 + 0.5f, CONSTANT.MAP_SIZE / 2 - 0.5f);

            transform.position = newPosition;

            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, speed * 100 * Time.deltaTime);
        } 
    }

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

    private IEnumerator WaitAndAttack()
    { 
        if (canAttack)
        {
            canAttack = false;
            if (targetEnemy && gameObject.tag.Equals("Player"))
            {
                Debug.Log("TARGETED");
                targetEnemy.GetChild(4).gameObject.SetActive(true);
            }
            
            yield return new WaitForSeconds(0.2f);
            if (_state != PlayerState.RUN && targetEnemy)
            {
                PoolingWeapon();
                weaponInHand.gameObject.SetActive(false);
                Attack();
            } 
            else
            {
                targetEnemy.GetChild(4).gameObject.SetActive(false);
            }
        }
        
        
    }

    public void CheckForEnemiesAndAttack()
    {
        if (canAttack && _state == PlayerState.IDLE && weaponInHand.activeInHierarchy)
        {

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radiusAttack, enemyLayer);
            if (hitColliders.Length > 1)
            {
                _state = PlayerState.ATTACK;
                _anim.UpdateAnimation(_state);
                // Tìm enemy gần nhất
                Transform closestEnemy = hitColliders[0].gameObject != gameObject ? hitColliders[0].transform : hitColliders[1].transform;
                float minDistance = Vector3.Distance(transform.position, closestEnemy.position);

                foreach (Collider hitCollider in hitColliders)
                {
                    if (hitCollider.gameObject != gameObject)
                    {
                        float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                        if (distance < minDistance)
                        {
                            closestEnemy = hitCollider.transform;
                            minDistance = distance;
                        }
                    }
                }
                targetEnemy = closestEnemy;
                
                if (!targetEnemy.gameObject.GetComponent<PlayerController>().IsDead)
                {
                    // Quay về phía enemy
                    Vector3 directionToEnemy = (closestEnemy.position - transform.position).normalized;
                    directionToEnemy.y = 0;
                    transform.rotation = Quaternion.LookRotation(directionToEnemy);

                    StartCoroutine(WaitAndAttack());
                }
                
            }
        }
    }

    public void Attack()
    {
        
        // Đẩy đạn theo hướng mà nhân vật đang quay mặt đến
        weapon.GetComponent<Rigidbody>().AddForce(transform.forward * attackForce, ForceMode.Impulse);
    }

    public void CheckDeath()
    {
        if (isDead)
        {
            //this.GetComponent<Collider>().enabled = false;
            StartCoroutine(WaitToDestroy());
        }
    }

    public void PoolingWeapon()
    {
        
        // Tạo vị trí xuất phát cho đạn phía trước nhân vật
        Vector3 attackPosition = transform.position + new Vector3(0, 0.3f * currScale, 0) + transform.forward * attackOffset * currScale;
        // Tạo đạn tại vị trí xuất phát
        weapon = ObjectPooling.Instance.GetObject(weaponPrefab, weaponList);
        weapon.transform.position = attackPosition;
        weapon.GetComponent<WeaponController>().Attacker = this.gameObject;
        weapon.transform.localScale *= currScale;
        weapon.SetActive(true);
        
    }

    public void ScaleCharacter()
    {
        level++;
        if (levelDisplay)
        {
            levelDisplay.SetLevel(level);
            levelDisplay.offset *= scaleRate;
        } 

        currScale *= scaleRate;
        this.transform.localScale *= scaleRate;
        radiusAttack *= scaleRate;
    }

    public void HittedTarget()
    {
        weaponInHand.gameObject.SetActive(true);
        if (targetEnemy && gameObject.tag.Equals("Player")) targetEnemy.GetChild(4).gameObject.SetActive(false);
    }
    public virtual void CheckState()
    {
        if (isDead)
        {
            _state = PlayerState.DEATH;
            return;
        }
        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            _state = PlayerState.RUN;
            canAttack = true;
        }
        else
        {
            _state = PlayerState.IDLE;
        }
    }

    public void DisplayLevel()
    {
        // Tạo đối tượng Level Display từ Prefab
        GameObject levelDisplayObject = Instantiate(levelDisplayPrefab, transform.position, Quaternion.identity, levelDisplayList.transform);
        levelDisplay = levelDisplayObject.GetComponent<LevelDisplay>();
        levelDisplay.target = this.transform;
        levelDisplay.offset = new Vector3(0, 1.3f, 0);
        levelDisplay.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = bodyColor.color;
    }
}
