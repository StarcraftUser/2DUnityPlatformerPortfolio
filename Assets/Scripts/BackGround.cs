using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    private MeshRenderer m_Renderer;
    public PlayerMove player;
    private float Offset;
    // Start is called before the first frame update
    void Awake()
    {
        m_Renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Offset = player.transform.position.x * 0.01f;
        m_Renderer.material.mainTextureOffset = new Vector2(Offset, 0);
    }
}
