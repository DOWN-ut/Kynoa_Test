using UnityEngine;

public class RamdonUIText : MonoBehaviour
{
    [SerializeField][Multiline] string[] texts;

    private void OnEnable()
    {
        GetComponent<TMPro.TMP_Text>().text = texts[Random.Range(0, texts.Length)];
    }
}
