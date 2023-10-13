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
        #endregion

        void Start()
        {
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
        }

        void Update()
        {

        }

        public void LoadItemInfo()
        {
            Item itemInfo = slotObject.GetComponent<Item>();
            int weaponID = itemInfo.ItemID;
                

            itemIMG.sprite = _GameController.InventoryIMG[weaponID];
            itemName.text = _GameController.WeaponName[weaponID];

            string damageType = _GameController.DamageTypes[_GameController.WeaponDamageType[weaponID]];
            int damage = _GameController.WeaponDamage[weaponID];

            damageWeapon.text = "Dano: " + damage.ToString() + " de " + damageType;

            int upgrade = _GameController.WeaponUpgrade[weaponID];
            
            foreach(GameObject a in upgrades)
            {
                a.SetActive(false);
            }

            for(int i = 0; i < upgrade; i++)
            {
                upgrades[i].SetActive(true);
            }
        }

        public void UpgradeButton()
        {

        }

        public void EquipButton()
        {
            slotObject.SendMessage("UseItem", SendMessageOptions.DontRequireReceiver);
            _GameController.ReturnGameplay();
        }

        public void DeleteButton()
        {

        }
    }
}
