using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ProjetoTCC
{
    public enum GameState
    {
        PAUSE,
        GAMEPLAY,
        ITENS,
        ITEM_INFO
    }

    public class _GameController : MonoBehaviour
    {
        [SerializeField]
        private WeaponProvider weaponProvider;
        public WeaponProvider WeaponProvider => weaponProvider;

        private PlayerScript playerScript;
        private Inventory inventory;

        [SerializeField]
        private GameState currentGameState;
        public GameState CurrentGameState => currentGameState;

        [Header("Configuração Para o Combate")]
        #region Variaveis Para As Mecanicas de Dano
        
        [SerializeField]
        private string[] damageTypes;
        public string[] DamageTypes => damageTypes;
        
        [SerializeField]
        private GameObject[] damageFX;
        public GameObject[] DamageFX => damageFX;
        
        [SerializeField]
        private GameObject deathFX;
        public GameObject DeathFX => deathFX;
        #endregion

        [Header("Configuração Para o Controle do Dinheiro")]
        #region Variaveis Para O Dinheiro
        
        [SerializeField]
        private int gold;
        public int Gold
        {
            get
            {
                return gold;
            }
            set
            {
                gold = value;
            }
        }

        [SerializeField]
        private TextMeshProUGUI goldTXT;
        #endregion

        [Header("Informações do Player")]
        #region Variaveis do Player Para Mudança de Cena
        
        [SerializeField]
        private int characterID;
        public int CharacterID => characterID;
        
        [SerializeField]
        private int currentCharacterID;
        public int CurrentCharacterID
        {
            get
            {
                return currentCharacterID;
            }
            set
            {
                currentCharacterID = value;
            }
        }
        
        [SerializeField]
        private int maxLife;
        public int MaxLife => maxLife;
        
        [SerializeField]
        private int currentLife;
        public int CurrentLife
            {
            get
            {
                return currentLife;
            }
            set
            {
                currentLife = value;
            }
        }
        
        [SerializeField]
        private int maxMana;
        public int MaxMana => maxMana;
        
        [SerializeField]
        private int currentMana;
        public int CurrentMana
        {
            get
            {
                return currentMana;
            }
            set
            {
                currentMana = value;
            }
        }
      
        private CustomWeaponData currentWeapon;
        public CustomWeaponData CurrentWeapon
        {
            get
            {
                return currentWeapon;
            }
            set
            {
                currentWeapon = value;
            }
        }
        
        [SerializeField]
        private int[] arrowQnt; // 0 - FLECHA COMUM 1 - FLECHA DE FERRO 2 - FLECHA DE OURO
        public int[] ArrowQnt
        {
            get
            {
                return arrowQnt;
            }
            set
            {
                arrowQnt = value;
            }
        }

        [SerializeField]
        private int equipedArrowID;
        public int EquipedArrowID
        {
            get
            {
                return equipedArrowID;
            }
            set
            {
                equipedArrowID = value;
            }
        }

        [SerializeField]
        private int[] potionQnt; // 0 - POÇÃO DE CURA 1 - POÇÃO DE MANA;
        public int[] PotionQnt
        {
            get
            {
                return potionQnt;
            }
            set
            {
                potionQnt = value;
            }
        }
        #endregion

        [Header("Banco de Personagens")]
        #region Variaveis do Personagem Atual
        
        [SerializeField]
        private string[] characterName;
        
        [SerializeField]
        private Texture[] spriteSheetsName;
        public Texture[] SpriteSheetName => spriteSheetsName;
        
        [SerializeField]
        private int[] characterClassID;
        public int[] CharacterClassID => characterClassID;

        [SerializeField]
        private GameObject[] inicialWeapon;
        public GameObject[] InicialWeapon => inicialWeapon;
        #endregion

        [Header("Banco de Dados de Armas")]
        #region Banco de Dados de Armas
        [SerializeField]
        private string[] weaponName;
        public string[] WeaponName => weaponName;
        [SerializeField]
        private Sprite[] inventoryIMG;
        public Sprite[] InventoryIMG => inventoryIMG;
        [SerializeField]
        private int[] weaponClassID; // 0 = Espada, Machado, Martelo, Adagas, Maça - 1 = Arcos - 2 = Cajados
        public int[] WeaponClassID => weaponClassID;

        [SerializeField]
        private Sprite[] weaponsSprites_1;
        public Sprite[] WeaponsSprites_1 => weaponsSprites_1;
        [SerializeField]
        private Sprite[] weaponsSprites_2;
        public Sprite[] WeaponsSprites_2 => weaponsSprites_2;
        [SerializeField]
        private Sprite[] weaponsSprites_3;
        public Sprite[] WeaponsSprites_3 => weaponsSprites_3;
        [SerializeField]
        private Sprite[] weaponsSprites_4;
        public Sprite[] WeaponsSprites_4 => weaponsSprites_4;
        [SerializeField]
        private int[] weaponDamage;
        public int[] WeaponDamage => weaponDamage;
        [SerializeField]
        private int[] weaponDamageType;
        public int[] WeaponDamageType => weaponDamageType;
        [SerializeField]
        private int[] weaponUpgrade;
        public int[] WeaponUpgrade => weaponUpgrade;
        [SerializeField]
        private Sprite[] arrowIcon;
        public Sprite[] ArrowIcon => arrowIcon;

        [SerializeField]
        private Sprite[] arrowImg;
        public Sprite[] ArrowImg => arrowImg;

        [SerializeField]
        private GameObject[] arrowPrefab;
        public GameObject[] ArrowPrefab => arrowPrefab;
        #endregion

        [Header("Paineis")]
        #region Variaveis do Painel
        private bool pauseState;
        [SerializeField]
        private GameObject pausePainel;
        [SerializeField]
        private GameObject itensPainel;
        [SerializeField]
        private GameObject itensInfoPainel;
        #endregion

        [Header("Navegação dos Paineis")]
        #region Variaveis para a Navegação dos Paineis
        [SerializeField]
        private Button firstPainelPause;
        [SerializeField]
        private Button firstPainelItens;
        [SerializeField]
        private Button firstPainelItenInfo;
        #endregion

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            inventory = FindObjectOfType(typeof(Inventory)) as Inventory;
            playerScript = FindObjectOfType(typeof(PlayerScript)) as PlayerScript;
            pausePainel.SetActive(false);
            itensPainel.SetActive(false);
            itensInfoPainel.SetActive(false);
            characterID = PlayerPrefs.GetInt("titleCharacterID");

            currentWeapon = weaponProvider.GetInicialWeaponByCharacterId((PlayerType)characterID);

            playerScript.ChangeWeapon(currentWeapon);

            inventory.InventoryItens.Add(inicialWeapon[characterID]);

            currentLife = maxLife;
            currentMana = maxMana;
        }

        void Update()
        {
            string s = Gold.ToString("N0");
            goldTXT.text = s.Replace(",",".");

            if(Input.GetButtonDown("Cancel") && currentGameState != GameState.ITENS && currentGameState != GameState.ITEM_INFO)
            {
                PauseGame();
            }
            else if(Input.GetButtonDown("Cancel") && currentGameState == GameState.ITENS)
            {
                ClosePainel();
            }
            else if (Input.GetButtonDown("Cancel") && currentGameState == GameState.ITEM_INFO)
            {
                CloseItemInfo();
            }
        }

        public void ValidateWeapon()
        {
            var weapon = WeaponProvider.GetWeaponById(currentWeapon.Id);
            if ((int)weapon.WeaponType != characterClassID[characterID])
            {
                currentWeapon = weaponProvider.GetInicialWeaponByCharacterId((PlayerType)characterID);
            }
        }

        public void PauseGame()
        {
            pauseState = pausePainel.activeSelf;
            pauseState = !pauseState;
            pausePainel.SetActive(pauseState);
            
            switch(pauseState)
            {
                case true:

                    Time.timeScale = 0;
                    ChangeState(GameState.PAUSE);
                    firstPainelPause.Select();
                    break;

                case false:

                    Time.timeScale = 1;
                    ChangeState(GameState.GAMEPLAY);
                    break;
            }
        }

        public void ChangeState(GameState newState)
        {
            currentGameState = newState;
            if(newState == GameState.GAMEPLAY)
            {
                Time.timeScale = 1;
            }
        }

        public void ButtonItensDown()
        {
            pausePainel.SetActive(false);
            itensPainel.SetActive(true);
            firstPainelItens.Select();
            inventory.LoadInventory();
            ChangeState(GameState.ITENS);
        }

        public void ClosePainel()
        {
            itensPainel.SetActive(false);
            pausePainel.SetActive(true);
            firstPainelPause.Select();

            inventory.ClearLoadedItens();

            ChangeState(GameState.PAUSE);
        }

        public void UseItemWeapon(string weaponID)
        {
            playerScript.ChangeWeapon(weaponID);
        }

        public void OpenItenInfo()
        {
            itensInfoPainel.SetActive(true);
            firstPainelItenInfo.Select();
            ChangeState(GameState.ITEM_INFO);
        }

        public void CloseItemInfo()
        {
            itensInfoPainel.SetActive(false);
            ChangeState(GameState.ITENS);
        }

        public void ReturnGameplay()
        {
            itensPainel.SetActive(false);
            pausePainel.SetActive(false);
            itensInfoPainel.SetActive(false);
            ChangeState(GameState.GAMEPLAY);
        }

        public void DeleteItem(int slotID)
        {
            inventory.InventoryItens.RemoveAt(slotID);
            inventory.LoadInventory();
            itensInfoPainel.SetActive(false);
            firstPainelItens.Select();
        }

        public void UpgradeWeapon(string weaponID, int slotID)
        {
            var weapon = weaponProvider.GetWeaponById(weaponID);
            var newWeaponId = $"{weaponID.Substring(0, weaponID.Length - 2)}_{weapon.Level+1}";
            inventory.InventoryItens[slotID].GetComponent<Item>().ItemID = newWeaponId;
        }

        public void SwapItensInventory(int slotID)
        {
            GameObject Item1 = inventory.InventoryItens[0];
            GameObject Item2 = inventory.InventoryItens[slotID];

            inventory.InventoryItens[0] = Item2;
            inventory.InventoryItens[slotID] = Item1;

            ReturnGameplay();
        }

        public void ColectItem(GameObject colectedObject)
        {
            inventory.InventoryItens.Add(colectedObject);
        }

        public void UsePotion(int potionID)
        { 
            if(potionQnt[potionID] > 0)
            {
                PotionQnt[potionID] -= 1;
                switch (potionID)
                {
                    case 0:
                        ; //VIDA

                        currentLife += 3;
                        if (currentLife > maxLife)
                        {
                            currentLife = maxLife;
                        }

                        break;
                    case 1: //MANA

                        currentMana += 3;
                        if(currentMana > maxMana)
                        {
                            currentMana = maxMana;
                        }

                        break;
                }
            }
        }
    }
}