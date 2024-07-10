using UnityEngine;

public class ToggleIcon : MonoBehaviour
{
    [SerializeField] SpriteRenderer iconVibrationRenderer;
    [SerializeField] SpriteRenderer iconVolumnRenderer;
    [SerializeField] SpriteMask iconVibrationMask;

    private bool isVibrationMasked = false;

    void Start()
    {
        if (iconVibrationRenderer == null || iconVibrationMask == null)
        {
            Debug.LogError("Please assign the iconVibrationRenderer and iconMask in the inspector.");
            return;
        }

        // Kiểm tra trạng thái ban đầu
        isVibrationMasked = iconVibrationRenderer.maskInteraction != SpriteMaskInteraction.None;
    }

    public void ToggleIconVibrationMask()
    {
        isVibrationMasked = !isVibrationMasked;

        if (isVibrationMasked)
        {
            // Khi được click, icon chỉ hiển thị phần giữa (hình chữ nhật)
            iconVibrationRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
        else
        {
            // Khi được click lần nữa, icon hiển thị toàn bộ
            iconVibrationRenderer.maskInteraction = SpriteMaskInteraction.None;
        }
    }

    public void ToggleIconVolumn()
    {
        iconVolumnRenderer.transform.GetChild(0).gameObject.SetActive(!iconVolumnRenderer.transform.GetChild(0).gameObject.activeInHierarchy);
    }
}
