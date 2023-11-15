using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProjetoTCC
{
    public class Inventory : MonoBehaviour
    {
        private _GameController _GameController;

        [Header("Inventario")]
        #region Variaveis do Inventario
        
        [SerializeField]
        private Button[] slots;
        
        [SerializeField]
        private TextMeshProUGUI qntHealth, qntMana, qntArrow1, qntArrow2, qntArrow3;
        
        [SerializeField]
        private int qHealth, qMana, qArrow1, qArrow2, qArrow3;
        
        [SerializeField]
        private List<string> inventoryItens;
        public List<string> InventoryItens
        {
            get
            {
                return inventoryItens;
            }
            set
            {
                inventoryItens = value;
            }
        }

        [SerializeField]
        private List<string> loadedItens;
        public List<string> LoadedItens
        {
            get
            {
                return loadedItens;
            }
            set
            {
                loadedItens = value;
            }
        }
        #endregion

        void Start()
        {
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
            
            if(loadedItens == null)
            {
                loadedItens = new List<string>();
            }
            if (inventoryItens == null)
            {
                loadedItens = new List<string>();
            }
        }    

        void Update()
        {

        }

        public void LoadInventory()
        {
            ClearLoadedItens();

            foreach (Button b in slots)
            {
                b.interactable = false;
            }

            qntHealth.text = "x " + _GameController.PotionQnt[0].ToString();
            qntMana.text = "x " + _GameController.PotionQnt[1].ToString();
            qntArrow1.text = "x " + _GameController.ArrowQnt[0].ToString();
            qntArrow2.text = "x " + _GameController.ArrowQnt[1].ToString();
            qntArrow3.text = "x " + _GameController.ArrowQnt[2].ToString();

            int s = 0;

            foreach(string i in inventoryItens)
            {
                var weapon = _GameController.WeaponProvider.GetWeaponById(i);

                loadedItens.Add(i);
                slots[s].GetComponent<SlotInventory>().SetIcon(weapon.InventoryIcon);
                slots[s].interactable = true;

                s++;
            }
        }

        public void ClearLoadedItens()
        {
            loadedItens.Clear();
        }

        public void ClearSlot(int slotID)
        {
            InventoryItens.RemoveAt(slotID);

            slots[slotID].GetComponent<SlotInventory>().SetEnableIcon(false);

            LoadInventory();
        }

        public void UpdateSlot(string itemID, int slotID)
        {
            InventoryItens[slotID] = itemID;
            LoadInventory();
            
        }
    }
}
