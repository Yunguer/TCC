using Cinemachine;
using System.Collections;
using UnityEngine;

namespace ProjetoTCC
{
    public class BossDamageController : MonoBehaviour
    {

        #region Variaveis Unity
        private _GameController _GameController;
        private PlayerScript playerScript;
        private SpriteRenderer sRender;
        private Animator animator;
        private BossIA bossIA;
        #endregion

        [Header("Configurações Para o Combate")]
        #region Variavies para mecanicas de dano
        
        [SerializeField]
        private int enemyLife;
        public int EnemyLife => enemyLife;
        
        [SerializeField]
        private float[] damageModifier; //SISTEMA DE RESISTENCIA/FRAQUESA CONTRA DETERMINADO TIPO DE DANO
        
        private bool tookHit; // VERIFICAÇÃO PARA SE LEVOU UM HIT
        public bool TookHit => tookHit;
        
        private bool dead; // INDICA SE ESTÁ MORTO
        public bool Dead => dead;
        
        [SerializeField]
        private Color[] characterColor; // CONTROLE DE COR DO PERSONAGEM
        
        [SerializeField]
        private Transform vfxPosition;
        
        [SerializeField]
        private GameObject trigger;
        
        private PolygonCollider2D polyTrigger;

        [SerializeField]
        private GameObject leftWall;

        [SerializeField]
        private GameObject rightWall;

        [SerializeField]
        private Transform cameraPosition;

        private CinemachineVirtualCamera vcam;
        #endregion


        [Header("Configurações Para a Movimentação")]
        #region Variaveis para movimentação
        [SerializeField]
        private bool isPlayerOnLeft; //VARIAVES DE CHECAGEM DE POSIÇÃO PARA O KNOCKBACK
        public bool IsPlayerOnLeft => isPlayerOnLeft;

        #endregion

        #region Variaveis para Checagem
        [SerializeField]
        private Transform groundCheck; // OBJETO RESPONSÁVEL POR DETECTAR SE O PERSONAGEM ESTÁ SOBRE UMA SUPERFÍCIE
        [SerializeField]
        private LayerMask whatIsGround; // INDICA O QUE É SUPERFICE PARA O TESTE DO GROUNDED
        #endregion

        [Header("Configurações Para o Loot")]
        #region Variaveis para o Sistema de Loot
        [SerializeField]
        private GameObject loots;
        #endregion

        void Start()
        {
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
            playerScript = FindObjectOfType(typeof(PlayerScript)) as PlayerScript;
            bossIA = FindObjectOfType(typeof(BossIA)) as BossIA;
            sRender = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            polyTrigger = trigger.GetComponent<PolygonCollider2D>();
            vcam = FindObjectOfType(typeof(CinemachineVirtualCamera)) as CinemachineVirtualCamera;

            tookHit = false;

            sRender.color = characterColor[0];

        }

