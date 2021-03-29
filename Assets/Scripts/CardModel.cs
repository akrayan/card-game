using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CardModel : MonoBehaviour
{
    [SerializeField] Sprite[] Faces;
    [SerializeField] Sprite Back;

    public Card Card;
    public bool m_faceHidden = true;

    SpriteRenderer m_renderer;

    private void Awake()
    {
        m_renderer = GetComponent<SpriteRenderer>();

        UpdateSprite();
    }

    void UpdateSprite()
    {
        if (m_faceHidden)
            m_renderer.sprite = Back;
        else
        {
            m_renderer.sprite = Faces[(int)Card.Color * 14 + Card.Value -1];
        }

    }

    public void ToggleFace(bool faceHidden)
    {
        m_faceHidden = faceHidden;
        UpdateSprite();
    }
}
