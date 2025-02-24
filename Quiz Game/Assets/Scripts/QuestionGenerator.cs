using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using TMPro;
using UnityEngine.UI;


public class QuestionGenerator : MonoBehaviour
{
    List<List<Object>> question = new List<List<Object>>();
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] Button[] optionButtons;
    [SerializeField] TextMeshProUGUI[] optionText;
    List<object> options;
    // Start is called before the first frame update
    void Start()
    {
        NextQuestion();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextQuestion()
    {
        List<object> question = Question_Generator();
        questionText.text = question[0].ToString();
        options = AnswerOptions((int)question[1]);
        for (int i = 0; i < 4; i++)
        {
            List<int> opts = (List<int>)options[0];
            optionText[i].text = opts[i].ToString();
        }
    }

    bool ValidateAnswer(int optionPressed)
    {
        if (optionPressed == (int)options[1]) return true;
        else return false;
    }

    List<object> Question_Generator()
    {
        int num1 = Random.Range(10, 100);
        int num2 = Random.Range(10, 100);
        char[] oper = { '+', '-', '*', '/' };
        int index = Random.Range(0, oper.Length);
        char ope = oper[index];
        string que = "";
        int ans = 0;
        if (ope == '/')
        {
            int mul = num1 * num2;
            que = mul.ToString() + " " + ope + " " + num2.ToString();
            num1 = mul;
        }
        else
        {
            que = num1.ToString() + " " + ope + " " + num2.ToString();
        }
        switch (ope)
        {
            case '+':
                ans = num1 + num2;
                break;
            case '-':
                ans = num1 - num2;
                break;
            case '*':
                ans = num1 * num2;
                break;
            case '/':
                ans = num1 / num2;
                break;
        }
        List<object> res = new List<object>();
        res.Add(que);
        res.Add(ans);

        return res;
    }


    List<object> AnswerOptions(int ans)
    {
        List<int> opt = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            int a = Random.Range(ans - 50, ans + 50);
            opt.Add(a);
        }
        int correctIndex = Random.Range(0, 4);
        opt.Insert(correctIndex, ans);
        List<object> options = new List<object>();
        options.Add(opt);
        options.Add(correctIndex);
        return options;
    }

    public void OptionButtonClick(int optionNumber)
    {
        if (ValidateAnswer(optionNumber))
        {
            NextQuestion();
        }
        else
        {
            Debug.Log("GameOver");
        }
    }
}
    /*
    void addQuestion(string que, List<string> opt)
    {
        List<string> l = new List<string>();
        string correct = opt[0];
        int cor = 0;
        for (int i = 0; i < opt.Count; i++)
        {
            int n = Random.Range(0, opt.Count);
            string temp = opt[i];
            opt[i] = opt[n];
            opt[n] = temp;
        }
        cor = opt.IndexOf(correct);

        l.Add(que);
        l.Add(cor);
        l.Add(opt);
        question.Add(l);

    }
    List<String> opt = new List<String>();
    opt.Add("1");
    opt.Add("2");
    opt.Add("3");
    opt.Add("4");

    addQuestion("Heljf", opt);
    Console.WriteLine(question[0][0]);
    Console.WriteLine(question[0][1]);
    List<String> temp = (List<String>)question[0][2];
    for (int i = 0; i < temp.Count(); i++)
    {
        Console.WriteLine(temp[i]);
    }



    //List<Object> que = new List<object>();

    //que = Question_Generator();
    //List<int> opt = answerOptions((int)que[1]);
    //Console.WriteLine(que[0]);
    //Console.WriteLine(que[1]);
    //for (int i=0; i<4; i++)
    //{
    //    Console.WriteLine(opt[i]);
    //}
    */