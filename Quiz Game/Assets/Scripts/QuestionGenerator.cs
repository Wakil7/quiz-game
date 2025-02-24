using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

List<List<Object>> question = new List<List<Object>>();

public class QuestionGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
List<object> Question_Generator()
{
    Random ran = new Random();
    int num1 = ran.Next(10, 100);
    int num2 = ran.Next(10, 100);
    char[] oper = { '+', '-', '*', '/' };
    int index = ran.Next(oper.Length);
    char ope = oper[index];
    String que = "";
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
List<int> answerOptions(int ans)
{
    Random r = new Random();
    List<int> opt = new List<int>();
    for (int i = 0; i < 3; i++)
    {
        int a = r.Next(ans - 50, ans + 50);
        opt.Add(a);
    }
    int correctIndex = r.Next(0, 4);
    opt.Insert(correctIndex, ans);
    return opt;
}
void addQuestion(String que, List<String> opt)
{
    List<Object> l = new List<Object>();
    Random r = new Random();
    String correct = opt[0];
    int cor = 0;
    for (int i = 0; i < opt.Count(); i++)
    {
        int n = r.Next(0, opt.Count());
        String temp = opt[i];
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
}
