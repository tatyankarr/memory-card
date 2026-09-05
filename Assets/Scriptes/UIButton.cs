using UnityEngine;

public class UIButton : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private string targetMessage;

    public Color highlightColor = Color.cyan;

    private void OnMouseOver()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.color = highlightColor;
        }
    }

    private void OnMouseExit()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.color = Color.white;
        }
    }

    private void OnMouseDown()
    {
        if (targetObject != null)
        {
            targetObject.SendMessage(targetMessage);
        }
    }

    private void OnMouseUp()
    {
        
    }
}
