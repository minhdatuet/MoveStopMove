using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : Singleton<ObjectPooling>
{
    // Tạo một Dictionary để lưu trữ các danh sách các GameObject
    [SerializeField] Dictionary<GameObject, List<GameObject>> _listObjects = new Dictionary<GameObject, List<GameObject>>();

    // Phương thức lấy đối tượng từ pool
    public GameObject GetObject(GameObject defaultPrefab, GameObject parentObject)
    {
        // Đảm bảo _listObjects có một mục cho defaultPrefab
        if (!_listObjects.ContainsKey(defaultPrefab))
        {
            _listObjects[defaultPrefab] = new List<GameObject>();
        }

        // Thêm tất cả các đối tượng con của parentObject vào _listObjects[defaultPrefab]
        foreach (Transform child in parentObject.transform)
        {
            if (!_listObjects[defaultPrefab].Contains(child.gameObject))
            {
                _listObjects[defaultPrefab].Add(child.gameObject);
            }
        }

        // Duyệt qua danh sách để tìm một đối tượng không hoạt động
        foreach (GameObject o in _listObjects[defaultPrefab])
        {
            if (!o.activeSelf)
            {
                return o; // Trả về đối tượng không hoạt động
            }
        }

        // Nếu không tìm thấy đối tượng nào không hoạt động, khởi tạo một đối tượng mới
        GameObject g = Instantiate(defaultPrefab, new Vector3(0, 0, 0), Quaternion.identity, parentObject.transform);
        _listObjects[defaultPrefab].Add(g); // Thêm đối tượng mới vào danh sách
        g.SetActive(false); // Đặt đối tượng mới là không hoạt động

        return g; // Trả về đối tượng mới
    }
}