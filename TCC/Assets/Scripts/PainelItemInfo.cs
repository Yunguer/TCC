using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ProjetoTCC
{
    public class PainelItemInfo : MonoBehaviour
    {
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
        [SerializeField]
        private GameObject slotObject;
        public GameObject SlotObject
        {
            get
            {
                return slotObject;
            }
            set
            {
                slotObject = value;
            }
        }
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
        [SerializeField]
        private Button equipBTN;
        [SerializeField]
        private Button deleteBTN;
        [SerializeField]
        private string weaponID;
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
            Item itemInfo = slotObject.GetComponent<Item>();
            weaponID = itemInfo.ItemID;

            var weapon = _GameController.WeaponProvider.GetWeaponById(weaponID);

            itemIMG.sprite = weapon.InventoryIcon;
            itemName.text = weapon.name;

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
        }

        public void UpgradeButton()
        {
            _GameController.UpgradeWeapon(weaponID, slotID);
            LoadUpgrade();
        }

        public void EquipButton()
        {
            slotObject.SendMessage("UseItem", SendMessageOptions.DontRequireReceiver);
            inventory.ClearLoadedItens();
            _GameController.SwapItensInventory(slotID);
        }

        public void DeleteButton()
        {
            _GameController.DeleteItem(slotID);
        }

        void LoadUpgrade()
        {
            int currentLevel = _GameController.WeaponProvider.GetWeaponById(weaponID).Level;

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
