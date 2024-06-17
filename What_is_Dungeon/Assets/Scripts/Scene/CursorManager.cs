using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }


    public Texture2D defaultCursor;
    public Texture2D clickCursor;
    public Vector2 cursorHotspot = Vector2.zero;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� ��ȯ�Ǵ��� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        // �⺻ ���� Ŀ�� ����
        SetDefaultCursor();
    }

    void Update()
    {
        // ���콺 ��ư Ŭ�� üũ
        if (Input.GetMouseButtonDown(0))
        {
            SetClickCursor();
        }
        else if (Input.GetMouseButton(0))
        {
            SetClickCursor();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            SetDefaultCursor();
        }
    }

    void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
    }

    void SetClickCursor()
    {
        Cursor.SetCursor(clickCursor, cursorHotspot, CursorMode.Auto);
    }
}
