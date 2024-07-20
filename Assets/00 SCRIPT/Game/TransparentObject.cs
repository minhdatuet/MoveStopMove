using System.Collections.Generic;
using UnityEngine;

public class TransparentObjects : MonoBehaviour
{
    public Transform target; // Nhân vật
    private List<Renderer> lastRenderers = new List<Renderer>(); // Các renderer đã được làm trong suốt
    private Dictionary<Renderer, Shader> originalShaders = new Dictionary<Renderer, Shader>(); // Lưu trữ shader ban đầu của các vật thể
    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>(); // Lưu trữ màu sắc ban đầu của các vật thể
    private Shader transparentShader; // Shader trong suốt
    private float transparency = 0.3f; // Độ trong suốt
    private float radiusAttack = 0.0f;

    void Start()
    {
        transparentShader = Shader.Find("Custom/TransparentShader");
    }

    void Update()
    {
        radiusAttack = target.GetComponent<PlayerController>().RadiusAttack / 2;

        // Khôi phục lại các vật thể trước đó
        foreach (Renderer renderer in lastRenderers)
        {
            if (originalShaders.ContainsKey(renderer))
            {
                renderer.material.shader = originalShaders[renderer]; 
                renderer.material.color = originalColors[renderer]; 
            }
        }

        lastRenderers.Clear(); // Xóa danh sách các renderer trước đó

        // Tìm các vật thể nằm trong vùng radiusAttack
        Collider[] colliders = Physics.OverlapSphere(target.position, radiusAttack);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("CanHidden")) // Chỉ ẩn các đối tượng có tag "CanHidden"
            {
                Renderer renderer = collider.GetComponent<Renderer>();
                if (renderer != null)
                {
                    if (!originalShaders.ContainsKey(renderer))
                    {
                        originalShaders[renderer] = renderer.material.shader;
                        originalColors[renderer] = renderer.material.color;
                    }

                    renderer.material.shader = transparentShader; 
                    Color color = renderer.material.color;
                    color.a = transparency; 
                    renderer.material.color = color;
                    lastRenderers.Add(renderer); 
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Vẽ một quả cầu tượng trưng cho vùng radiusAttack trong chế độ Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.position, radiusAttack);
    }
}
