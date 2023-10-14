using Cainos.PixelArtPlatformer_VillageProps;
using UnityEngine;

namespace ProjetoTCC
{
    public class PlayerScript : MonoBehaviour
    {
        #region Variaveis Unity
        private Animator playerAnimator;
        private Rigidbody2D playerRb;
        #endregion

        #region Comunicação Entre Scripts
        private _GameController _GameController;
        #endregion

        [Header("Configurção de Vida/Mana do Player")]
        #region Variaveis para Vida/Mana
        [SerializeField]
        private int maxLife;
        public int MaxLife => maxLife;
        [SerializeField]
        private int currentLife;
        public int CurrentLife => currentLife;
        [SerializeField]
        private int maxMana;
        public int MaxMana => maxMana;
        [SerializeField]
        private int currentMana;
        public int CurrentMana => currentMana;
        #endregion

        [Header("Triggers Para a Movimentação")]
        #region Variaveis para Checagem
        [SerializeField]
        private Transform groundCheck; // OBJETO RESPONSÁVEL POR DETECTAR SE O PERSONAGEM ESTÁ SOBRE UMA SUPERFÍCIE
        [SerializeField]
        private LayerMask whatIsGround; // INDICA O QUE É SUPERFICE PARA O TESTE DO GROUNDED
        #endregion

        [Header("Configuração de Movimentação")]
        #region Variaveis de Mecânicas 
        [SerializeField]
        private float speed; // VELOCIDADE DE MOVIMENTO DO PERSONAGEM
        [SerializeField]
        private float jumpForce; // FORÇA APLICADA PARA GERAR O PULO DO PERSONAGEM
        [SerializeField]
        private bool isGrounded; // INDICA SE O PERSONAGEM ESTÁ PISANDO EM ALGUMA SUPERFÍCE
        [SerializeField]
        private bool isLookingLeft; // INDICA SE O PERSONAGEM ESTÁ VIRADO PARA A ESQUERDA
        [SerializeField]
        private bool isAttacking; // INDICAR SE O PERSONAGEM ESTÁ EXECUTANDO UM ATAQUE 
        [SerializeField]
        private int idAnimation; // INDICA O ID DA ANIMAÇÃO
        [SerializeField]
        private Collider2D standing, crounching; // COLISÃO EM PÉ E AGACHADO
        private float h, v; // VALORES HORIZONTAL E VERTICAL
        #endregion

        [Header("Configuração de Interação")]
        #region Variaveis para Interação de Objetos
        //INTERAÇÃO COM ITENS E OBJETOS
        [SerializeField]
        private Transform hand;
        private Vector3 dir = Vector3.right;
        [SerializeField]
        private LayerMask interaction;
        [SerializeField]
        private GameObject objectInteraction;
        private bool isInteractionAvailable = true;
        [SerializeField] 
        private GameObject interactionBaloon;
        [SerializeField]
        private GameObject interactionBaloonKey;
        private bool canInteract = true;
        public bool CanInteract
        {
            get
            {
                return canInteract;
            }
            set
            {
                canInteract = value;
            }
        }
        #endregion

        [Header("Configuração de Armas")]
        #region Variaveis para as Armas
        //SISTEMA DE ARMAS
        [SerializeField]
        private int weaponID;
        [SerializeField]
        private int currentWeaponID;
        [SerializeField]
        private GameObject[] weapons, bows, staffs, arrows;
        [SerializeField]
        private GameObject prefabArrow, prefabMagic;
        [SerializeField]
        private Transform spawnArrow, spawnMagic;
        #endregion


        private void OnEnable() // FUNÇÃO PARA EXECUTAR A ANIMAÇÃO DOS OBJETOS UMA VEZ
        {
            SimpleChest.OnAnimationEnded += OnInteractionAnimationEnded;
            EntranceWay.OnAnimationEnded += OnInteractionAnimationEnded;
            NextScene.OnAnimationEnded += OnInteractionAnimationEnded;
        }

        private void OnDisable() // FUNÇÃO PARA EXECUTAR A ANIMAÇÃO DOS OBJETOS UMA VEZ
        {
            SimpleChest.OnAnimationEnded -= OnInteractionAnimationEnded;
            EntranceWay.OnAnimationEnded -= OnInteractionAnimationEnded;
            NextScene.OnAnimationEnded -= OnInteractionAnimationEnded;
        }

        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;

            //CARREGANDO OS DADOS INICIAIS DO PERSONAGEM

            maxLife = _GameController.MaxLife;
            maxMana = _GameController.MaxMana;
            weaponID = _GameController.WeaponID;

            playerRb = GetComponent<Rigidbody2D>(); // ASSOSSIA O COMPONENTE A VARÁVEL
            playerAnimator = GetComponent<Animator>(); // ASSOSSIA O COMPONENTE A VARIÁVEL

            currentLife = maxLife;
            currentMana = maxMana;

            foreach (GameObject o in weapons)
            {
                o.SetActive(false);
            }
            foreach (GameObject o in bows)
            {
                o.SetActive(false);
            }
            foreach (GameObject o in staffs)
            {
                o.SetActive(false);
            }

            ChangeWeapon(weaponID);
        }

