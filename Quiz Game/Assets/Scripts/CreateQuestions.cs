using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateQuestions : MonoBehaviour
{
    // Start is called before the first frame update
    List<object> questions;
    [SerializeField] TMP_InputField questionField;
    [SerializeField] TMP_InputField optionField0;
    [SerializeField] TMP_InputField optionField1;
    [SerializeField] TMP_InputField optionField2;
    [SerializeField] TMP_InputField optionField3;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAddQuestionButtonClicked()
    {
        List<string> questionAnswers = new List<string>();
        string que = questionField.text.Trim();
        string opt0 = optionField0.text.Trim();
        string opt1 = optionField1.text.Trim();
        string opt2 = optionField2.text.Trim();
        string opt3 = optionField3.text.Trim();
        if (que=="" || opt0=="" || opt1=="" || opt2=="" || opt3 == "")
        {
            Debug.Log("Invalid");
        }
        else
        {
            questionAnswers.Add(que);
            questionAnswers.Add(opt0);
            questionAnswers.Add(opt1);
            questionAnswers.Add(opt2);
            questionAnswers.Add(opt3);
            questions.Add(questionAnswers);
        }

    }

    public void SaveQuestions()
    {

    }
}
