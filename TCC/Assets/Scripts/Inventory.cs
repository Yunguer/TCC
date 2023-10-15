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
        private Image[] itensIcons;
        [SerializeField]
        private TextMeshProUGUI qntHealth, qntMana, qntArrow1, qntArrow2, qntArrow3;
        [SerializeField]
        private int qHealth, qMana, qArrow1, qArrow2, qArrow3;
        [SerializeField]
        private List<GameObject> inventoryItens;
        public List<GameObject> InventoryItens
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
        private List<GameObject> loadedItens;
        public List<GameObject> LoadedItens
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
                loadedItens = new List<GameObject>();
            }
            if (inventoryItens == null)
            {
                loadedItens = new List<GameObject>();
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

            foreach(Image i in itensIcons)
            {
                i.sprite = null;
                i.gameObject.SetActive(false);
            }

            qntHealth.text = "x 0";
            qntMana.text = "x 0";
            qntArrow1.text = "x " + _GameController.ArrowQnt[0];
            qntArrow2.text = "x " + _GameController.ArrowQnt[1];
            qntArrow3.text = "x " + _GameController.ArrowQnt[2];

            int s = 0;

            foreach(GameObject i in inventoryItens)
            {
                GameObject temp = Instantiate(i);
                Item itemInfo = temp.GetComponent<Item>();

                loadedItens.Add(temp);

                slots[s].GetComponent<SlotInventory>().SlotObject = temp;
                slots[s].interactable = true;

                itensIcons[s].sprite = _GameController.InventoryIMG[itemInfo.ItemID];
                itensIcons[s].gameObject.SetActive(true);

                s++;
            }
        }

        public void ClearLoadedItens()
        {
            foreach (GameObject li in loadedItens)
            {
                Destroy(li);
            }

            loadedItens.Clear();
        }
    }
}
