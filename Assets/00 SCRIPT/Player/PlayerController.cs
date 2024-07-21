﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;
using static WeaponList;
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
    [SerializeField] protected GameObject weaponColorContainer;
    public GameObject WeaponInHand
    {
        get { return weaponInHand; }
    }
    [SerializeField] protected float speed = CONSTANT.PLAYER_SPEED;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    [SerializeField] protected float radiusAttack;
    public float RadiusAttack
    {
        get { return radiusAttack; }
        set { radiusAttack = value; }
    }
    Transform targetEnemy;
    [SerializeField] protected float attackForce = 4.0f; // Lực bay (tốc độ bay) của đạn
    [SerializeField] protected float attackOffset = 0.7f; // Khoảng cách từ nhân vật tới vị trí xuất phát của đạn
    [SerializeField] protected float scaleRate = 1.1f; // Tỉ lệ phóng đại khi bắn trúng enemy
    [SerializeField] protected float currScale = 1.0f; // Tỉ lệ phóng đại hiện tại
    public float CurrScale
    {
        get { return currScale; }
        set { currScale = value; }
    }
    protected int level = 1;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }
    [SerializeField] protected GameObject levelDisplayPrefab;
    [SerializeField] protected GameObject levelDisplayList;
    protected LevelDisplay levelDisplay;
    [SerializeField] protected GameObject nameDisplayPrefab;
    [SerializeField] protected GameObject nameDisplayList;
    [SerializeField] GameObject hairContainer;
    [SerializeField] List<Material> pantList = new List<Material>();
    [SerializeField] GameObject shieldContainer;
    [SerializeField] List<Image> customColorList = new List<Image>();
    protected NameDisplay nameDisplay;
    public NameDisplay NameDisplay
    {
        get { return nameDisplay; }
        set { nameDisplay = value; }
    }
    protected Material bodyColor;

    protected bool canAttack = false;
    protected bool isDead = false;
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }
    bool isChangingSkin = false;
    public bool IsChangingSkin
    {
        get { return isChangingSkin; }
        set { isChangingSkin = value; }
    }
    protected LayerMask enemyLayer;

    void Start()
    {
        radiusAttack = attackRange.GetComponent<Renderer>().bounds.size.x * 0.5f + 1.0f;
        bodyColor = transform.GetChild(1).GetComponent<Renderer>().material;
        enemyLayer = LayerMask.GetMask("Enemy");
        DisplayLevelAndName();
        StartCoroutine(SetBeginSkinCouroutine());
    }

    void Update()
    {
        CheckDeath();
        if (!isDead)
        {
            Move();
            FindTargetEnemy();
            CheckForEnemiesAndAttack();
        }
        CheckState();
        _anim.UpdateAnimation(_state);
    }

    private void OnEnable()
    {
        if (gameObject.CompareTag("Player")) StartCoroutine(SetBeginSkinCouroutine());
    }

    public void SetWeapon()
    {
        if (weapon)
        {
            for (int i = 0; i < weaponInHand.transform.childCount; i++)
            {
                if (weaponInHand.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    for (int j = 0; j < weapon.transform.childCount; j ++)
                    {
                        if (weapon.transform.GetChild(j).gameObject.name.Equals(weaponInHand.transform.GetChild(i).gameObject.name))
                        {
                            weapon.transform.GetChild(j).gameObject.SetActive(true);
                            weapon.transform.GetChild(j).GetComponent<Renderer>().materials = weaponInHand.transform.GetChild(i).GetComponent<Renderer>().materials;
                        }
                        else
                        {
                            weapon.transform.GetChild(j).gameObject.SetActive(false);
                        }
                    }
                    break;
                }
                
            }
        }
    }
    
    IEnumerator SetBeginSkinCouroutine()
    {
        yield return null;
        GameData data = SaveLoadManager.Instance.LoadData();
        for (int i = 0; i < data.player.hair.Count; i++)
        {
            if (data.player.hair[i].enable)
            {
                hairContainer.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                hairContainer.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        GameObject playerPant = gameObject.transform.GetChild(2).gameObject;
        for (int i = 0; i < data.player.pant.Count; i++)
        {
            if (data.player.pant[i].enable)
            {
                playerPant.SetActive(true);
                playerPant.GetComponent<Renderer>().material = pantList[i];
                break;
            }
            if (i == data.player.pant.Count - 1)
            {
                playerPant.SetActive(false);
            }
        }
        for (int i = 0; i < data.player.shield.Count; i++)
        {
            if (data.player.shield[i].enable)
            {

                shieldContainer.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                shieldContainer.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < weaponInHand.transform.childCount; i++)
        {
            if (data.player.weapon[i].enable)
            {
                weaponInHand.transform.GetChild(i).gameObject.SetActive(true);
                for (int j = 0; j < data.player.weapon[i].color.Count; j++)
                {
                    if (data.player.weapon[i].color[j].enable)
                    {
                        GameObject currColor = weaponColorContainer.transform.GetChild(j).transform.GetChild(0).gameObject;
                        weaponInHand.transform.GetChild(i).gameObject.GetComponent<Renderer>().materials = currColor.transform.GetChild(i).GetComponent<Renderer>().materials;
                    }
                }
                //weaponInHand.transform.GetChild(i).GetComponent<Renderer>().materials.Length
                if (data.player.weapon[i].color[4].enable)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        weaponInHand.transform.GetChild(i).GetComponent<Renderer>().materials[j].color = customColorList[data.player.weapon[i].partColor[j].id].color;
                    }
                }
                
            }
            else
            {
                weaponInHand.transform.GetChild(i).gameObject.SetActive(false);
            }

        }
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

    public void SetAdditionalFeature()
    {
        for (int i = 0; i < hairContainer.transform.childCount; i++)
        {
            if (hairContainer.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                radiusAttack *= 1.05f;
                gameObject.transform.GetChild(3).localScale *= 1.05f;
                break;
            }
        }

        if (gameObject.transform.GetChild(2).gameObject.activeInHierarchy)
        {
            speed *= 1.08f;
        }

        for (int i = 0; i < WeaponInHand.transform.childCount; i++)
        {
            if (WeaponInHand.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                switch (i)
                {
                    case 0:
                        {
                            radiusAttack += 0.1f;
                            break;
                        }
                    case 1:
                        {
                            radiusAttack += 0.3f;
                            break;
                        }
                    case 2:
                        {
                            attackForce += 0.5f;
                            break;
                        }
                    case 3:
                    case 4:
                        {
                            radiusAttack += 0.5f;
                            break;
                        }
                    case 5:
                        {
                            radiusAttack += 1f;
                            break;
                        }
                    case 6:
                        {
                            radiusAttack += 1.5f;
                            break;
                        }
                    case 7:
                        {
                            attackForce += 1.5f;
                            break;
                        }
                    case 8:
                        {
                            radiusAttack += 2f;
                            break;
                        }
                    case 9:
                        {
                            radiusAttack += 1f;
                            break;
                        }
                    case 10:
                        {
                            attackForce += 2.5f;
                            break;
                        }
                    case 11:
                        {
                            attackForce += 2.5f;
                            break;
                        }
                }
                break;
            }
        }
    }

    private IEnumerator WaitAndAttack()
    { 
        if (canAttack)
        {
            canAttack = false;
            
            yield return new WaitForSeconds(0.2f);
            if (_state != PlayerState.RUN && targetEnemy)
            {
                PoolingWeapon();
                weaponInHand.gameObject.SetActive(false);
                Attack();
            } 
        }
        
    }

    private IEnumerator WaitAnimationAttack()
    {
        yield return new WaitForSeconds(0.3f);
        _anim.attacking = false;
    }

    public void FindTargetEnemy()
    {
        if ( targetEnemy && gameObject.tag.Equals("Player"))
        {
            targetEnemy.GetChild(4).gameObject.SetActive(false);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radiusAttack / 2, enemyLayer);
        if (hitColliders.Length > 1)
        {
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
            if (gameObject.tag.Equals("Player")) targetEnemy.GetChild(4).gameObject.SetActive(true);
        }
    }
    public void CheckForEnemiesAndAttack()
    {
        if (canAttack && _state == PlayerState.IDLE && weaponInHand.activeInHierarchy && targetEnemy && Vector3.Distance(transform.position, targetEnemy.transform.position) < radiusAttack) 
        {
            if (gameObject.CompareTag("Player") && !targetEnemy.GetChild(4).gameObject.activeInHierarchy) return;
            _anim.attacking = true;
            _state = PlayerState.ATTACK;
            _anim.UpdateAnimation(_state);
            StartCoroutine(WaitAnimationAttack());               
              
            if (targetEnemy && !targetEnemy.gameObject.GetComponent<PlayerController>().IsDead)
            {
                // Quay về phía enemy
                Vector3 directionToEnemy = (targetEnemy.transform.position - transform.position).normalized;
                directionToEnemy.y = 0;
                transform.rotation = Quaternion.LookRotation(directionToEnemy);

                StartCoroutine(WaitAndAttack());
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

    private IEnumerator WaitToDestroy()
    {
        if (gameObject.CompareTag("Player"))
        {
            _joystick.SetHorizontal(0);
            _joystick.SetVertical(0);
        }
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
        if (nameDisplay) nameDisplay.gameObject.SetActive(false);
        if (levelDisplay) levelDisplay.gameObject.SetActive(false);

    }

    public void RevivePlayer()
    {
        isDead = false;
        gameObject.SetActive(true);
        if (nameDisplay) nameDisplay.gameObject.SetActive(true);
        if (levelDisplay) levelDisplay.gameObject.SetActive(true);
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
        SetWeapon();
    }

    public void ScaleCharacter()
    {
        if (gameObject.CompareTag("Player"))
        {
            float fov = Camera.main.fieldOfView;
            //Quaternion camRotate = Camera.main.transform.rotation;
            //camRotate.x *= scaleRate;
            //Camera.main.transform.rotation = camRotate;
            fov += 7f;
            Camera.main.fieldOfView = fov;
        }
        level++;
        if (levelDisplay)
        {
            //levelDisplay.transform.localScale *= scaleRate;
            levelDisplay.SetLevel(level);
            levelDisplay.offset *= scaleRate;
        }
        if (nameDisplay)
        {
            nameDisplay.offset *= scaleRate;
        }

        currScale *= scaleRate;
        this.transform.localScale *= scaleRate;
        radiusAttack *= scaleRate;
    }

    public void HittedTarget()
    {
        weaponInHand.gameObject.SetActive(true);
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
        else if (isChangingSkin)
        {
            _state = PlayerState.DANCE_CHAR_SKIN;
        } else
        {
            _state = PlayerState.IDLE;
        }
    }

    public void DisplayLevelAndName()
    {
        // Tạo đối tượng Level Display từ Prefab
        GameObject levelDisplayObject = Instantiate(levelDisplayPrefab, transform.position, Quaternion.identity, levelDisplayList.transform);
        levelDisplay = levelDisplayObject.GetComponent<LevelDisplay>();
        levelDisplay.target = this.transform;
        levelDisplay.offset = new Vector3(0, 1.3f, 0);
        levelDisplay.transform.localRotation = Quaternion.identity;
        levelDisplay.transform.GetChild(0).GetComponent<Image>().color = bodyColor.color;

        // Tạo đối tượng Level Display từ Prefab
        GameObject nameDisplayObject = Instantiate(nameDisplayPrefab, transform.position, Quaternion.identity, nameDisplayList.transform);
        nameDisplay = nameDisplayObject.GetComponent<NameDisplay>();
        nameDisplay.target = this.transform;
        nameDisplay.offset = new Vector3(0, 1.4f, 0);
        nameDisplay.transform.localRotation = Quaternion.identity;

        nameDisplay.GetComponent<Text>().color = bodyColor.color;
        nameDisplay.SetName(SaveLoadManager.Instance.LoadData().player.name);
    }
}
