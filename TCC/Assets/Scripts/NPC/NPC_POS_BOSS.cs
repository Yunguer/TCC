using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using TMPro;
using System;
using UnityEngine.UI;

namespace ProjetoTCC
{
    public class NPC_POS_BOSS : MonoBehaviour
    {
        private _GameController _GameController;

        [SerializeField]
        private string xmlName;

        public static Action OnAnimationEnded;

        [SerializeField]
        private GameObject npcCanvas;

        [SerializeField]
        private TMP_Text textBox;

        [SerializeField]
        private int dialogueLineID;

        [SerializeField]
        private List<string> dialogue_1;

        [SerializeField]
        private List<string> dialogue_2;

        [SerializeField]
        private List<string> chatLines;

        private int chatID;

        private bool isTalking;


        void Start()
        {
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;

            npcCanvas.SetActive(false);
            isTalking = false;

            LoadDialogueData();
        }

        public void Interaction()
        {
            if (_GameController.CurrentGameState == GameState.GAMEPLAY)
            {
                _GameController.ChangeState(GameState.DIALOGO);
                dialogueLineID = 0;

                if (chatLines != null)
                {
                    chatLines.Clear();
                }


                PreparDialogue();

                Dialogue();
                npcCanvas.SetActive(true);
                isTalking = true;
                OnAnimationEnd();
            }

        }

        public void Talk()
        {
            if (isTalking == true)
            {
                dialogueLineID += 1;
                Dialogue();
                OnAnimationEnd();
            }
        }

        public void Dialogue()
        {
            if (dialogueLineID < chatLines.Count)
            {
                textBox.text = chatLines[dialogueLineID];
            }
            else
            {
                switch (chatID)
                {
                    case 0:
                        chatID = 1;

                        break;

                    case 1:

                        break;
                }

                npcCanvas.SetActive(false);
                isTalking = false;

                _GameController.ChangeState(GameState.FIM_DIALOGO);
            }

        }

        public void PreparDialogue()
        {
            chatLines.Clear();
            switch (chatID)
            {
                case 0:

                    foreach (string s in dialogue_1)
                    {
                        chatLines.Add(s);
                    }

                    break;

                case 1:

                    foreach (string s in dialogue_2)
                    {
                        chatLines.Add(s);
                    }

                    break;
            }
        }

        public void LoadDialogueData()
        {
            TextAsset xmlData = (TextAsset)Resources.Load(xmlName);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlData.text);

            foreach (XmlNode dialogue in xmlDocument["chats"].ChildNodes)
            {
                string dialogueName = dialogue.Attributes["name"].Value;

                foreach (XmlNode f in dialogue["lines"].ChildNodes)
                {
                    switch (dialogueName)
                    {
                        case "dialogue_1":
                            dialogue_1.Add(f.InnerText);
                            break;

                        case "dialogue_2":
                            dialogue_2.Add(f.InnerText);
                            break;
                    }
                }
            }
        }

        public void OnAnimationEnd()
        {
            OnAnimationEnded.Invoke();
        }
    }
}
