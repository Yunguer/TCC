using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ProjetoTCC
{
    public enum GameState
    {
        PAUSE,
        GAMEPLAY,
        ITENS
    }

    public class _GameController : MonoBehaviour
    {
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
        private int[] inicialWeaponID;
        public int[] InicialWeaponID => inicialWeaponID;


        #endregion

        [Header("Banco de Dados de Armas")]
        #region Banco de Dados de Armas
        [SerializeField]
        private string[] weaponName;
        [SerializeField]
        private Sprite[] inventoryIMG;
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
        #endregion

        [Header("Paineis")]
        #region Variaveis do Painel
        [SerializeField]
        private GameObject pausePainel;
        private bool pauseState;
        [SerializeField]
        private GameObject itensPainel;
        #endregion

        [Header("Navegação dos Paineis")]
        #region Variaveis para a Navegação dos Paineis
        [SerializeField]
        private Button firstPainelPause;
        [SerializeField]
        private Button firstPainelItens;
        #endregion

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            inventory = FindObjectOfType(typeof(Inventory)) as Inventory;
            pausePainel.SetActive(false);
            itensPainel.SetActive(false);
            characterID = PlayerPrefs.GetInt("titleCharacterID");
        }

        void Update()
        {
            string s = Gold.ToString("N0");
            goldTXT.text = s.Replace(",",".");

            if(Input.GetButtonDown("Cancel") && currentGameState != GameState.ITENS)
            {
                PauseGame();
            }
            else if(Input.GetButtonDown("Cancel") && currentGameState == GameState.ITENS)
            {
                ClosePainel();
            }
        }

        public void ValidateWeapon()
        {
            if(weaponClassID[weaponID] != characterClassID[characterID])
            {
                weaponID = inicialWeaponID[characterID];
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
    }
}