        void FixedUpdate() // TAXA DE ATUALIZAÇÃO FIXA DE 0.02
        {
            if(_GameController.CurrentGameState != GameState.GAMEPLAY)
            {
                return;
            }

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f, whatIsGround);
            playerRb.velocity = new Vector2(h * speed, playerRb.velocity.y);
        }

        private void LateUpdate()
        {
            if (_GameController.WeaponID != _GameController.CurrentWeaponID)
            {
                ChangeWeapon(_GameController.WeaponID);
            }
        }

        void Update()
        {

            if (_GameController.CurrentGameState != GameState.GAMEPLAY)
            {
                return;
            }
            if(!canInteract)
            {
                idAnimation = 0;
                playerAnimator.SetInteger("idAnimation", idAnimation);
                h = 0;
                return;
            }

            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");

            if (h > 0 && isLookingLeft == true && isAttacking == false)
            {
                Flip();
            }
            else if (h < 0 && isLookingLeft == false && isAttacking == false)
            {
                Flip();
            }


            if (v < 0)
            {
                idAnimation = 2;
                if (isGrounded == true)
                {
                    h = 0;
                }
            }
            else if (h != 0)
            {
                idAnimation = 1;
            }
            else
            {
                idAnimation = 0;
            }

            if (Input.GetButtonDown("Fire1") && v >= 0 && isAttacking == false && objectInteraction == null)
            {
                playerAnimator.SetTrigger("atack");
            }

            if (Input.GetButtonDown("Fire1") && v >= 0 && isAttacking == false && objectInteraction != null)
            {
                if (objectInteraction.tag == "Entrance")
                {
                    objectInteraction.GetComponent<EntranceWay>().TPlayer = this.transform;
                }
                if (isInteractionAvailable)
                {
                    isInteractionAvailable = false;
                    objectInteraction.SendMessage("Interaction", SendMessageOptions.DontRequireReceiver);
                }
            }

            if (Input.GetButtonDown("Jump") && isGrounded == true && isAttacking == false)
            {
                playerRb.AddForce(new Vector2(0, jumpForce));
                crounching.enabled = false;
                standing.enabled = true;
            }

            if(Input.GetKeyDown(KeyCode.Alpha1) && isAttacking == false)
            {
                ChangeWeapon(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && isAttacking == false)
            {
                ChangeWeapon(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && isAttacking == false)
            {
                ChangeWeapon(5);
            }

            if (isAttacking == true && isGrounded == true)
            {
                h = 0;
            }

            if (v < 0 && isGrounded == true)
            {
                crounching.enabled = true;
                standing.enabled = false;
            }
            else if (v >= 0 && isGrounded == true)
            {
                crounching.enabled = false;
                standing.enabled = true;
            }
            else if (v != 0 && isGrounded == false)
            {
                crounching.enabled = false;
                standing.enabled = true;
            }

            playerAnimator.SetBool("grounded", isGrounded);
            playerAnimator.SetInteger("idAnimation", idAnimation);
            playerAnimator.SetFloat("speedY", playerRb.velocity.y);
            playerAnimator.SetFloat("weaponClassID", _GameController.WeaponClassID[_GameController.CurrentWeaponID]);

            Interact();

            if(_GameController.ArrowQnt > 0)
            {
                arrows[0].SetActive(true);
                arrows[1].SetActive(true);
            }
            else
            {
                arrows[0].SetActive(false);
                arrows[1].SetActive(false);
            }
        }


        void Flip()
        {
            isLookingLeft = !isLookingLeft; // INVERTE O VALOR DA VARIAVEL BOLEANA
            float x = transform.localScale.x;
            x *= -1; // INVERTE O FINAL DO SCALE X
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
            dir.x = x;
            interactionBaloonKey.transform.localScale = new Vector3(interactionBaloonKey.transform.localScale.x * -1, interactionBaloonKey.transform.localScale.y, interactionBaloonKey.transform.localScale.z);
        }

        public void Attack(int atk)
        {
            switch (atk)
            {
                case 0:
                    isAttacking = false;
                    weapons[2].SetActive(false);
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
                    break;

                case 1:
                    
                    isAttacking = true;
                    break;

                case 2:
                    if(_GameController.ArrowQnt > 0)
                    {
                        _GameController.ArrowQnt--;
                        GameObject tempPrefab = Instantiate(prefabArrow, spawnArrow.position, spawnArrow.localRotation);
                        tempPrefab.transform.localScale = new Vector3(tempPrefab.transform.localScale.x * dir.x, tempPrefab.transform.localScale.y, tempPrefab.transform.localScale.z);
                        tempPrefab.GetComponent<Rigidbody2D>().velocity = new Vector2(5 * dir.x, 0);
                        Destroy(tempPrefab, 2);
                    }
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
                    break;
                case 1:
                    isAttacking = true;
                    break;

                case 2:
                    if(currentMana >= 1)
                    {
                        GameObject tempPrefab = Instantiate(prefabMagic, spawnMagic.position, spawnMagic.localRotation);
                        tempPrefab.GetComponent<Rigidbody2D>().velocity = new Vector2(3 * dir.x, 0);
                        Destroy(tempPrefab, 1);
                        currentMana = currentMana - 1;
                    }
                    break;
            }
        }

        void Interact()
        {
            UnityEngine.Debug.DrawRay(hand.position, dir * 0.1f, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(hand.position, dir, 0.1f, interaction);

            if (hit == true)
            {
                objectInteraction = hit.collider.gameObject;
                interactionBaloon.SetActive(true);
                interactionBaloonKey.SetActive(true);
                
            }
            else
            {
                objectInteraction = null;
                interactionBaloon.SetActive(false);
                interactionBaloonKey.SetActive(false);
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

        private void OnTriggerEnter2D(Collider2D col)
        {
            switch(col.gameObject.tag)
            {
                case "Collectable":
                    col.gameObject.SendMessage("Collect", SendMessageOptions.DontRequireReceiver);
                    break;
            }       
        }

        private void OnInteractionAnimationEnded()
        {
            isInteractionAvailable = true;
        }

        public void ChangeWeapon(int id)
        {
            WeaponData tempWeaponData;
            weaponID = id;

            switch(_GameController.WeaponClassID[weaponID])
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
            
            _GameController.CurrentWeaponID = _GameController.WeaponID;
        }

    }
}
