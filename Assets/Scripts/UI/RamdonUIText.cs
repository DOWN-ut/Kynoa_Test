using UnityEngine;

public class RamdonUIText : MonoBehaviour
{
    [SerializeField][Multiline] string[] texts;

    [SerializeField] float duration; float d;

    private void OnEnable()
    {
        UpdateRandom();
    }

    void UpdateRandom()
    {
        GetComponent<TMPro.TMP_Text>().text = texts[Random.Range(0, texts.Length)];
        d = 0;
    }


    private void Update()
    {
        if(d < duration)
        {
            d += Time.deltaTime;
        }
        else
        {
            UpdateRandom();
        }
    }
}
