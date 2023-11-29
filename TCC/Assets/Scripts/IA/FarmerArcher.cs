using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{

    public class FarmerArcher : MonoBehaviour
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
        private string weaponID;

        [SerializeField]
        private int classID;

        [SerializeField]
        private bool isAlertOnHit;

        #endregion
        void Start()
        {
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
            enemyDamageController = gameObject.GetComponent<EnemyDamageController>();
            playerScript = FindObjectOfType(typeof(PlayerScript)) as PlayerScript;
            rigidbody2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            isAlertOnHit = false;

            if (!isLookingLeft)
            {
                Flip();
            }
            ChangeWeapon(weaponID);


        }


        void Update()
        {

            if (enemyDamageController.TookHit == true)
            {
                foreach (GameObject o in weapons)
                {
                    o.SetActive(false);
                }
                return;
            }

            if (currentEnemyState != EnemyState.ATTACKING && currentEnemyState != EnemyState.RETREAT)
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

            if (currentEnemyState == EnemyState.ALERT && playerScript.IsGrounded)
            {
                float dist = transform.position.x - playerScript.transform.position.x;

                var isLookingToPlayer = enemyDamageController.IsPlayerOnLeft ? isLookingLeft : !isLookingLeft;

                var isClose = Mathf.Abs(dist) <= atackDistance;
                var isAlert = Mathf.Abs(dist) <= leaveAlertDistance || isAlertOnHit;

                if (isClose && isLookingToPlayer)
                {
                    ChangeState(EnemyState.ATTACKING);
                }
                else if (!isAlert || !isLookingToPlayer)
                {
                    print("Saiu alerta");
                    ChangeState(EnemyState.STOPPED);
                    isAlertOnHit = false;
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


            animator.SetFloat("classID", classID);

        }

        void Flip()
        {
            isLookingLeft = !isLookingLeft; // INVERTE O VALOR DA VARIAVEL BOLEANA
            enemyDamageController.IsLookingLeft = isLookingLeft;
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

            switch (newState)
            {
                case EnemyState.STOPPED:
                    Speed = 0;
                    StartCoroutine(nameof(Idle));
                    break;
                case EnemyState.PATROL:
                    Speed = baseSpeed;
                    break;
                case EnemyState.ALERT:
                    Speed = 0;
                    break;
                case EnemyState.ATTACKING:
                    animator.SetTrigger("atack");
                    StartCoroutine(nameof(AwaitToToAttack));
                    
                    break;
                case EnemyState.RETREAT:
                    if(enemyDamageController.EnemyLife > 0)
                    {
                        var isLookingToPlayer = enemyDamageController.IsPlayerOnLeft ? isLookingLeft : !isLookingLeft;
                        if (isLookingToPlayer)
                        {
                            Flip();
                        }
                        Speed = baseSpeed * 2;
                        StartCoroutine(nameof(Retreat));
                    }
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

        public void ChangeWeapon(string id)
        {
            var weapon = _GameController.WeaponProvider.GetWeaponById(id);

            switch (weapon.WeaponType)
            {
                case WeaponType.Melee:

                    for (int i = 0; i < weapons.Length; i++)
                    {
                        if (weapon.AnimationIcons.Length > i)
                        {
                            weapons[i].GetComponent<SpriteRenderer>().sprite = weapon.AnimationIcons[i];
                        }
                    }

                    break;

                case WeaponType.Bow:

                    for (int i = 0; i < bows.Length; i++)
                    {
                        if (weapon.AnimationIcons.Length > i)
                        {
                            bows[i].GetComponent<SpriteRenderer>().sprite = weapon.AnimationIcons[i];
                        }
                    }

                    break;

                case WeaponType.Staff:

                    for (int i = 0; i < staffs.Length; i++)
                    {
                        if (weapon.AnimationIcons.Length > i)
                        {
                            staffs[i].GetComponent<SpriteRenderer>().sprite = weapon.AnimationIcons[i];
                        }
                    }

                    break;
            }
        }

        public void TookHit()
        {
            isAlertOnHit = true;
            if(enemyDamageController.EnemyLife > 0)
            {
                var isLookingToPlayer = enemyDamageController.IsPlayerOnLeft ? isLookingLeft : !isLookingLeft;
                float dist = transform.position.x - playerScript.transform.position.x;

                ChangeState(EnemyState.RETREAT);
                if (Mathf.Abs(dist) >= leaveAlertDistance)
                {
                    StartCoroutine(nameof(AwaitToLeaveAlert));
                }
            }
            isAlertOnHit = false;
        }

        IEnumerator AwaitToLeaveAlert()
        {
            yield return new WaitForSeconds(1);
        }


        IEnumerator Idle()
        {
            yield return new WaitForSeconds(waitingTimeIdle);
            if(!isAlertOnHit && currentEnemyState != EnemyState.RETREAT)
            {
                Flip();
                ChangeState(EnemyState.PATROL);
            }
        }

        IEnumerator Retreat()
        {
            yield return new WaitForSeconds(waitingTimeRetreat);
            Flip();
            ChangeState(EnemyState.ALERT);
        }

        IEnumerator AwaitToToAttack()
        {
            yield return new WaitForSeconds(2);
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Atack") && currentEnemyState != EnemyState.RETREAT)
            {
                ChangeState(EnemyState.ALERT);
            }
        }
    }
}
