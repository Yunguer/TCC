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
        private int maxMana;
        public int MaxMana => maxMana;
        [SerializeField]
        private int weaponID, currentWeaponID;
        public int WeaponID
        {
            get
            {
                return weaponID;
            }
            set
            {
                weaponID = value;
            }
        }
        public int CurrentWeaponID
        {
            get
            {
                return currentWeaponID;
            }
            set
            {
                currentWeaponID = value;
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
        private int inicialWeaponID;
        public int InicialWeaponID => inicialWeaponID;
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

            inventory.InventoryItens.Add(inicialWeapon[characterID]);

            GameObject weaponTemp = Instantiate(inicialWeapon[characterID]);

            inventory.LoadedItens.Add(weaponTemp);

            inicialWeaponID = weaponTemp.GetComponent<Item>().ItemID;

            inventory.ClearLoadedItens();
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
            if(weaponClassID[weaponID] != characterClassID[characterID])
            {
                weaponID = inicialWeaponID;
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

        public void UseItemWeapon(int weaponID)
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

        public void UpgradeWeapon(int weaponID)
        {
            int up = weaponUpgrade[weaponID];
            if(up < 10)
            {
                up += 1;
                weaponUpgrade[weaponID] = up;
            }
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
    }
}