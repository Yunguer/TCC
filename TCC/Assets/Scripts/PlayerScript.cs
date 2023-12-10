using Cainos.PixelArtPlatformer_VillageProps;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    public enum PlayerType
    { 
        Curupira,
        Farmer,
        Saci,
        Boto
    }

    public class PlayerScript : MonoBehaviour
    {  

        #region Variaveis Unity
        private Animator playerAnimator;
        public Animator PlayerAnimator => playerAnimator;
        private Rigidbody2D playerRb;
        public Rigidbody2D PlayerRb
        {
            get
            {
                return playerRb;
            }
            set
            {
                playerRb = value;
            }
        }

        private SpriteRenderer pRender;
        public SpriteRenderer PRender
        {
            get
            {
                return pRender;
            }
            set
            {
                pRender = value;
            }
        }
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
        public bool IsGrounded => isGrounded;
        
        [SerializeField]
        private bool isLookingLeft; // INDICA SE O PERSONAGEM ESTÁ VIRADO PARA A ESQUERDA
        public bool IsLookingLeft => isLookingLeft;

        [SerializeField]
        private bool isAttacking;
        public bool IsAttacking// INDICAR SE O PERSONAGEM ESTÁ EXECUTANDO UM ATAQUE 
        {
            get
            {
                return isAttacking;
            }
            set
            {
                isAttacking = value;
            }
        }

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
        private GameObject[] weapons, bows, staffs, arrows;
        [SerializeField]
        private GameObject prefabMagic;
        [SerializeField]
        private Transform spawnArrow, spawnMagic;
        #endregion

        [Header("Configuração de Dano")]
        #region Variaveis para Dano Tomado
        
        [SerializeField]
        private Color[] characterColor; // CONTROLE DE COR DO PERSONAGEM
        public Color[] CharacterColor => characterColor;

        [SerializeField]
        private Transform vfxPosition;
        public Transform VfxPosition => vfxPosition;

        [SerializeField]
        private GameObject knockForcePrefab; //FORÇA DE REPULSÃO
        public GameObject KnockForcePrefab => knockForcePrefab;

        [SerializeField]
        private Transform knockPosition; // PONTO DE ORIGEM DA FORÇA
        public Transform KnockPosition => knockPosition;

        [SerializeField]
        private float knockX; // VALOR PADRÃO DO POSITION X
        public float KnockX => knockX;

        private float kxTemp;
        public float KxTemp
        {
            get
            {
                return kxTemp;
            }
            set
            {
                kxTemp = value;
            }
        }

        private bool tookHit;
        public bool TookHit
        {
            get
            {
                return tookHit;
            }
            set
            {
                tookHit = value;
            }
        }
        #endregion


        private void OnEnable() // FUNÇÃO PARA EXECUTAR A ANIMAÇÃO DOS OBJETOS UMA VEZ
        {
            SimpleChest.OnAnimationEnded += OnInteractionAnimationEnded;
            EntranceWay.OnAnimationEnded += OnInteractionAnimationEnded;
            FinishGame.OnAnimationEnded += OnInteractionAnimationEnded;
            NPC.OnAnimationEnded += OnInteractionAnimationEnded;
            NPC_START.OnAnimationEnded += OnInteractionAnimationEnded;
            NPC_BOSS.OnAnimationEnded += OnInteractionAnimationEnded;
            NPC_POS_BOSS.OnAnimationEnded += OnInteractionAnimationEnded;

        }

        private void OnDisable() // FUNÇÃO PARA EXECUTAR A ANIMAÇÃO DOS OBJETOS UMA VEZ
        {
            SimpleChest.OnAnimationEnded -= OnInteractionAnimationEnded;
            EntranceWay.OnAnimationEnded -= OnInteractionAnimationEnded;
            FinishGame.OnAnimationEnded -= OnInteractionAnimationEnded;
            NPC.OnAnimationEnded -= OnInteractionAnimationEnded;
            NPC_BOSS.OnAnimationEnded -= OnInteractionAnimationEnded;
            NPC_POS_BOSS.OnAnimationEnded -= OnInteractionAnimationEnded;
        }

        void Start()
        {
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;

            //CARREGANDO OS DADOS INICIAIS DO PERSONAGEM

            maxLife = _GameController.MaxLife;
            maxMana = _GameController.MaxMana;

            tookHit = false;



            pRender = GetComponent<SpriteRenderer>();
            playerRb = GetComponent<Rigidbody2D>(); // ASSOSSIA O COMPONENTE A VARÁVEL
            playerAnimator = GetComponent<Animator>(); // ASSOSSIA O COMPONENTE A VARIÁVEL

            currentLife = maxLife;
            currentMana = _GameController.CurrentMana;

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

            ChangeWeapon(_GameController.WeaponProvider.GetWeaponById(_GameController.CurrentWeaponId));

        }

        void FixedUpdate() // TAXA DE ATUALIZAÇÃO FIXA DE 0.02
        {
            if(_GameController.CurrentGameState != GameState.GAMEPLAY)
            {
                return;
            }

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f, whatIsGround);
              
        }

        void Update()
        {
            if(_GameController.CurrentGameState == GameState.DIALOGO)
            {
                playerRb.velocity = new Vector2(0, playerRb.velocity.y);
                playerAnimator.SetInteger("idAnimation", 0);

                if(Input.GetButtonDown("Fire1"))
                {
                    objectInteraction.SendMessage("Talk", SendMessageOptions.DontRequireReceiver);
                }
            }

            if(_GameController.CurrentLife <= 0)
            {
                playerAnimator.SetInteger("idAnimation", 3);
                playerRb.velocity = new Vector2(0, playerRb.velocity.y);
                return;
            }

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

            if(tookHit)
            {
                playerAnimator.ResetTrigger("atack");
                IsAttacking = false;
                for (int i = 0; i < weapons.Length; i++)
                {
                    weapons[i].SetActive(false);
                }

                for (int i = 0; i < bows.Length; i++)
                {
                    bows[i].SetActive(false);
                }

                for (int i = 0; i < staffs.Length; i++)
                {
                    staffs[i].SetActive(false);
                }
            }

            if (Input.GetButtonDown("Fire1") && v >= 0 && isAttacking == false && objectInteraction == null && !tookHit)
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
            if(_GameController.CurrentWeapon != null)
            {
                playerAnimator.SetFloat("weaponClassID", (int)_GameController.CurrentWeapon.WeaponType);
            }
            

            Interact();

            if(_GameController.ArrowQnt != null)
            {
                if (_GameController.ArrowQnt[_GameController.EquipedArrowID] > 0)
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

            if (!tookHit)
            {
                playerRb.velocity = new Vector2(h * speed, playerRb.velocity.y);
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
                    if(_GameController.ArrowQnt[_GameController.EquipedArrowID] > 0)
                    {
                        _GameController.ArrowQnt[_GameController.EquipedArrowID]--;
                        GameObject tempPrefab = Instantiate(_GameController.ArrowPrefab[_GameController.EquipedArrowID], spawnArrow.position, spawnArrow.localRotation);
                        tempPrefab.GetComponent<WeaponData>().Damage = _GameController.CurrentWeapon.Damage * tempPrefab.GetComponent<WeaponData>().Damage;
                        tempPrefab.GetComponent<WeaponData>().DamageType = (int)_GameController.CurrentWeapon.DamageType;
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
                    if(_GameController.CurrentMana >= 1)
                    {
                        _GameController.CurrentMana = _GameController.CurrentMana - 1;
                        GameObject tempPrefab = Instantiate(prefabMagic, spawnMagic.position, spawnMagic.localRotation);
                        tempPrefab.GetComponent<WeaponData>().Damage = _GameController.CurrentWeapon.Damage;
                        tempPrefab.GetComponent<WeaponData>().DamageType = (int)_GameController.CurrentWeapon.DamageType;
                        tempPrefab.GetComponent<Rigidbody2D>().velocity = new Vector2(3 * dir.x, 0);
                        Destroy(tempPrefab, 1);     
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

        public void ChangeWeapon(string id)
        {
            var weapon = _GameController.WeaponProvider.GetWeaponById(id);
            _GameController.CurrentWeapon = weapon;

            

            switch (weapon.WeaponType)
            {
                case WeaponType.Melee:

                    for (int i = 0; i < weapons.Length; i++)
                    {
                        if(weapon.AnimationIcons.Length > i)
                        {
                            weapons[i].GetComponent<SpriteRenderer>().sprite = weapon.AnimationIcons[i];
                        }
                        var weaponData = weapons[i].GetComponent<WeaponData>();

                        weaponData.Damage = weapon.Damage;
                        weaponData.DamageType = (int)weapon.DamageType;
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

        public void ChangeWeapon(CustomWeaponData weapon)
        {

            switch (weapon.WeaponType)
            {
                case WeaponType.Melee:

                    for (int i = 0; i < weapons.Length; i++)
                    {
                        if (weapon.AnimationIcons.Length > i)
                        {
                            weapons[i].GetComponent<SpriteRenderer>().sprite = weapon.AnimationIcons[i];
                        }

                        var weaponData = weapons[i].GetComponent<WeaponData>();

                        weaponData.Damage = weapon.Damage;
                        weaponData.DamageType = (int)weapon.DamageType;
                        _GameController.CurrentWeapon = weapon;
                    }

                    break;

                case WeaponType.Bow:

                    for (int i = 0; i < bows.Length; i++)
                    {
                        if (weapon.AnimationIcons.Length > i)
                        {
                            bows[i].GetComponent<SpriteRenderer>().sprite = weapon.AnimationIcons[i];
                        }

                        var weaponData = bows[i].GetComponent<WeaponData>();
                        _GameController.CurrentWeapon = weapon;
                    }

                    break;

                case WeaponType.Staff:

                    for (int i = 0; i < staffs.Length; i++)
                    {
                        if (weapon.AnimationIcons.Length > i)
                        {
                            staffs[i].GetComponent<SpriteRenderer>().sprite = weapon.AnimationIcons[i];
                        }

                        var weaponData = staffs[i].GetComponent<WeaponData>();
                        _GameController.CurrentWeapon = weapon;
                    }

                    break;

            }
        }

        public void DeActivetWeaponObjects()
        {
            var weapon = _GameController.CurrentWeapon;

            switch (weapon.WeaponType)
            {
                case WeaponType.Melee:

                    for (int i = 0; i < weapons.Length; i++)
                    {
                        weapons[i].SetActive(false);
                    }

                    break;

                case WeaponType.Bow:

                    for (int i = 0; i < bows.Length; i++)
                    {
                        bows[i].SetActive(false);
                    }

                    break;

                case WeaponType.Staff:

                    for (int i = 0; i < staffs.Length; i++)
                    {
                        staffs[i].SetActive(false);
                    }

                    break;

            }
        }

    }
}
