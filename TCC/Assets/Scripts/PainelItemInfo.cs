using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ProjetoTCC
{
    public class PainelItemInfo : MonoBehaviour
    {
        public static System.Action OnItemClicked;

        private _GameController _GameController;
        private Inventory inventory;

        [Header("Configuração dos Slots do Inventario")]
        #region Variaveis para informação do Slot
        [SerializeField]
        private int slotID;
        public int SlotID
        {
            get
            {
                return slotID;
            }
            set
            {
                slotID = value;
            }
        }

        private string itemID;
        public string ItemID => itemID;
        #endregion

        [Header("Configuração da HUD do Item")]
        #region Variaveis da HUD dos Itens
        [SerializeField]
        private Image itemIMG;
        [SerializeField]
        private TMP_Text itemName;
        [SerializeField]
        private TMP_Text damageWeapon;
        [SerializeField]
        private GameObject[] upgrades;
        #endregion

        [Header("Configuração dos Botões")]
        #region Variaveis para os Botões
        [SerializeField]
        private Button upgradeBTN;
        public Button UpgradeBTN => upgradeBTN;
        [SerializeField]
        private Button equipBTN;
        [SerializeField]
        private Button deleteBTN;
        [SerializeField]
        private int upgrade;
        #endregion

        void Start()
        {
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
            inventory = FindObjectOfType(typeof(Inventory)) as Inventory;
        }

        void Update()
        {
            
        }

        public void LoadItemInfo()
        {
            itemID = inventory.InventoryItens[slotID];
            var weapon = _GameController.WeaponProvider.GetWeaponById(itemID);

            itemIMG.sprite = weapon.InventoryIcon;
            itemName.text = weapon.Name;

            string damageType = weapon.DamageType.ToString();
            int damage = weapon.Damage;

            damageWeapon.text = "Dano: " + damage.ToString() + " de " + damageType;

            LoadUpgrade();

            if(slotID == 0)
            {
                equipBTN.interactable = false;
                deleteBTN.interactable = false;
            }
            else
            {
                int weaponClassID = (int)weapon.WeaponType;
                int characterClassID = _GameController.CharacterClassID[_GameController.CharacterID];

                if(weaponClassID == characterClassID)
                {
                    equipBTN.interactable = true;
                }
                else
                {
                    equipBTN.interactable = false;
                }

                deleteBTN.interactable = true;
            }
            if (_GameController.Gold < 15)
            {
                upgradeBTN.interactable = false;
            }
            else if (_GameController.Gold >= 15)
            {
                upgradeBTN.interactable = true;
            }
            if (weapon.Level == 10)
            {
                upgradeBTN.interactable = false;
            }  
        }

        public void UpgradeButton()
        {
            _GameController.UpgradeWeapon(itemID, slotID);
            LoadItemInfo();
        }

        public void EquipButton()
        {
            inventory.ClearLoadedItens();
            _GameController.SwapItensInventory(slotID);
            _GameController.UseItemWeapon(itemID);
        }

        public void DeleteButton()
        {
            _GameController.DeleteItem(slotID);
        }

        void LoadUpgrade()
        {
            itemID = inventory.InventoryItens[slotID];
            int currentLevel = _GameController.WeaponProvider.GetWeaponById(itemID).Level;

            SetupUpgradeButton(currentLevel);

            foreach (GameObject a in upgrades)
            {
                a.SetActive(false);
            }

            for (int i = 0; i < currentLevel; i++)
            {
                upgrades[i].SetActive(true);
            }
        }

        private void SetupUpgradeButton(int currentLevel)
        {
            upgradeBTN.interactable = currentLevel < 10;
        }


    }
}
