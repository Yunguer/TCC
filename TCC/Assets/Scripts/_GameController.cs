using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using ProjetoTCC.OdinSerializer;

namespace ProjetoTCC
{
    public enum GameState
    {
        PAUSE,
        GAMEPLAY,
        ITENS,
        ITEM_INFO,
        DIALOGO,
        FIM_DIALOGO,
        START,
        DEATH,
        COMPLETED
    }

    public class _GameController : MonoBehaviour
    {
        [SerializeField]
        private WeaponProvider weaponProvider;
        public WeaponProvider WeaponProvider => weaponProvider;

        private PlayerScript playerScript;
        private Inventory inventory;
        private PainelItemInfo painelItemInfo;

        [SerializeField]
        private GameState currentGameState;
        public GameState CurrentGameState => currentGameState;

        [Header("Configura��o Para o Combate")]
        #region Variaveis Para As Mecanicas de Dano
        
        [SerializeField]
        private GameObject[] damageFX;
        public GameObject[] DamageFX => damageFX;
        
        [SerializeField]
        private GameObject deathFX;
        public GameObject DeathFX => deathFX;
        #endregion

        [Header("Configura��o Para o Controle do Dinheiro")]
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

        [Header("Informa��es do Player")]
        #region Variaveis do Player Para Mudan�a de Cena
        
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
      
        [SerializeField]
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

        private string currentWeaponId;
        public string CurrentWeaponId => currentWeaponId;

        
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
        private int[] potionQnt; // 0 - PO��O DE CURA 1 - PO��O DE MANA;
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

        #endregion

        [Header("Banco de Dados de Armas")]
        #region Banco de Dados de Armas
     
        [SerializeField]
        private Sprite[] arrowIcon;
        public Sprite[] ArrowIcon => arrowIcon;

        [SerializeField]
        private Sprite[] arrowImg;
        public Sprite[] ArrowImg => arrowImg;

        [SerializeField]
        private GameObject[] arrowPrefab;
        public GameObject[] ArrowPrefab => arrowPrefab;

        private List<string> inventoryItens;
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

        [Header("Navega��o dos Paineis")]
        #region Variaveis para a Navega��o dos Paineis
        [SerializeField]
        private Button firstPainelPause;
        [SerializeField]
        private Button firstPainelItens;
        [SerializeField]
        private Button firstPainelItenInfo;
        [SerializeField]
        private GameObject startPainel;
        [SerializeField]
        private GameObject completeGamePainel;
        public GameObject CompleteGamePainel => completeGamePainel;
        #endregion

        [Header("Miss�es")]
        #region Variaveis para as Miss�es

        private bool mission1 = false;
        public bool Mission1
        {
            get
            {
                return mission1;
            }
            set
            {
                mission1 = value;
            }
        }
        private int mission1Count = 0;
        public int Mission1Count
        {
            get
            {
                return mission1Count;
            }
            set
            {
                mission1Count = value;
            }
        }

        [SerializeField]
        private GameObject deathPainel;
        #endregion

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            inventory = FindObjectOfType(typeof(Inventory)) as Inventory;
            pausePainel.SetActive(false);
            itensPainel.SetActive(false);
            itensInfoPainel.SetActive(false);
            painelItemInfo = FindObjectOfType(typeof(PainelItemInfo)) as PainelItemInfo;


            var hasLoad = Load(PlayerPrefs.GetString("slot"));
            if(!hasLoad)
            {
                NewGame();
            }

            currentWeapon = weaponProvider.GetWeaponById(currentWeaponId);
        }

        void Update()
        {
            string s = Gold.ToString("N0");
            goldTXT.text = s.Replace(",",".");


            
            if(Input.anyKeyDown)
            {

                if (Input.GetKeyDown(KeyCode.Escape) && currentGameState != GameState.ITENS && currentGameState != GameState.ITEM_INFO && currentGameState != GameState.START && currentGameState != GameState.DEATH && currentGameState != GameState.COMPLETED)
                {
                    PauseGame();
                }
                else if (Input.GetKeyDown(KeyCode.Escape) && currentGameState == GameState.ITENS)
                {
                    ClosePainel();
                }
                else if (Input.GetKeyDown(KeyCode.Escape) && currentGameState == GameState.ITEM_INFO)
                {
                    CloseItemInfo();
                }

                if ((Input.GetKeyDown(KeyCode.Escape) && currentGameState == GameState.START))
                {
                    startPainel.SetActive(false);
                    ChangeState(GameState.GAMEPLAY);
                }
            }

            if (currentLife <= 0)
            {
                StartCoroutine(nameof(Death));   
            }

            
        }

