using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using TMPro;
using System;
using UnityEngine.UI;

namespace ProjetoTCC
{
    public class NPC : MonoBehaviour
    {
        [SerializeField]
        private string xmlName;

        public static Action OnAnimationEnded;

        [SerializeField]
        private GameObject npcCanvas;

        [SerializeField]
        private TMP_Text textBox;

        [SerializeField]
        private GameObject answerPainel;

        private bool answeringQuestion;

        [SerializeField]
        private Button btn_A;

        [SerializeField]
        private TMP_Text textButtonA, textButtonB;

        [SerializeField]
        private int dialogueLineID;

        [SerializeField]
        private List<string> dialogue_1;
        
        [SerializeField]
        private List<string> dialogue_2;

        [SerializeField]
        private List<string> dialogue_3;

        [SerializeField]
        private List<string> dialogue_4;

        [SerializeField]
        private List<string> answerQuestion;

        [SerializeField]
        private List<string> chatLines;

        private int chatID;

        private bool isTalking;


        void Start()
        {
            npcCanvas.SetActive(false);
            answerPainel.SetActive(false);
            isTalking = false;

            LoadDialogueData();
        }


        void Update()
        {

        }

        public void Interaction()
        {
            if(isTalking == false)
            {
                dialogueLineID = 0;

                if(chatLines != null)
                {
                    chatLines.Clear();
                }

                PreparDialogue();

                Dialogue();
                npcCanvas.SetActive(true);
                isTalking = true;
                OnAnimationEnd();
            }
            else if(isTalking == true && answeringQuestion == false)
            {
                dialogueLineID += 1;
                Dialogue();
                OnAnimationEnd();
            }
        }

        public void Dialogue()
        {
            if(dialogueLineID < chatLines.Count)
            {
                textBox.text = chatLines[dialogueLineID];

                if(chatID == 0 && dialogueLineID == 2)
                {
                    textButtonA.text = answerQuestion[0];
                    textButtonB.text = answerQuestion[1];

                    answeringQuestion = true;
                    answerPainel.SetActive(true);
                    btn_A.Select();
                }
            }
            else
            {
                switch(chatID)
                {
                    case 0:

                        break;

                    case 1:
                        chatID = 3;
                        break;

                    case 2:
                        chatID = 0;
                        break;
                }

                npcCanvas.SetActive(false);
                isTalking = false;
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

                case 2:

                    foreach (string s in dialogue_3)
                    {
                        chatLines.Add(s);
                    }

                    break;

                case 3:

                    foreach (string s in dialogue_4)
                    {
                        chatLines.Add(s);
                    }

                    break;
            }
        }

        public void AnswerAButton()
        {
            chatID = 1;
            PreparDialogue();
            dialogueLineID = 0;

            answeringQuestion = false;
            answerPainel.SetActive(false);

            Dialogue();
            OnAnimationEnd();
        }

        public void AnswerBButton()
        {
            chatID = 2;
            PreparDialogue();
            dialogueLineID = 0;

            answeringQuestion = false;
            answerPainel.SetActive(false);

            Dialogue();
            OnAnimationEnd();
        }

        public void LoadDialogueData()
        {
            TextAsset xmlData = (TextAsset)Resources.Load(xmlName);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlData.text);

            foreach(XmlNode dialogue in xmlDocument["chats"].ChildNodes)
            {
                string dialogueName = dialogue.Attributes["name"].Value;

                foreach(XmlNode f in dialogue["lines"].ChildNodes)
                {
                    switch(dialogueName)
                    {
                        case "dialogue_1":
                            dialogue_1.Add(f.InnerText);
                            break;

                        case "dialogue_2":
                            dialogue_2.Add(f.InnerText);
                            break;

                        case "dialogue_3":
                            dialogue_3.Add(f.InnerText);
                            break;

                        case "dialogue_4":
                            dialogue_4.Add(f.InnerText);
                            break;

                        case "resposta_1":
                            answerQuestion.Add(f.InnerText);
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
