using System.Collections.Generic;
using UnityEngine;

public class TransparentObjects : MonoBehaviour
{
    public Transform target; // Nhân vật
    //public LayerMask layerMask; // Layer của các vật thể chắn đường
    private List<Renderer> lastRenderers = new List<Renderer>(); // Các renderer đã được làm trong suốt
    private Dictionary<Renderer, Shader> originalShaders = new Dictionary<Renderer, Shader>(); // Lưu trữ shader ban đầu của các vật thể
    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>(); // Lưu trữ màu sắc ban đầu của các vật thể
    private Shader transparentShader; // Shader trong suốt
    private float transparency = 0.3f; // Độ trong suốt

    void Start()
    {
        transparentShader = Shader.Find("Custom/TransparentShader");
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, target.position - transform.position); // Tia từ camera đến nhân vật
        RaycastHit[] hits = Physics.RaycastAll(ray, Vector3.Distance(transform.position, target.position)); // Tìm các vật thể chắn đường

        // Khôi phục lại các vật thể trước đó
        foreach (Renderer renderer in lastRenderers)
        {
            if (originalShaders.ContainsKey(renderer))
            {
                renderer.material.shader = originalShaders[renderer]; // Khôi phục lại shader ban đầu
                renderer.material.color = originalColors[renderer]; // Khôi phục lại màu sắc ban đầu
            }
        }

        lastRenderers.Clear(); // Xóa danh sách các renderer trước đó

        // Làm trong suốt các vật thể mới chắn đường
        foreach (RaycastHit hit in hits)
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                if (!originalShaders.ContainsKey(renderer))
                {
                    originalShaders[renderer] = renderer.material.shader; // Lưu trữ shader ban đầu
                    originalColors[renderer] = renderer.material.color; // Lưu trữ màu sắc ban đầu
                }

                renderer.material.shader = transparentShader; // Đổi shader thành trong suốt
                Color color = renderer.material.color;
                color.a = transparency; // Đặt độ trong suốt
                renderer.material.color = color;
                lastRenderers.Add(renderer); // Thêm vào danh sách các renderer đã được làm trong suốt
            }
        }
    }
}
