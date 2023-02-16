using TMPro;
using UnityEngine;

public class ItemView : MonoBehaviour
{
    [SerializeField] public TextMeshPro Text;
    [SerializeField] private SpriteRenderer SpriteRenderer;

    private bool _needMove;
    private float _speed;
    private int _maxX;
    private int _minX;

    public void SetData(int maxX, int minX, float animationSpeed, string text, Color color)
    {
        _maxX = maxX;
        _minX = minX;
        _speed = animationSpeed;
        Text.text = text;
        SpriteRenderer.color = color;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    
    public void SetMove(bool needMove)
    {
        _needMove = needMove;
    }
    
    private void Update()
    {
        if (!_needMove) return;
        
        transform.Translate(new Vector3(0.01f * _speed,0,0),Space.Self);
        if (transform.position.x >= _maxX)
        {
            transform.position = new Vector3(_minX, 0, 0);
        }
        
    }
}
