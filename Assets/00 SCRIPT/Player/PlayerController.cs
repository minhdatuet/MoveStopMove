using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using PlayerState = CONSTANT.PlayerState;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] protected Rigidbody _rigi;
    [SerializeField] FixedJoystick _joystick;
    [SerializeField] protected AnimationController _anim;
    [SerializeField] protected PlayerState _state = PlayerState.IDLE;
    [SerializeField] protected GameObject attackRange;
    [SerializeField] public GameObject weaponPrefab;
    [SerializeField] public GameObject weaponList;
    [SerializeField] protected float speed = CONSTANT.PLAYER_SPEED;
    [SerializeField] protected float radiusAttack;
    public float RadiusAttack
    {
        get { return radiusAttack; }
        set { radiusAttack = value; }
    }
    [SerializeField] float attackForce = 10.0f;
    [SerializeField] float attackOffset = 2.0f; // Khoảng cách từ nhân vật tới vị trí xuất phát của đạn
    [SerializeField] float scaleRate = 1.0f;

    protected bool canAttack = false;
    protected LayerMask enemyLayer;

    void Start()
    {
        radiusAttack = attackRange.GetComponent<Renderer>().bounds.size.x * 0.5f;
        Debug.Log(radiusAttack);
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    void Update()
    {
        Move();
        CheckState();
        CheckForEnemiesAndAttack();
        _anim.UpdateAnimation(_state);
    }

    public virtual void Move()
    {
        Vector3 movement = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);

        if (movement != Vector3.zero)
        {
            transform.position += movement * speed * Time.deltaTime;

            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, speed * 100 * Time.deltaTime);
        }
    }

    private IEnumerator WaitAnimationAttack()
    {
        _anim.UpdateAnimation(_state);
        yield return new WaitForSeconds(0.9f);
        
    }

    public void CheckForEnemiesAndAttack()
    {
        if (canAttack && _state == PlayerState.IDLE)
        {

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radiusAttack, enemyLayer);
            if (hitColliders.Length > 1)
            {
                _state = PlayerState.ATTACK;

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

                // Quay về phía enemy
                Vector3 directionToEnemy = (closestEnemy.position - transform.position).normalized;
                directionToEnemy.y = 0; 
                transform.rotation = Quaternion.LookRotation(directionToEnemy);

                StartCoroutine(WaitAnimationAttack());
                Attack();
            }
        }
    }

    public void Attack()
    {
        // Tạo vị trí xuất phát cho đạn phía trước nhân vật
        Vector3 attackPosition = transform.position + new Vector3(0, 0.3f, 0) + transform.forward * attackOffset;
        // Tạo đạn tại vị trí xuất phát
        GameObject weapon = ObjectPooling.Instance.GetObject(weaponPrefab.gameObject, weaponList.gameObject);
        weapon.transform.position = attackPosition;
        weapon.SetActive(true);
        // Đẩy đạn theo hướng mà nhân vật đang quay mặt đến
        weapon.GetComponent<Rigidbody>().AddForce(transform.forward * attackForce, ForceMode.Impulse);

        // Bỏ qua va chạm giữa vũ khí và người ném
        

        canAttack = false;
    }

    public virtual void CheckState()
    {
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
}
