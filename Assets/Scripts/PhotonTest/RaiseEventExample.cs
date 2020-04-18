using UnityEngine;

public class RaiseEventExample : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ChangeColor();
        }
    }

    private void ChangeColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);

        _spriteRenderer.color = new Color(r, g, b, 1f);
    }
}