        void Update()
        {

            //VERIFICA SE O PLAYER ESTÁ A ESQUERDA OU A DIREITA DO INIMIGO
            float xPlayer = playerScript.transform.position.x;

            if (xPlayer < transform.position.x)
            {
                isPlayerOnLeft = true;
            }
            else if (xPlayer > transform.position.x)
            {
                isPlayerOnLeft = false;
            }

        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (dead == true)
            {
                return;
            }
            switch (col.gameObject.tag)
            {
                case "Weapon":

                    if (tookHit == false)
                    {
                        tookHit = true;

                        WeaponData weaponInfo = col.gameObject.GetComponent<WeaponData>();
                        animator.SetTrigger("hit");

                        float weaponDamage = weaponInfo.Damage;
                        int damageType = weaponInfo.DamageType;

                        //danoTomado = danoArma + (danoArma * (ajusteDano[id]/100))
                        float danoTomado = weaponDamage + (weaponDamage * (damageModifier[damageType] / 100));

                        enemyLife -= Mathf.RoundToInt(danoTomado);

                        if (isPlayerOnLeft)
                        {
                            GameObject fxTemp = Instantiate(_GameController.DamageFX[damageType], vfxPosition.position, transform.localRotation);
                            fxTemp.transform.localScale = new Vector3(fxTemp.transform.localScale.x * -1, fxTemp.transform.localScale.y, fxTemp.transform.localScale.z);
                            Destroy(fxTemp, 1);
                        }
                        else
                        {
                            GameObject fxTemp = Instantiate(_GameController.DamageFX[damageType], vfxPosition.position, transform.localRotation);
                            Destroy(fxTemp, 1);
                        }

                        if (enemyLife <= 0)
                        {
                            dead = true;
                            bossIA.CurrentBossRoutine = BossIA.BossRoutine.E;
                            animator.SetInteger("idAnimation", 3);
                            StartCoroutine("Loot");
                        }

                        StartCoroutine("Invulnerable");

                    }
                    break;

                case "Player":
                    if (!dead)
                    {
                        if (playerScript.TookHit == false)
                        {
                            playerScript.TookHit = true;
                            playerScript.PlayerRb.velocity = new Vector2(0, playerScript.PlayerRb.velocity.y);
                            _GameController.CurrentLife -= 1;
                            playerScript.PlayerAnimator.SetTrigger("hit");

                            if (isPlayerOnLeft && !playerScript.IsLookingLeft)
                            {
                                GameObject fxTemp = Instantiate(_GameController.DamageFX[0], playerScript.VfxPosition.position, playerScript.transform.localRotation);
                                Destroy(fxTemp, 1);
                            }
                            else if (isPlayerOnLeft && playerScript.IsLookingLeft)
                            {
                                GameObject fxTemp = Instantiate(_GameController.DamageFX[0], playerScript.VfxPosition.position, transform.localRotation);
                                fxTemp.transform.localScale = new Vector3(fxTemp.transform.localScale.x * -1, fxTemp.transform.localScale.y, fxTemp.transform.localScale.z);
                                Destroy(fxTemp, 1);
                            }
                            else if (!isPlayerOnLeft && !playerScript.IsLookingLeft)
                            {
                                GameObject fxTemp = Instantiate(_GameController.DamageFX[0], playerScript.VfxPosition.position, playerScript.transform.localRotation);
                                fxTemp.transform.localScale = new Vector3(fxTemp.transform.localScale.x * -1, fxTemp.transform.localScale.y, fxTemp.transform.localScale.z);
                                Destroy(fxTemp, 1);
                            }
                            else if (!isPlayerOnLeft && playerScript.IsLookingLeft)
                            {
                                GameObject fxTemp = Instantiate(_GameController.DamageFX[0], playerScript.VfxPosition.position, playerScript.transform.localRotation);
                                fxTemp.transform.localScale = new Vector3(fxTemp.transform.localScale.x * -1, fxTemp.transform.localScale.y, fxTemp.transform.localScale.z);
                                Destroy(fxTemp, 1);
                            }

                            if (isPlayerOnLeft && playerScript.IsLookingLeft)
                            {
                                playerScript.KxTemp = playerScript.KnockX * -1;
                                print(playerScript.KxTemp);
                            }
                            else if (isPlayerOnLeft && !playerScript.IsLookingLeft)
                            {
                                playerScript.KxTemp = playerScript.KnockX;
                                print(playerScript.KxTemp);
                            }
                            else if (!isPlayerOnLeft && playerScript.IsLookingLeft)
                            {
                                playerScript.KxTemp = playerScript.KnockX;
                                print(playerScript.KxTemp);
                            }
                            else if (!isPlayerOnLeft && !playerScript.IsLookingLeft)
                            {
                                playerScript.KxTemp = playerScript.KnockX * -1;
                                print(playerScript.KxTemp);
                            }

                            playerScript.KnockPosition.localPosition = new Vector3(playerScript.KxTemp, playerScript.KnockPosition.localPosition.y, 0);

                            GameObject knockTemp = Instantiate(playerScript.KnockForcePrefab, playerScript.KnockPosition.position, playerScript.KnockPosition.localRotation);
                            Destroy(knockTemp, 0.02f);

                            if (_GameController.CurrentLife <= 0)
                            {
                                //qnd morrer
                            }

                            StartCoroutine(nameof(InvulnerablePlayer));
                        }
                    }
                    break;

            }

        }


        IEnumerator Loot()
        {
            polyTrigger.enabled = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            yield return new WaitForSeconds(2f);
            leftWall.SetActive(false);
            rightWall.SetActive(false);
            vcam.Follow = cameraPosition;
        }

        IEnumerator Invulnerable()
        {
            sRender.color = characterColor[1];
            yield return new WaitForSeconds(0.5f);
            sRender.color = characterColor[0];
            yield return new WaitForSeconds(0.1f);

            tookHit = false;
        }

        IEnumerator InvulnerablePlayer()
        {
            playerScript.PRender.color = playerScript.CharacterColor[1];
            yield return new WaitForSeconds(0.4f);
            playerScript.PRender.color = playerScript.CharacterColor[0];
            playerScript.TookHit = false;
        }
    }
}
