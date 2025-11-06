using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using UnityEngine.UI;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif


public class AltConsoleLogViewer : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform content;
    [SerializeField] public GameObject LogItemPrefab;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private InputField filterInput;
    [SerializeField] private Button clearButton;
    [SerializeField] private Button copyButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button resetFilterButton;
    [SerializeField] private Toggle logToggle;
    [SerializeField] private Toggle warningToggle;
    [SerializeField] private Toggle errorToggle;
    [SerializeField] public Color NormalToggleColor;
    [SerializeField] public Color ActiveToggleColor;

    [Header("Settings")]
    [SerializeField] private int poolSize = 50;
    [SerializeField] private float itemHeight = 45f;
    [SerializeField] private int maxLogCount = 1000;
    [SerializeField] private float verticalPadding = 5f;
    [SerializeField] private float topBottomPadding = 10f;


    private Canvas canvas;
    private GameObject notificationPrefab;

    [Header("Animation Settings")]
    public float fadeDuration = 0.5f;
    public float showDuration = 1.0f;

    private Image logToggleImage;
    private Image warningToggleImage;
    private Image errorToggleImage;

    private List<RectTransform> pooledItems = new List<RectTransform>();
    private List<LogData> allLogs = new List<LogData>();
    private List<LogData> filteredLogs = new List<LogData>();
    private float contentHeight = 0f;
    private bool needsRefresh = false;
    private string currentFilter = "";
    private bool showLogs = true;
    private bool showWarnings = true;
    private bool showErrors = true;
    private bool isInitialized = false;
    private TextGenerationSettings textGenerationSettings;
    private TextGenerator textGenerator;

    public static AltConsoleLogViewer Instance { get; private set; }

#if UNITY_WEBGL && !UNITY_EDITOR
[DllImport("__Internal")]
private static extern void CopyToClipboard(string str);
#endif

    public class LogData
    {
        public string Message;
        public string StackTrace;
        public LogType LogType;
        public string FullText;
        public float Width;
    }
    public void Copy(string str)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        CopyToClipboard(str);
