using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProjetoTCC
{
    public class Hud : MonoBehaviour
    {
        #region Comunicação Entre Scripts
        private PlayerScript playerScript;
        private _GameController _GameController;
        #endregion

        [Header("Configuração Para a HUD")]
        #region Variaveis para a HUD
        
        [SerializeField]
        private Image[] hpBar;
        
        [SerializeField]
        private Image[] mpBar;
        
        [SerializeField]
        private GameObject mpHUD;
        
        [SerializeField]
        private GameObject arrowHUD;
        
        [SerializeField]
        private TMP_Text arrowQnt;
        
        [SerializeField]
        private Image arrowIcon;
        
        [SerializeField]
        private TMP_Text healthPotionQnt, manaPotionQnt;
        
        [SerializeField]
        private GameObject manaBox, healthBox;
        
        [SerializeField]
        private RectTransform box1, box2;

        [SerializeField]
        private Vector2 pos1, pos2;

        #endregion

        void Start()
        {
            playerScript = FindObjectOfType(typeof(PlayerScript)) as PlayerScript;
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;

            mpHUD.SetActive(false);
            arrowHUD.SetActive(false);
            manaBox.SetActive(false);
            healthBox.SetActive(false);
            StartCoroutine(nameof(CheckClassBar));

            pos1 = box1.anchoredPosition;
            pos2 = box2.anchoredPosition;
        }


        void Update()
        {
            LifeBarController();
            if (mpHUD.activeSelf == true)
            {
                ManaBarController();
                manaPotionQnt.text = _GameController.PotionQnt[1].ToString();
            }

            BoxPotionPosition();

            if (arrowHUD.activeSelf == true)
            {
                if(Input.GetButtonDown("ButtonLeft") && playerScript.IsAttacking == false)
                {
                    if(_GameController.EquipedArrowID == 0)
                    {
                        _GameController.EquipedArrowID = _GameController.ArrowIcon.Length - 1;
                    }
                    else
                    {
                        _GameController.EquipedArrowID -= 1;
                    }
                }
                else if(Input.GetButtonDown("ButtonRight") && playerScript.IsAttacking == false )
                {
                    if (_GameController.EquipedArrowID == _GameController.ArrowIcon.Length - 1)
                    {
                        _GameController.EquipedArrowID = 0;
                    }
                    else
                    {
                        _GameController.EquipedArrowID += 1;
                    }
                }
                arrowIcon.sprite = _GameController.ArrowIcon[_GameController.EquipedArrowID];
                arrowQnt.text = "x " + _GameController.ArrowQnt[_GameController.EquipedArrowID].ToString();
            }
            healthPotionQnt.text = _GameController.PotionQnt[0].ToString();
        }

        void BoxPotionPosition()
        { 
            if(_GameController.PotionQnt[0] > 0)
            {
                healthBox.GetComponent<RectTransform>().anchoredPosition = pos1;
                manaBox.GetComponent<RectTransform>().anchoredPosition = pos2;
            }
            else
            {
                healthBox.GetComponent<RectTransform>().anchoredPosition = pos2;
                manaBox.GetComponent<RectTransform>().anchoredPosition = pos1;
            }
        }

        void LifeBarController()
        {

            float lifePercentage = (float)_GameController.CurrentLife / (float)_GameController.MaxLife;

            if (Input.GetButtonDown("ItemA") && lifePercentage < 1)
            {
                _GameController.UsePotion(0); // USA POÇÃO DE CURA
            }

            foreach (Image img in hpBar)
            {
                img.enabled = true;
            }

            if(lifePercentage >= 1)
            {

            }
            else if(lifePercentage >= 0.9f)
            {
                hpBar[9].enabled = false;
            }
            else if (lifePercentage >= 0.8f)
            {
                hpBar[9].enabled = false;
                hpBar[8].enabled = false;
            }
            else if (lifePercentage >= 0.7f)
            {
                hpBar[9].enabled = false;
                hpBar[8].enabled = false;
                hpBar[7].enabled = false;
            }
            else if (lifePercentage >= 0.6f)
            {
                hpBar[9].enabled = false;
                hpBar[8].enabled = false;
                hpBar[7].enabled = false;
                hpBar[6].enabled = false;
            }
            else if (lifePercentage >= 0.5f)
            {
                hpBar[9].enabled = false;
                hpBar[8].enabled = false;
                hpBar[7].enabled = false;
                hpBar[6].enabled = false;
                hpBar[5].enabled = false;
            }
            else if (lifePercentage >= 0.4f)
            {
                hpBar[9].enabled = false;
                hpBar[8].enabled = false;
                hpBar[7].enabled = false;
                hpBar[6].enabled = false;
                hpBar[5].enabled = false;
                hpBar[4].enabled = false;
            }
            else if (lifePercentage >= 0.3f)
            {
                hpBar[9].enabled = false;
                hpBar[8].enabled = false;
                hpBar[7].enabled = false;
                hpBar[6].enabled = false;
                hpBar[5].enabled = false;
                hpBar[4].enabled = false;
                hpBar[3].enabled = false;
            }
            else if (lifePercentage >= 0.2f)
            {
                hpBar[9].enabled = false;
                hpBar[8].enabled = false;
                hpBar[7].enabled = false;
                hpBar[6].enabled = false;
                hpBar[5].enabled = false;
                hpBar[4].enabled = false;
                hpBar[3].enabled = false;
                hpBar[2].enabled = false;
            }
            else if (lifePercentage >= 0.1f)
            {
                hpBar[9].enabled = false;
                hpBar[8].enabled = false;
                hpBar[7].enabled = false;
                hpBar[6].enabled = false;
                hpBar[5].enabled = false;
                hpBar[4].enabled = false;
                hpBar[3].enabled = false;
                hpBar[2].enabled = false;
                hpBar[1].enabled = false;
            }
            else if (lifePercentage >= 0.0f)
            {
                hpBar[9].enabled = false;
                hpBar[8].enabled = false;
                hpBar[7].enabled = false;
                hpBar[6].enabled = false;
                hpBar[5].enabled = false;
                hpBar[4].enabled = false;
                hpBar[3].enabled = false;
                hpBar[2].enabled = false;
                hpBar[1].enabled = false;
                hpBar[0].enabled = false;
            }

            if(_GameController.PotionQnt[0] > 0)
            {
                healthBox.SetActive(true);
            }
            else
            {
                healthBox.SetActive(false);
            }

        }
        void ManaBarController()
        {

            float manaPercentage = (float)_GameController.CurrentMana / (float)_GameController.MaxMana;

            if (Input.GetButtonDown("ItemB") && manaPercentage < 1)
            {
                _GameController.UsePotion(1); // USA POÇÃO DE MANA
            }

            foreach (Image img in mpBar)
            {
                img.enabled = true;
            }

            if (manaPercentage >= 1)
            {

            }
            else if (manaPercentage >= 0.9f)
            {
                mpBar[9].enabled = false;
            }
            else if (manaPercentage >= 0.8f)
            {
                mpBar[9].enabled = false;
                mpBar[8].enabled = false;
            }
            else if (manaPercentage >= 0.7f)
            {
                mpBar[9].enabled = false;
                mpBar[8].enabled = false;
                mpBar[7].enabled = false;
            }
            else if (manaPercentage >= 0.6f)
            {
                mpBar[9].enabled = false;
                mpBar[8].enabled = false;
                mpBar[7].enabled = false;
                mpBar[6].enabled = false;
            }
            else if (manaPercentage >= 0.5f)
            {
                mpBar[9].enabled = false;
                mpBar[8].enabled = false;
                mpBar[7].enabled = false;
                mpBar[6].enabled = false;
                mpBar[5].enabled = false;
            }
            else if (manaPercentage >= 0.4f)
            {
                mpBar[9].enabled = false;
                mpBar[8].enabled = false;
                mpBar[7].enabled = false;
                mpBar[6].enabled = false;
                mpBar[5].enabled = false;
                mpBar[4].enabled = false;
            }
            else if (manaPercentage >= 0.3f)
            {
                mpBar[9].enabled = false;
                mpBar[8].enabled = false;
                mpBar[7].enabled = false;
                mpBar[6].enabled = false;
                mpBar[5].enabled = false;
                mpBar[4].enabled = false;
                mpBar[3].enabled = false;
            }
            else if (manaPercentage >= 0.2f)
            {
                mpBar[9].enabled = false;
                mpBar[8].enabled = false;
                mpBar[7].enabled = false;
                mpBar[6].enabled = false;
                mpBar[5].enabled = false;
                mpBar[4].enabled = false;
                mpBar[3].enabled = false;
                mpBar[2].enabled = false;
            }
            else if (manaPercentage >= 0.1f)
            {
                mpBar[9].enabled = false;
                mpBar[8].enabled = false;
                mpBar[7].enabled = false;
                mpBar[6].enabled = false;
                mpBar[5].enabled = false;
                mpBar[4].enabled = false;
                mpBar[3].enabled = false;
                mpBar[2].enabled = false;
                mpBar[1].enabled = false;
            }
            else if (manaPercentage >= 0.0f)
            {
                mpBar[9].enabled = false;
                mpBar[8].enabled = false;
                mpBar[7].enabled = false;
                mpBar[6].enabled = false;
                mpBar[5].enabled = false;
                mpBar[4].enabled = false;
                mpBar[3].enabled = false;
                mpBar[2].enabled = false;
                mpBar[1].enabled = false;
                mpBar[0].enabled = false;
            }

            if (_GameController.PotionQnt[1] > 0)
            {
                manaBox.SetActive(true);
            }
            else
            {
                manaBox.SetActive(false);
            }

        }

        IEnumerator CheckClassBar()
        {
            yield return new WaitForSeconds(0.1f);
            if (_GameController.CharacterClassID[_GameController.CharacterID] == 2)
            {
                mpHUD.SetActive(true);
            }
            if (_GameController.CharacterClassID[_GameController.CharacterID] == 1)
            {
                arrowIcon.sprite = _GameController.ArrowIcon[_GameController.EquipedArrowID];
                arrowHUD.SetActive(true);
            }
        }
    }
}
