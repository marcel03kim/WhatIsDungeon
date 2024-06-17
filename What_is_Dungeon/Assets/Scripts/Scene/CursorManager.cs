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
            DontDestroyOnLoad(gameObject); // 씬이 전환되더라도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        // 기본 상태 커서 설정
        SetDefaultCursor();
    }

    void Update()
    {
        // 마우스 버튼 클릭 체크
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
