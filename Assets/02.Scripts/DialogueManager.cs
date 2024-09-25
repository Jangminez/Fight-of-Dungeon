using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    Queue<string> messages;
    public Text dialogueText;

    void Awake()
    {
        messages = new Queue<string>();
    }

    // 대화 출력 부분
    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("대화 출력");

        messages.Clear();

        foreach(var message in dialogue.messages)
        {
            // 출력할 내용들 큐에 추가
            messages.Enqueue(message);
        }

        DisplayNextSentence();
    }

    // 다음 대화 출력을 위함 Queue가 비어있다면 대화 종료
    public void DisplayNextSentence()
    {
        if(messages.Count == 0)
        {
            EndDialogue();
            return;
        }
        
        // 출력할 내용 큐에서 꺼내기
        var message = messages.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeMessage(message));
    }

    IEnumerator TypeMessage(string message)
    {
        dialogueText.text = "";

        foreach(var ch in message.ToCharArray())
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(0.05f);
        }
    }

    // 대화 종료
    void EndDialogue()
    {
        Debug.Log("출력 종료");
    }
}
