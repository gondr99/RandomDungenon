using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class MainScreen : MonoBehaviour
{
    public UnityEvent OnGenereate;
    private UIDocument _uiDocument;
    
    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;
        Button btn = root.Q<Button>("generate-btn");
        
        btn.RegisterCallback<ClickEvent>(HandleGenerateBtnClick);
    }

    private void HandleGenerateBtnClick(ClickEvent evt)
    {
        OnGenereate?.Invoke();
    }
}
