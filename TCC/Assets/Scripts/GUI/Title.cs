using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

namespace ProjetoTCC
{
    public class Title : MonoBehaviour
    {
        [SerializeField]
        private Button loadGameBtn;
        
        [SerializeField]
        private Button loadSlot1Btn;
        [SerializeField]
        private Button loadSlot2Btn;
        [SerializeField]
        private Button loadSlot3Btn;

        [SerializeField]
        private Button newSlot1Btn;
        [SerializeField]
        private Button newSlot2Btn;
        [SerializeField]
        private Button newSlot3Btn;

        [SerializeField]
        private GameObject delSlot1Btn;
        [SerializeField]
        private GameObject delSlot2Btn;
        [SerializeField]
        private GameObject delSlot3Btn;


        // Start is called before the first frame update
        void Start()
        {
            VerifySavedGame();
        }

        public void SelectCharacter(int titleCharacterID)
        {
            PlayerPrefs.SetInt("titleCharacterID", titleCharacterID);
            SceneManager.LoadScene("Load");
        }

        public void VerifySavedGame()
        {
            loadGameBtn.interactable = false;
            loadSlot1Btn.interactable = false;
            loadSlot2Btn.interactable = false;
            loadSlot3Btn.interactable = false;

            newSlot1Btn.interactable = true;
            newSlot2Btn.interactable = true;
            newSlot3Btn.interactable = true;

            delSlot1Btn.SetActive(false);
            delSlot2Btn.SetActive(false);
            delSlot3Btn.SetActive(false);

            if(File.Exists(Application.persistentDataPath + "/playerdata1.dat"))
            {
                loadSlot1Btn.interactable = true;

                newSlot1Btn.interactable = false;

                delSlot1Btn.SetActive(true);

            }
            if(File.Exists(Application.persistentDataPath + "/playerdata2.dat"))
            {
                loadSlot2Btn.interactable = true;

                newSlot2Btn.interactable = false;

                delSlot2Btn.SetActive(true);


            }
            if (File.Exists(Application.persistentDataPath + "/playerdata3.dat"))
            {
                loadSlot3Btn.interactable = true;

                newSlot3Btn.interactable = false;

                delSlot3Btn.SetActive(true);
            }

            if(loadSlot1Btn.interactable == true || loadSlot2Btn.interactable == true || loadSlot3Btn.interactable == true)
            {
                loadGameBtn.interactable = true;
            }
        }

        public void NewGame(int slot)
        {

            switch(slot)
            {
                case 1:
                    PlayerPrefs.SetString("slot", "playerdata1.dat");
                    break;
                case 2:
                    PlayerPrefs.SetString("slot", "playerdata2.dat");
                    break;
                case 3:
                    PlayerPrefs.SetString("slot", "playerdata3.dat");
                    break;
            } 
        }

        public void LoadGame(int slot)
        {
            switch (slot)
            {
                case 1:
                    PlayerPrefs.SetString("slot", "playerdata1.dat");
                    break;
                case 2:
                    PlayerPrefs.SetString("slot", "playerdata2.dat");
                    break;
                case 3:
                    PlayerPrefs.SetString("slot", "playerdata3.dat");
                    break;
            }

            SceneManager.LoadScene("Load");
        }


        public void DeleteSave(int slot)
        {
            switch (slot)
            {
                case 1:
                    if(File.Exists(Application.persistentDataPath + "/playerdata1.dat"))
                    {
                        File.Delete(Application.persistentDataPath + "/playerdata1.dat");
                    }
                    break;
                case 2:
                    if (File.Exists(Application.persistentDataPath + "/playerdata2.dat"))
                    {
                        File.Delete(Application.persistentDataPath + "/playerdata2.dat");
                    }
                    break;
                case 3:
                    if (File.Exists(Application.persistentDataPath + "/playerdata3.dat"))
                    {
                        File.Delete(Application.persistentDataPath + "/playerdata3.dat");
                    }
                    break;
            }

            VerifySavedGame();
        }

    }

}