using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    public enum EnemyState 
    { 
        STOPPED,
        ALERT,
        PATROL,
        ATTACKING,
        RETREAT
    }

    public class Farmer : MonoBehaviour
    {
        private _GameController _GameController;
        private EnemyDamageController enemyDamageController;
        private PlayerScript playerScript;

        #region Variavies de Componentes
#pragma warning disable CS0108 // O membro oculta o membro herdado; nova palavra-chave ausente
        private Rigidbody2D rigidbody2D;
#pragma warning restore CS0108 // O membro oculta o membro herdado; nova palavra-chave ausente
        
        private Animator animator;

        [SerializeField]
        private EnemyState currentEnemyState;
        #endregion

        [Header("Configuração para a Movimentação da IA")]
        #region Variaveis para a Movimentação da IA
        
        [SerializeField]
        private float changeRoteDistance;

        [SerializeField]
        private LayerMask obstaclesLayer;

        [SerializeField]
        private float seeCharacterDistance;

        [SerializeField]
        private LayerMask characterLayer;

        [SerializeField]
        private float atackDistance;

        [SerializeField]
        private float leaveAlertDistance;

        private Vector3 dir = Vector3.right;
        
        [SerializeField]
        private float baseSpeed;
        
        [SerializeField]
        private float Speed;
        
        [SerializeField]
        private bool isLookingLeft;

        [SerializeField]
        private float waitingTimeIdle;

        [SerializeField]
        private float waitingTimeRetreat;

        private bool isAttacking;

        [SerializeField]
        private GameObject[] weapons, bows, staffs;

        [SerializeField]
        private int weaponID;

        [SerializeField]
        private int classID;

        [SerializeField]
        private bool isAlertOnHit;

        #endregion
        void Start()
        {
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
            enemyDamageController = FindObjectOfType(typeof(EnemyDamageController)) as EnemyDamageController;
            playerScript = FindObjectOfType(typeof(PlayerScript)) as PlayerScript;
            rigidbody2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            isAlertOnHit = false;
            enemyDamageController.IsLookingLeft = isLookingLeft;
            if(!isLookingLeft)
            {
                Flip();
            }
            ChangeWeapon(classID);
        }


        void Update()
        { 
            if(enemyDamageController.TookHit == true)
            {
                return;
            }

            if(currentEnemyState != EnemyState.ATTACKING && currentEnemyState != EnemyState.RETREAT)
            {
                UnityEngine.Debug.DrawRay(transform.position, dir * seeCharacterDistance, Color.red);
                RaycastHit2D hitCharacter = Physics2D.Raycast(transform.position, dir, seeCharacterDistance, characterLayer);
                if (hitCharacter)
                {
                    ChangeState(EnemyState.ALERT);
                }
            }
           
            if (currentEnemyState == EnemyState.PATROL)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, changeRoteDistance, obstaclesLayer);

                if (hit == true)
                {
                    ChangeState(EnemyState.STOPPED);
                }
            }

            if (currentEnemyState == EnemyState.RETREAT)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, changeRoteDistance, obstaclesLayer);

                if (hit == true)
                {
                    Flip();
                }
            }

            if (currentEnemyState == EnemyState.ALERT)
            {
                float dist = Vector3.Distance(transform.position, playerScript.transform.position);

                if (dist <= atackDistance)
                {
                    ChangeState(EnemyState.ATTACKING);
                }
                else if (dist >= leaveAlertDistance && isAlertOnHit == false)
                {
                    print("Saiu alerta");
                    ChangeState(EnemyState.STOPPED);
                }
            }

            rigidbody2D.velocity = new Vector2(Speed, rigidbody2D.velocity.y);
            if (Speed == 0)
            {
                animator.SetInteger("idAnimation", 0);
            }
            else if (Speed != 0)
            {
                animator.SetInteger("idAnimation", 1);
            }
            enemyDamageController.IsLookingLeft = isLookingLeft;
            animator.SetFloat("classID", classID);

        }

        void Flip()
        {
            isLookingLeft = !isLookingLeft; // INVERTE O VALOR DA VARIAVEL BOLEANA
            float x = transform.localScale.x;
            x *= -1; // INVERTE O FINAL DO SCALE X
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
            dir.x = x;
            baseSpeed *= -1;
            float currentSpeed = Speed * -1;
            Speed = currentSpeed;
        }

        public void ChangeState(EnemyState newState)
        {
            currentEnemyState = newState;

            switch(newState)
            {
                case EnemyState.STOPPED:
                    Speed = 0;
                    StartCoroutine(nameof(Idle));
                    break;
                case EnemyState.PATROL:
                    Speed = baseSpeed;
                    break;
                case EnemyState.ALERT:
                    print("alerta");
                    Speed = 0;
                    break;
                case EnemyState.ATTACKING:
                    print("ataque");
                    animator.SetTrigger("atack");
                    break;
                case EnemyState.RETREAT:
                    Flip();
                    Speed = baseSpeed * 2;
                    StartCoroutine(nameof(Retreat));
                    break;
            }       
        }

        public void Attack(int atk)
        {
            switch (atk)
            {
                case 0:
                    isAttacking = false;
                    weapons[2].SetActive(false);
                    ChangeState(EnemyState.RETREAT);
                    break;
                case 1:
                    isAttacking = true;
                    break;
            }
        }

        public void BowAttack(int atk)
        {
            switch (atk)
            {
                case 0:

                    isAttacking = false;
                    bows[2].SetActive(false);
                    ChangeState(EnemyState.RETREAT);
                    break;

                case 1:

                    isAttacking = true;
                    break;

                case 2:
                    //WeaponData ArrowWeaponData = _GameController.ArrowPrefab[_GameController.EquipedArrowID].GetComponent<WeaponData>();
                    //GameObject tempPrefab = Instantiate(_GameController.ArrowPrefab[_GameController.EquipedArrowID], spawnArrow.position, spawnArrow.localRotation);
                    //tempPrefab.transform.localScale = new Vector3(tempPrefab.transform.localScale.x * dir.x, tempPrefab.transform.localScale.y, tempPrefab.transform.localScale.z);
                    //tempPrefab.GetComponent<Rigidbody2D>().velocity = new Vector2(5 * dir.x, 0);
                    //Destroy(tempPrefab, 2);

                    break;
            }
        }

        public void StaffAttack(int atk)
        {
            switch (atk)
            {
                case 0:
                    isAttacking = false;
                    staffs[3].SetActive(false);
                    ChangeState(EnemyState.RETREAT);
                    break;
                case 1:
                    isAttacking = true;
                    break;

                case 2:
                    //GameObject tempPrefab = Instantiate(prefabMagic, spawnMagic.position, spawnMagic.localRotation);
                    //tempPrefab.GetComponent<Rigidbody2D>().velocity = new Vector2(3 * dir.x, 0);
                    //Destroy(tempPrefab, 1);
                    //_GameController.CurrentMana = _GameController.CurrentMana - 1;
                    break;
            }
        }

        void WeaponControl(int id)
        {
            foreach (GameObject o in weapons)
            {
                o.SetActive(false);
            }

            weapons[id].SetActive(true);

        }

        void BowControl(int id)
        {
            foreach (GameObject o in bows)
            {
                o.SetActive(false);
            }

            bows[id].SetActive(true);

        }

        void StaffControl(int id)
        {
            foreach (GameObject o in staffs)
            {
                o.SetActive(false);
            }

            staffs[id].SetActive(true);

        }

        public void ChangeWeapon(int id)
        {
            WeaponData tempWeaponData;

            switch (id)
            {
                case 0: // Espadas, Machados, Martelos, Adagas, Maças

                    weapons[0].GetComponent<SpriteRenderer>().sprite = _GameController.WeaponsSprites_1[weaponID];
                    tempWeaponData = weapons[0].GetComponent<WeaponData>();
                    tempWeaponData.Damage = _GameController.WeaponDamage[weaponID];
                    tempWeaponData.DamageType = _GameController.WeaponDamageType[weaponID];


                    weapons[1].GetComponent<SpriteRenderer>().sprite = _GameController.WeaponsSprites_2[weaponID];
                    tempWeaponData = weapons[1].GetComponent<WeaponData>();
                    tempWeaponData.Damage = _GameController.WeaponDamage[weaponID];
                    tempWeaponData.DamageType = _GameController.WeaponDamageType[weaponID];

                    weapons[2].GetComponent<SpriteRenderer>().sprite = _GameController.WeaponsSprites_3[weaponID];
                    tempWeaponData = weapons[2].GetComponent<WeaponData>();
                    tempWeaponData.Damage = _GameController.WeaponDamage[weaponID];
                    tempWeaponData.DamageType = _GameController.WeaponDamageType[weaponID];

                    _GameController.WeaponID = id;

                    break;

                case 1: // Arcos

                    bows[0].GetComponent<SpriteRenderer>().sprite = _GameController.WeaponsSprites_1[weaponID];
                    bows[1].GetComponent<SpriteRenderer>().sprite = _GameController.WeaponsSprites_2[weaponID];
                    bows[2].GetComponent<SpriteRenderer>().sprite = _GameController.WeaponsSprites_3[weaponID];

                    _GameController.WeaponID = id;

                    break;

                case 2: //Cajados

                    staffs[0].GetComponent<SpriteRenderer>().sprite = _GameController.WeaponsSprites_1[weaponID];
                    staffs[1].GetComponent<SpriteRenderer>().sprite = _GameController.WeaponsSprites_2[weaponID];
                    staffs[2].GetComponent<SpriteRenderer>().sprite = _GameController.WeaponsSprites_3[weaponID];
                    staffs[3].GetComponent<SpriteRenderer>().sprite = _GameController.WeaponsSprites_4[weaponID];

                    _GameController.WeaponID = id;

                    break;

            }
        }

        public void TookHit()
        {
            isAlertOnHit = true;
            StartCoroutine(nameof(AwaitAfterHit));
            ChangeState(EnemyState.ALERT);
        }

        IEnumerator AwaitAfterHit()
        {
            yield return new WaitForSeconds(1);
            isAlertOnHit = false;
        }


        IEnumerator Idle()
        {
            yield return new WaitForSeconds(waitingTimeIdle);
            Flip();
            ChangeState(EnemyState.PATROL);
        }

        IEnumerator Retreat()
        {
            yield return new WaitForSeconds(waitingTimeRetreat);
            Flip();
            ChangeState(EnemyState.ALERT);
        }
    }
}