        public void ValidateWeapon()
        {
            if(currentWeapon == null)
            {
                currentWeapon = weaponProvider.GetInicialWeaponByCharacterId((PlayerType)characterID);
                return;
            }
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
            if (newState == GameState.FIM_DIALOGO)
            {
                StartCoroutine(nameof(DialogueEnd));
            }
            if(newState == GameState.DEATH)
            {
                Time.timeScale = 0;
            }
            if (newState == GameState.COMPLETED)
            {
                Time.timeScale = 0;
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
            if(playerScript == null)
            {
                playerScript = FindObjectOfType(typeof(PlayerScript)) as PlayerScript;
            }
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
            inventory.ClearSlot(slotID);
            itensInfoPainel.SetActive(false);
            firstPainelItens.Select();
        }

        public void UpgradeWeapon(string weaponID, int slotID)
        {
            var weapon = weaponProvider.GetWeaponById(weaponID);
            var newWeaponId = $"{weaponID.Substring(0, weaponID.Length - 2)}_{weapon.Level+1}";
            var newWeapon = weaponProvider.GetWeaponById(newWeaponId);

            if (playerScript == null)
            {
                playerScript = FindObjectOfType(typeof(PlayerScript)) as PlayerScript;
            }

            if (weapon.Level < 10)
            {
                inventory.UpdateSlot(newWeaponId, slotID);
                if (slotID == 0)
                {
                    playerScript.ChangeWeapon(newWeapon);
                }
                gold -= 15;
            }
            if(weapon.Level == 9)
            {
                painelItemInfo.UpgradeBTN.interactable = false;
            }
        }

        public void SwapItensInventory(int slotID)
        {
            var Item1 = inventory.InventoryItens[0];
            var Item2 = inventory.InventoryItens[slotID];

            inventory.InventoryItens[0] = Item2;
            inventory.InventoryItens[slotID] = Item1;

            ReturnGameplay();
        }

        public void ColectItem(string colectedObject)
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

        IEnumerator DialogueEnd()
        {
            yield return new WaitForEndOfFrame();
            ChangeState(GameState.GAMEPLAY);
        }

        public void Save()
        {
            string saveFileName = PlayerPrefs.GetString("slot");

            PlayerData data = new PlayerData();
            data.gold = gold;
            data.characterId = PlayerPrefs.GetInt("titleCharacterID");
            data.currentWeaponId = currentWeapon.Id;
            data.equipedArrow = equipedArrowID;
            data.arrowQnt = arrowQnt;
            data.potionQnt = potionQnt;

            if (inventoryItens != null)
            {
                inventoryItens.Clear();
            }
            else
            {
                inventoryItens = new List<string>();
            }

            foreach (string i in inventory.InventoryItens)
            {
                inventoryItens.Add(i);
            }

            data.inventoryItens = inventoryItens;

            byte[] bytes = SerializationUtility.SerializeValue(data, DataFormat.Binary);
            File.WriteAllBytes(Application.persistentDataPath + "/" + saveFileName, bytes);
        }

        public bool Load(string slot)
        {
            if (File.Exists(Application.persistentDataPath + "/" + slot))
            {

                byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/" + slot);
                var data = SerializationUtility.DeserializeValue<PlayerData>(bytes, DataFormat.Binary);

                PlayerPrefs.SetInt("titleCharacterID", data.characterId);

                gold = data.gold;
                characterID = data.characterId;
                currentWeaponId = data.currentWeaponId;
                equipedArrowID = data.equipedArrow;
                arrowQnt = data.arrowQnt;
                potionQnt = data.potionQnt;
                inventoryItens = data.inventoryItens;
                currentLife = maxLife;
                currentMana = maxMana;


                if (inventory.InventoryItens != null)
                {
                    inventory.InventoryItens.Clear();
                }
                else
                {
                    inventory.InventoryItens = new List<string>();
                }


                if (inventoryItens != null)
                {
                    foreach (string i in inventoryItens)
                    {
                        inventory.InventoryItens.Add(i);
                    }
                }

                SceneManager.LoadScene("Cena_1");
                return true;
            }
            else
            {
                return false;
            }
        }

        public void NewGame()
        {
            characterID = PlayerPrefs.GetInt("titleCharacterID");

            currentWeapon = weaponProvider.GetInicialWeaponByCharacterId((PlayerType)characterID);
            currentWeaponId = currentWeapon.Id;

            currentLife = maxLife;
            currentMana = maxMana;

            gold = 0;

            arrowQnt = new int[3];

            arrowQnt[0] = 25;
            arrowQnt[1] = 0;
            arrowQnt[2] = 0;

            potionQnt = new int[2];

            potionQnt[0] = 10;
            potionQnt[1] = 10;

            inventory.InventoryItens.Add(currentWeapon.Id);

            equipedArrowID = 0;

            startPainel.SetActive(true);
           

            Save();
            Load(PlayerPrefs.GetString("slot"));
            ChangeState(GameState.START);
        }

        IEnumerator Death()
        {
            yield return new WaitForSeconds(2.5f);
            ChangeState(GameState.DEATH);
            deathPainel.SetActive(true);
        }

        public void BackToMenu()
        {
            SceneManager.LoadScene("Titulo");
            ChangeState(GameState.GAMEPLAY);
            Destroy(this.gameObject);
        }
        
    }

    [Serializable]
    class PlayerData
    {
        public int gold;
        public int characterId;
        public string currentWeaponId;
        public int equipedArrow;
        public int[] arrowQnt;
        public int[] potionQnt;
        public List<string> inventoryItens;
        
    }
}