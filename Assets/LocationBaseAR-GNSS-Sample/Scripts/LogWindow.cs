using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogWindow : MonoBehaviour
{
    private static LogWindow _instance;
    public static LogWindow Instance => _instance;

    [SerializeField] private GameObject _debugConsole;
    [SerializeField] private TMP_Text _textComponent;
    [SerializeField] private Button _displayToggleButton;
    [SerializeField] private RectTransform _contentParent;

    void Awake ()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void ToggleDisplay()
    {
        _debugConsole.SetActive(!_debugConsole.activeSelf);
    }

    public void Log(string message)
    {
        _textComponent.text = message + "\n" + _textComponent.text;
    }
}
