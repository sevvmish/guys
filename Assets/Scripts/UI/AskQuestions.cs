using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AskQuestions : MonoBehaviour
{
    [SerializeField] private Button closeB;
    [SerializeField] private Button sendData;

    [Header("texts")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI sendText;
    [SerializeField] private TextMeshProUGUI button0Text;
    [SerializeField] private TextMeshProUGUI button1Text;
    [SerializeField] private TextMeshProUGUI button2Text;
    [SerializeField] private TextMeshProUGUI button3Text;
    [SerializeField] private TextMeshProUGUI button4Text;


    [Header("buttons")]
    [SerializeField] private Button[] buttonAnswer;


    private bool isReady;
    private int[] answers;

    private float _timer;


    // Start is called before the first frame update
    void Start()
    {
        answers = new int[5];
        sendData.interactable = false;

        closeB.onClick.AddListener(() =>
        {
            SoundUI.Instance.PlayUISound(SoundsUI.click);
            this.gameObject.SetActive(false);
        });

        sendData.onClick.AddListener(() =>
        {
            SoundUI.Instance.PlayUISound(SoundsUI.positive);
            string toSend = "questions:" + answers[0] +"-" + answers[1] + "-" + answers[2] + "-" + answers[3] + "-" + answers[4];
            print(toSend);
            Analitycs.Instance.Send(toSend);            
            this.gameObject.SetActive(false);
        });

        for (int i = 0; i < 5; i++)
        {
            buttonAnswer[i].transform.GetChild(2).gameObject.SetActive(false);
            buttonAnswer[i].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        }

        buttonAnswer[0].onClick.AddListener(() =>
        {
            int index = 0;
            SoundUI.Instance.PlayUISound(SoundsUI.click);

            if (!buttonAnswer[index].transform.GetChild(2).gameObject.activeSelf)
            {
                buttonAnswer[index].transform.GetChild(2).gameObject.SetActive(true);
                answers[index] = 1;
                buttonAnswer[index].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                buttonAnswer[index].transform.GetChild(2).gameObject.SetActive(false);
                answers[index] = 0;
                buttonAnswer[index].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
            }
        });

        buttonAnswer[1].onClick.AddListener(() =>
        {
            int index = 1;
            SoundUI.Instance.PlayUISound(SoundsUI.click);

            if (!buttonAnswer[index].transform.GetChild(2).gameObject.activeSelf)
            {
                buttonAnswer[index].transform.GetChild(2).gameObject.SetActive(true);
                answers[index] = 1;
                buttonAnswer[index].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                buttonAnswer[index].transform.GetChild(2).gameObject.SetActive(false);
                answers[index] = 0;
                buttonAnswer[index].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
            }
        });


        buttonAnswer[2].onClick.AddListener(() =>
        {
            int index = 2;
            SoundUI.Instance.PlayUISound(SoundsUI.click);

            if (!buttonAnswer[index].transform.GetChild(2).gameObject.activeSelf)
            {
                buttonAnswer[index].transform.GetChild(2).gameObject.SetActive(true);
                answers[index] = 1;
                buttonAnswer[index].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                buttonAnswer[index].transform.GetChild(2).gameObject.SetActive(false);
                answers[index] = 0;
                buttonAnswer[index].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
            }
        });

        buttonAnswer[3].onClick.AddListener(() =>
        {
            int index = 3;
            SoundUI.Instance.PlayUISound(SoundsUI.click);

            if (!buttonAnswer[index].transform.GetChild(2).gameObject.activeSelf)
            {
                buttonAnswer[index].transform.GetChild(2).gameObject.SetActive(true);
                answers[index] = 1;
                buttonAnswer[index].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                buttonAnswer[index].transform.GetChild(2).gameObject.SetActive(false);
                answers[index] = 0;
                buttonAnswer[index].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
            }
        });

        buttonAnswer[4].onClick.AddListener(() =>
        {
            int index = 4;
            SoundUI.Instance.PlayUISound(SoundsUI.click);

            if (!buttonAnswer[index].transform.GetChild(2).gameObject.activeSelf)
            {
                buttonAnswer[index].transform.GetChild(2).gameObject.SetActive(true);
                answers[index] = 1;
                buttonAnswer[index].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                buttonAnswer[index].transform.GetChild(2).gameObject.SetActive(false);
                answers[index] = 0;
                buttonAnswer[index].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
            }
        });


    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;
            title.text = Globals.Language.WhatToImprove;
            sendText.text = Globals.Language.Send;
            button0Text.text = Globals.Language.question0;
            button1Text.text = Globals.Language.question1;
            button2Text.text = Globals.Language.question2;
            button3Text.text = Globals.Language.question3;
            button4Text.text = Globals.Language.question4;
        }


        if (_timer > 0.2f)
        {
            _timer = 0;

            bool isIn = false;
            for (int i = 0; i < answers.Length; i++)
            {
                if (answers[i] == 1)
                {
                    isIn= true;
                    break;
                }
            }

            if (isIn && !sendData.interactable)
            {
                sendData.interactable = true;
            }
            else if (!isIn && sendData.interactable)
            {
                sendData.interactable = false;
            }
        }
        else
        {
            _timer += Time.deltaTime;
        }

        
    }
}