#endif
    }

    protected void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(this);
            return;
        }
        Instance = this;
        content = GameObject.Find("AltTesterPrefab/AltDialog/LogsPanel/Scroll View/Viewport/Content").GetComponent<RectTransform>();
        scrollRect = GameObject.Find("AltTesterPrefab/AltDialog/LogsPanel/Scroll View").GetComponent<ScrollRect>();
        filterInput = GameObject.Find("AltTesterPrefab/AltDialog/LogsPanel/LandscapeLayout/Filter").GetComponent<InputField>();
        clearButton = GameObject.Find("AltTesterPrefab/AltDialog/LogsPanel/LandscapeLayout/ClearButton").GetComponent<Button>();
        copyButton = GameObject.Find("AltTesterPrefab/AltDialog/LogsPanel/LandscapeLayout/CopyButton").GetComponent<Button>();
        closeButton = GameObject.Find("AltTesterPrefab/AltDialog/LogsPanel/LandscapeLayout/CloseButton").GetComponent<Button>();
        resetFilterButton = GameObject.Find("AltTesterPrefab/AltDialog/LogsPanel/LandscapeLayout/Filter/ResetSearchButton").GetComponent<Button>();
        logToggle = GameObject.Find("AltTesterPrefab/AltDialog/LogsPanel/LandscapeLayout/Log").GetComponent<Toggle>();
        warningToggle = GameObject.Find("AltTesterPrefab/AltDialog/LogsPanel/LandscapeLayout/Warning").GetComponent<Toggle>();
        errorToggle = GameObject.Find("AltTesterPrefab/AltDialog/LogsPanel/LandscapeLayout/Error").GetComponent<Toggle>();
        canvas = GameObject.Find("AltTesterPrefab/AltDialog").GetComponent<Canvas>();
        notificationPrefab = GameObject.Find("AltTesterPrefab/AltDialog/BackgroundClipboard");

        initializePool();
        setupUIListeners();
        Application.logMessageReceivedThreaded += handleLog;
        isInitialized = true;
        needsRefresh = true;
        content.anchoredPosition = Vector2.zero;
        scrollRect.verticalNormalizedPosition = 1;
        resetFilterButton.gameObject.SetActive(false);
        resetFilterButton.onClick.AddListener(() => { filterInput.text = ""; resetFilterButton.gameObject.SetActive(false); });
        notificationPrefab.SetActive(false);
        scrollToBottom();

    }

    protected void Start()
    {
        scrollToBottom();
        SetTextGenerationSettings();
    }

    private void SetTextGenerationSettings()
    {
        if (pooledItems.Count == 0) return;
        textGenerator = new UnityEngine.TextGenerator();
        UnityEngine.UI.Text referenceText = pooledItems.Count > 0 ? pooledItems[0].GetComponentInChildren<UnityEngine.UI.Text>() : null;
        textGenerationSettings = new UnityEngine.TextGenerationSettings();
        if (referenceText != null)
        {
            textGenerationSettings.font = referenceText.font;
            textGenerationSettings.fontSize = referenceText.fontSize;
            textGenerationSettings.fontStyle = referenceText.fontStyle;
            textGenerationSettings.richText = true; // Enable rich text for accurate width
            textGenerationSettings.scaleFactor = referenceText.canvas != null ? referenceText.canvas.scaleFactor : 1f;
            textGenerationSettings.color = referenceText.color;
            textGenerationSettings.lineSpacing = referenceText.lineSpacing;
            textGenerationSettings.textAnchor = referenceText.alignment;
            textGenerationSettings.horizontalOverflow = HorizontalWrapMode.Overflow;
            textGenerationSettings.verticalOverflow = VerticalWrapMode.Overflow;
            textGenerationSettings.generateOutOfBounds = true;
            textGenerationSettings.pivot = Vector2.zero;
        }
    }

    protected void OnEnable()
    {
        scrollToBottom();
    }

    private void initializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject item = Instantiate(LogItemPrefab, content);
            item.SetActive(false);
            pooledItems.Add(item.GetComponent<RectTransform>());
        }
    }

    private void setupUIListeners()
    {
        logToggleImage = logToggle.GetComponent<Image>();
        warningToggleImage = warningToggle.GetComponent<Image>();
        errorToggleImage = errorToggle.GetComponent<Image>();
        logToggleImage.color = showLogs ? ActiveToggleColor : NormalToggleColor;
        warningToggleImage.color = showWarnings ? ActiveToggleColor : NormalToggleColor;
        errorToggleImage.color = showErrors ? ActiveToggleColor : NormalToggleColor;
        if (clearButton != null)
            clearButton.onClick.AddListener(clearLogs);

        if (copyButton != null)
            copyButton.onClick.AddListener(copyLogs);

        if (closeButton != null)
            closeButton.onClick.AddListener(closePanel);

        if (filterInput != null)
            filterInput.onValueChanged.AddListener(filterLogs);

        if (logToggle != null)
            logToggle.onValueChanged.AddListener((value) => { showLogs = value; needsRefresh = true; logToggleImage.color = value ? ActiveToggleColor : NormalToggleColor; });

        if (warningToggle != null)
            warningToggle.onValueChanged.AddListener((value) => { showWarnings = value; needsRefresh = true; warningToggleImage.color = value ? ActiveToggleColor : NormalToggleColor; });

        if (errorToggle != null)
            errorToggle.onValueChanged.AddListener((value) => { showErrors = value; needsRefresh = true; errorToggleImage.color = value ? ActiveToggleColor : NormalToggleColor; });
        scrollRect.onValueChanged.AddListener(handleScroll);
    }

    private void handleScroll(Vector2 arg0)
    {
        needsRefresh = true;
    }

    private void handleLog(string logString, string stackTrace, LogType type)
    {
        if (!isInitialized) return;
        if (logString.Contains("|DoNotHandle|")) return;
        string timeStamp = DateTime.Now.ToString("HH:mm:ss");
        var logData = new LogData
        {
            Message = logString,
            StackTrace = stackTrace,
            LogType = type,
            FullText = formatLogText(timeStamp, logString, stackTrace, type),
            Width = -1
        };

        // Add to collection
        allLogs.Add(logData);

        // Enforce max log count
        if (allLogs.Count > maxLogCount)
        {
            allLogs.RemoveAt(0);
        }

        // Apply current filters
        if (shouldShowLog(logData))
        {
            filteredLogs.Add(logData);
            needsRefresh = true;
        }
    }

    private bool shouldShowLog(LogData log)
    {
        bool typeMatches = false;
        switch (log.LogType)
        {
            case LogType.Log:
                typeMatches = showLogs;
                break;
            case LogType.Warning:
                typeMatches = showWarnings;
                break;
            case LogType.Error:
            case LogType.Exception:
            case LogType.Assert:
                typeMatches = showErrors;
                break;
        }

        if (!typeMatches) return false;

        if (!string.IsNullOrEmpty(currentFilter) && !log.FullText.Contains(currentFilter))
        {
            return false;
        }

        return true;
    }

    private string formatLogText(string timeStamp, string message, string stackTrace, LogType type)
    {
        var sb = new StringBuilder();

        sb.Append($"<color=#aaaaaa>[{timeStamp}]</color> ");
        switch (type)
        {
            case LogType.Error:
            case LogType.Exception:
            case LogType.Assert:
                sb.Append("<color=red>");
                break;
            case LogType.Warning:
                sb.Append("<color=yellow>");
                break;
            default:
                sb.Append("<color=white>");
                break;
        }

        sb.Append(message);
        sb.Append("</color>");

        return sb.ToString();
    }

    protected void Update()
    {
        if (needsRefresh)
        {
            refreshLogDisplay();
            needsRefresh = false;
        }
    }

    private void refreshLogDisplay()
    {
        updateFilteredLogs();
        updateContentHeight();
        updateVisibleItems();
    }

    private void updateFilteredLogs()
    {
        filteredLogs.Clear();
        foreach (var log in allLogs)
        {
            if (shouldShowLog(log))
            {
                filteredLogs.Add(log);
            }
        }
    }

    private void updateContentHeight()
    {
        float totalItemHeight = (itemHeight + verticalPadding) * filteredLogs.Count;
        contentHeight = Mathf.Max(totalItemHeight + topBottomPadding * 2, scrollRect.viewport.rect.height);

        content.sizeDelta = new Vector2(getMaxWidth(), contentHeight); // Extra padding for safety
    }
    private float getWidth(string text)
    {
        return textGenerator.GetPreferredWidth(text, textGenerationSettings) / textGenerationSettings.scaleFactor;
    }
    private float getMaxWidth()
    {

        float longestWidth = scrollRect.viewport.rect.width;
        foreach (var log in filteredLogs)
        {
            if (log.Width == -1)
            {
                log.Width = getWidth(RemoveNewlines(log.FullText));
            }
            string singleLineText = RemoveNewlines(log.FullText);
            if (log.Width > longestWidth) longestWidth = log.Width;
        }
        return longestWidth + (longestWidth > scrollRect.viewport.rect.width - 200 ? 200 : 0);

    }
    private void updateVisibleItems()
    {
        foreach (var item in pooledItems)
        {
            item.gameObject.SetActive(false);
        }

        if (filteredLogs.Count == 0) return;

        float totalItemHeight = itemHeight + verticalPadding;
        float viewportHeight = scrollRect.viewport.rect.height;

        float contentTop = content.anchoredPosition.y;
        float contentBottom = contentTop + viewportHeight;

        int firstVisibleIndex = Mathf.FloorToInt((contentTop - topBottomPadding) / totalItemHeight);
        int lastVisibleIndex = Mathf.CeilToInt((contentBottom - topBottomPadding) / totalItemHeight);

        firstVisibleIndex = Mathf.Clamp(firstVisibleIndex, 0, filteredLogs.Count - 1);
        lastVisibleIndex = Mathf.Clamp(lastVisibleIndex, 0, filteredLogs.Count - 1);

        for (int i = firstVisibleIndex; i <= lastVisibleIndex; i++)
        {
            int poolIndex = i - firstVisibleIndex;
            if (poolIndex >= pooledItems.Count) continue;

            var item = pooledItems[poolIndex];
            var tmpText = item.GetComponentInChildren<Text>();
            var log = filteredLogs[i];

            tmpText.text = RemoveNewlines(log.FullText);
            item.sizeDelta = new Vector2(content.sizeDelta.x, itemHeight);

            float yPos = -topBottomPadding - (i * totalItemHeight);
            item.anchoredPosition = new Vector2(0, yPos);

            item.gameObject.SetActive(true);
        }
    }
    // Removes newlines from a string
    private string RemoveNewlines(string input)
    {
        return input.Replace("\n", " ").Replace("\r", " ");
    }

    private void scrollToBottom()
    {
        needsRefresh = true;
        if (filteredLogs.Count > 0)
        {
            scrollRect.verticalNormalizedPosition = 0;
        }
    }

    private void filterLogs(string filter)
    {
        resetFilterButton.gameObject.SetActive(!(filter == null || filter.Length == 0));
        currentFilter = filter;
        needsRefresh = true;
    }

    private void clearLogs()
    {
        allLogs.Clear();
        filteredLogs.Clear();
        needsRefresh = true;
    }

    private void copyLogs()
    {
        ShowClipboardNotification(GetMousePosition());
        StringBuilder sb = new StringBuilder();
        sb.Clear();
        foreach (var log in filteredLogs)
        {
            sb.AppendLine(stripRichText(log.FullText));
        }

#if UNITY_WEBGL && !UNITY_EDITOR
    CopyToClipboard(sb.ToString());
#else
        GUIUtility.systemCopyBuffer = sb.ToString();
#endif
        sb.Clear();
    }

    public static Vector2 GetMousePosition()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif
    }


    private string stripRichText(string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(input, "<.*?>", string.Empty);
    }

    private void closePanel()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    protected void OnDestroy()
    {
        Application.logMessageReceivedThreaded -= handleLog;
    }




    public void ShowClipboardNotification(Vector2 screenPosition, string message = "Copied to clipboard")
    {
        // Instantiate the full prefab
        GameObject instance = Instantiate(notificationPrefab, canvas.transform);
        instance.transform.position = screenPosition;
        instance.SetActive(true);
        // Set the message text
        Text text = instance.GetComponentInChildren<Text>();
        text.text = message;

        // Get CanvasGroup to control alpha
        CanvasGroup group = instance.GetComponent<CanvasGroup>();
        group.alpha = 0f;

        StartCoroutine(AnimateNotification(group, instance));
    }

    private IEnumerator AnimateNotification(CanvasGroup group, GameObject instance)
    {
        float time = 0f;

        // Fade in
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Clamp01(time / fadeDuration);
            yield return null;
        }

        yield return new WaitForSeconds(showDuration);

        // Fade out
        time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            group.alpha = 1f - Mathf.Clamp01(time / fadeDuration);
            yield return null;
        }

        Destroy(instance);
    }
}