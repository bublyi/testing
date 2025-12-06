using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBuilder : MonoBehaviour
{
    public Sprite roundedSprite;
    private TextMeshProUGUI sizeDisplayText;
    private int currentSize = 5;
    private GameObject settingsPanel;
    private GameObject titleScreen;
    
    void Start()
    {
        Debug.Log("UIBuilder Start called!");
        CreateRoundedSprite();
        BuildUI();
    }
    
    void CreateRoundedSprite()
    {
        int size = 100;
        Texture2D tex = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];
        
        int radius = 20;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                bool isInside = true;
                
                if (x < radius && y < radius)
                    isInside = Mathf.Pow(x - radius, 2) + Mathf.Pow(y - radius, 2) < radius * radius;
                else if (x > size - radius && y < radius)
                    isInside = Mathf.Pow(x - (size - radius), 2) + Mathf.Pow(y - radius, 2) < radius * radius;
                else if (x < radius && y > size - radius)
                    isInside = Mathf.Pow(x - radius, 2) + Mathf.Pow(y - (size - radius), 2) < radius * radius;
                else if (x > size - radius && y > size - radius)
                    isInside = Mathf.Pow(x - (size - radius), 2) + Mathf.Pow(y - (size - radius), 2) < radius * radius;
                
                pixels[y * size + x] = isInside ? Color.white : Color.clear;
            }
        }
        
        tex.SetPixels(pixels);
        tex.Apply();
        
        roundedSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect, new Vector4(radius, radius, radius, radius));
    }
    
    void BuildUI()
    {
        GameObject canvasObj = new GameObject("MainCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.pixelPerfect = true;
        
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;
        scaler.referencePixelsPerUnit = 100;
        
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // Title Screen - matching your inspector settings
        titleScreen = new GameObject("TitleScreen");
        titleScreen.transform.SetParent(canvasObj.transform, false);
        RectTransform titleScreenRect = titleScreen.AddComponent<RectTransform>();
        titleScreenRect.anchorMin = new Vector2(0.5f, 0.5f);
        titleScreenRect.anchorMax = new Vector2(0.5f, 0.5f);
        titleScreenRect.pivot = new Vector2(0.5f, 0.5f);
        titleScreenRect.anchoredPosition = new Vector2(-500, -200);
        titleScreenRect.sizeDelta = new Vector2(950, 1200);
        titleScreenRect.localScale = new Vector3(0.5f, 0.5f, 1f);
        
        // Title "seethru"
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(titleScreen.transform, false);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchoredPosition = new Vector2(0, 200);
        titleRect.sizeDelta = new Vector2(800, 120);
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "seethru";
        titleText.fontSize = 80;
        titleText.color = Color.black;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.fontStyle = FontStyles.Bold;
        titleText.enableAutoSizing = false;
        
        // Subtitle
        GameObject subtitleObj = new GameObject("Subtitle");
        subtitleObj.transform.SetParent(titleScreen.transform, false);
        RectTransform subtitleRect = subtitleObj.AddComponent<RectTransform>();
        subtitleRect.anchoredPosition = new Vector2(0, 100);
        subtitleRect.sizeDelta = new Vector2(900, 60);
        TextMeshProUGUI subtitleText = subtitleObj.AddComponent<TextMeshProUGUI>();
        subtitleText.text = "Your AR-Powered Accessibility Reader";
        subtitleText.fontSize = 32;
        subtitleText.color = Color.black;
        subtitleText.alignment = TextAlignmentOptions.Center;
        subtitleText.enableAutoSizing = false;
        
        // Open Settings Button - BLUE
        GameObject openBtn = CreateRoundedButton(titleScreen.transform, "OpenSettingsButton", new Vector2(0, -100), new Vector2(600, 100), "Open Settings");
        openBtn.GetComponent<Image>().color = new Color(0.2f, 0.5f, 0.9f, 1f); // Blue
        openBtn.GetComponent<Button>().onClick.AddListener(OpenSettings);
        
        // Settings Panel - matching your inspector settings
        settingsPanel = new GameObject("SettingsPanel");
        settingsPanel.transform.SetParent(canvasObj.transform, false);
        RectTransform panelRect = settingsPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = new Vector2(-500, -200);
        panelRect.sizeDelta = new Vector2(950, 1200);
        panelRect.localScale = new Vector3(0.5f, 0.5f, 1f);
        
        Image panelImage = settingsPanel.AddComponent<Image>();
        panelImage.sprite = roundedSprite;
        panelImage.type = Image.Type.Sliced;
        panelImage.color = new Color(0.75f, 0.75f, 0.75f, 0.95f);
        
        GameObject closeBtn = CreateRoundedButton(settingsPanel.transform, "CloseMenuButton", new Vector2(0, 500), new Vector2(850, 100), "close menu >");
        closeBtn.GetComponent<Image>().color = new Color(0.65f, 0.65f, 0.65f, 1f);
        closeBtn.GetComponent<Button>().onClick.AddListener(CloseSettings);
        TextMeshProUGUI closeBtnText = closeBtn.GetComponentInChildren<TextMeshProUGUI>();
        closeBtnText.fontSize = 48;
        closeBtnText.enableAutoSizing = false;
        
        CreateLabel(settingsPanel.transform, "FontLabel", new Vector2(-320, 330), "font", 48);
        GameObject fontDropdown = CreateRoundedDropdown(settingsPanel.transform, "FontDropdown", new Vector2(130, 330), new Vector2(480, 80), 
            new string[] { "OpenDyslexic", "Arial", "Verdana" }, 1);
        fontDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener((value) => {
            Debug.Log("Font changed to: " + fontDropdown.GetComponent<TMP_Dropdown>().options[value].text);
        });
        
        CreateLabel(settingsPanel.transform, "SizeLabel", new Vector2(-320, 180), "size", 48);
        
        GameObject minusBtn = CreateRoundedButton(settingsPanel.transform, "SizeMinusButton", new Vector2(-20, 180), new Vector2(100, 80), "-");
        minusBtn.GetComponent<Image>().color = new Color(0.88f, 0.88f, 0.88f, 1f);
        minusBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        minusBtn.GetComponentInChildren<TextMeshProUGUI>().fontSize = 56;
        minusBtn.GetComponent<Button>().onClick.AddListener(DecreaseSize);
        
        GameObject sizeDisplay = CreateRoundedButton(settingsPanel.transform, "SizeDisplay", new Vector2(130, 180), new Vector2(100, 80), "5");
        sizeDisplay.GetComponent<Image>().color = Color.white;
        sizeDisplayText = sizeDisplay.GetComponentInChildren<TextMeshProUGUI>();
        sizeDisplayText.color = Color.black;
        sizeDisplayText.fontSize = 48;
        sizeDisplay.GetComponent<Button>().interactable = false;
        
        GameObject plusBtn = CreateRoundedButton(settingsPanel.transform, "SizePlusButton", new Vector2(280, 180), new Vector2(100, 80), "+");
        plusBtn.GetComponent<Image>().color = new Color(0.88f, 0.88f, 0.88f, 1f);
        plusBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        plusBtn.GetComponentInChildren<TextMeshProUGUI>().fontSize = 56;
        plusBtn.GetComponent<Button>().onClick.AddListener(IncreaseSize);
        
        CreateLabel(settingsPanel.transform, "BoldingLabel", new Vector2(-320, 30), "bolding", 48);
        GameObject boldingDropdown = CreateRoundedDropdown(settingsPanel.transform, "BoldingDropdown", new Vector2(130, 30), new Vector2(480, 80), 
            new string[] { "first letter", "none", "all" }, 0);
        boldingDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener((value) => {
            Debug.Log("Bolding changed to: " + boldingDropdown.GetComponent<TMP_Dropdown>().options[value].text);
        });
        
        CreateLabel(settingsPanel.transform, "SummarizerLabel", new Vector2(-320, -120), "summarizer", 48);
        GameObject summarizerDropdown = CreateRoundedDropdown(settingsPanel.transform, "SummarizerDropdown", new Vector2(130, -120), new Vector2(480, 80), 
            new string[] { "on", "off" }, 0);
        summarizerDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener((value) => {
            Debug.Log("Summarizer changed to: " + summarizerDropdown.GetComponent<TMP_Dropdown>().options[value].text);
        });
        
        CreateLabel(settingsPanel.transform, "ContrastLabel", new Vector2(-320, -270), "high contrast", 48);
        GameObject contrastDropdown = CreateRoundedDropdown(settingsPanel.transform, "ContrastDropdown", new Vector2(130, -270), new Vector2(480, 80), 
            new string[] { "on", "off" }, 0);
        contrastDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener((value) => {
            Debug.Log("Contrast changed to: " + contrastDropdown.GetComponent<TMP_Dropdown>().options[value].text);
        });
        
        // Start with title screen visible, settings hidden
        titleScreen.SetActive(true);
        settingsPanel.SetActive(false);
    }
    
    void OpenSettings()
    {
        titleScreen.SetActive(false);
        settingsPanel.SetActive(true);
        Debug.Log("Settings opened");
    }
    
    void CloseSettings()
    {
        settingsPanel.SetActive(false);
        titleScreen.SetActive(true);
        Debug.Log("Settings closed");
    }
    
    void IncreaseSize()
    {
        if (currentSize < 10)
        {
            currentSize++;
            sizeDisplayText.text = currentSize.ToString();
            Debug.Log("Size increased to: " + currentSize);
        }
    }
    
    void DecreaseSize()
    {
        if (currentSize > 1)
        {
            currentSize--;
            sizeDisplayText.text = currentSize.ToString();
            Debug.Log("Size decreased to: " + currentSize);
        }
    }
    
    GameObject CreateLabel(Transform parent, string name, Vector2 position, string text, float fontSize)
    {
        GameObject labelObj = new GameObject(name);
        labelObj.transform.SetParent(parent, false);
        RectTransform rect = labelObj.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(380, 80);
        TextMeshProUGUI labelText = labelObj.AddComponent<TextMeshProUGUI>();
        labelText.text = text;
        labelText.fontSize = fontSize;
        labelText.color = Color.white;
        labelText.alignment = TextAlignmentOptions.MidlineLeft;
        labelText.fontStyle = FontStyles.Bold;
        labelText.enableAutoSizing = false;
        return labelObj;
    }
    
    GameObject CreateRoundedButton(Transform parent, string name, Vector2 position, Vector2 size, string text)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent, false);
        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        
        Image btnImage = buttonObj.AddComponent<Image>();
        btnImage.sprite = roundedSprite;
        btnImage.type = Image.Type.Sliced;
        btnImage.color = new Color(0.9f, 0.9f, 0.9f, 1f);
        
        Button btn = buttonObj.AddComponent<Button>();
        
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        TextMeshProUGUI btnText = textObj.AddComponent<TextMeshProUGUI>();
        btnText.text = text;
        btnText.fontSize = 42;
        btnText.color = Color.white;
        btnText.alignment = TextAlignmentOptions.Center;
        btnText.fontStyle = FontStyles.Bold;
        btnText.enableAutoSizing = false;
        
        return buttonObj;
    }
    
    GameObject CreateRoundedDropdown(Transform parent, string name, Vector2 position, Vector2 size, string[] options, int defaultIndex)
    {
        GameObject dropdownObj = new GameObject(name);
        dropdownObj.transform.SetParent(parent, false);
        RectTransform rect = dropdownObj.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        
        Image dropImage = dropdownObj.AddComponent<Image>();
        dropImage.sprite = roundedSprite;
        dropImage.type = Image.Type.Sliced;
        dropImage.color = Color.white;
        
        TMP_Dropdown dropdown = dropdownObj.AddComponent<TMP_Dropdown>();
        
        GameObject labelObj = new GameObject("Label");
        labelObj.transform.SetParent(dropdownObj.transform, false);
        RectTransform labelRect = labelObj.AddComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = new Vector2(30, 10);
        labelRect.offsetMax = new Vector2(-80, -10);
        TextMeshProUGUI labelText = labelObj.AddComponent<TextMeshProUGUI>();
        labelText.fontSize = 40;
        labelText.color = Color.black;
        labelText.alignment = TextAlignmentOptions.MidlineLeft;
        labelText.enableAutoSizing = false;
        
        GameObject arrowObj = new GameObject("Arrow");
        arrowObj.transform.SetParent(dropdownObj.transform, false);
        RectTransform arrowRect = arrowObj.AddComponent<RectTransform>();
        arrowRect.anchorMin = new Vector2(1, 0.5f);
        arrowRect.anchorMax = new Vector2(1, 0.5f);
        arrowRect.pivot = new Vector2(0.5f, 0.5f);
        arrowRect.sizeDelta = new Vector2(50, 50);
        arrowRect.anchoredPosition = new Vector2(-40, 0);
        TextMeshProUGUI arrowText = arrowObj.AddComponent<TextMeshProUGUI>();
        arrowText.text = "▼";
        arrowText.fontSize = 36;
        arrowText.color = Color.black;
        arrowText.alignment = TextAlignmentOptions.Center;
        arrowText.enableAutoSizing = false;
        
        GameObject templateObj = new GameObject("Template");
        templateObj.transform.SetParent(dropdownObj.transform, false);
        RectTransform templateRect = templateObj.AddComponent<RectTransform>();
        templateRect.anchorMin = new Vector2(0, 0);
        templateRect.anchorMax = new Vector2(1, 0);
        templateRect.pivot = new Vector2(0.5f, 1);
        templateRect.anchoredPosition = new Vector2(0, 0);
        templateRect.sizeDelta = new Vector2(0, 250);
        
        Image templateImage = templateObj.AddComponent<Image>();
        templateImage.sprite = roundedSprite;
        templateImage.type = Image.Type.Sliced;
        templateImage.color = new Color(0.95f, 0.95f, 0.95f, 1f);
        
        ScrollRect scrollRect = templateObj.AddComponent<ScrollRect>();
        scrollRect.horizontal = false;
        scrollRect.vertical = true;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;
        
        GameObject viewportObj = new GameObject("Viewport");
        viewportObj.transform.SetParent(templateObj.transform, false);
        RectTransform viewportRect = viewportObj.AddComponent<RectTransform>();
        viewportRect.anchorMin = new Vector2(0.05f, 0.05f);
        viewportRect.anchorMax = new Vector2(0.95f, 0.95f);
        viewportRect.offsetMin = Vector2.zero;
        viewportRect.offsetMax = Vector2.zero;
        Image viewportImage = viewportObj.AddComponent<Image>();
        viewportImage.color = Color.clear;
        Mask viewportMask = viewportObj.AddComponent<Mask>();
        viewportMask.showMaskGraphic = false;
        
        GameObject contentObj = new GameObject("Content");
        contentObj.transform.SetParent(viewportObj.transform, false);
        RectTransform contentRect = contentObj.AddComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0, 1);
        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.pivot = new Vector2(0.5f, 1);
        contentRect.anchoredPosition = Vector2.zero;
        contentRect.sizeDelta = new Vector2(0, options.Length * 75);
        
        GameObject itemObj = new GameObject("Item");
        itemObj.transform.SetParent(contentObj.transform, false);
        RectTransform itemRect = itemObj.AddComponent<RectTransform>();
        itemRect.anchorMin = new Vector2(0, 1);
        itemRect.anchorMax = new Vector2(1, 1);
        itemRect.pivot = new Vector2(0.5f, 1);
        itemRect.sizeDelta = new Vector2(0, 75);
        
        Toggle itemToggle = itemObj.AddComponent<Toggle>();
        Image itemBg = itemObj.AddComponent<Image>();
        itemBg.color = new Color(0.9f, 0.9f, 0.9f, 1f);
        itemToggle.targetGraphic = itemBg;
        
        ColorBlock colors = itemToggle.colors;
        colors.normalColor = new Color(0.9f, 0.9f, 0.9f, 1f);
        colors.highlightedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
        colors.pressedColor = new Color(0.7f, 0.7f, 0.7f, 1f);
        colors.selectedColor = new Color(0.85f, 0.85f, 0.85f, 1f);
        itemToggle.colors = colors;
        
        GameObject itemBackgroundObj = new GameObject("Item Background");
        itemBackgroundObj.transform.SetParent(itemObj.transform, false);
        RectTransform itemBgRect = itemBackgroundObj.AddComponent<RectTransform>();
        itemBgRect.anchorMin = Vector2.zero;
        itemBgRect.anchorMax = Vector2.one;
        itemBgRect.sizeDelta = Vector2.zero;
        Image itemBgImage = itemBackgroundObj.AddComponent<Image>();
        itemBgImage.color = Color.clear;
        
        GameObject checkmarkObj = new GameObject("Item Checkmark");
        checkmarkObj.transform.SetParent(itemObj.transform, false);
        RectTransform checkmarkRect = checkmarkObj.AddComponent<RectTransform>();
        checkmarkRect.anchorMin = new Vector2(0, 0.5f);
        checkmarkRect.anchorMax = new Vector2(0, 0.5f);
        checkmarkRect.pivot = new Vector2(0.5f, 0.5f);
        checkmarkRect.anchoredPosition = new Vector2(35, 0);
        checkmarkRect.sizeDelta = new Vector2(40, 40);
        Image checkmarkImage = checkmarkObj.AddComponent<Image>();
        checkmarkImage.color = new Color(0.2f, 0.6f, 0.2f, 1f);
        
        GameObject itemLabelObj = new GameObject("Item Label");
        itemLabelObj.transform.SetParent(itemObj.transform, false);
        RectTransform itemLabelRect = itemLabelObj.AddComponent<RectTransform>();
        itemLabelRect.anchorMin = Vector2.zero;
        itemLabelRect.anchorMax = Vector2.one;
        itemLabelRect.offsetMin = new Vector2(60, 10);
        itemLabelRect.offsetMax = new Vector2(-30, -10);
        TextMeshProUGUI itemLabelText = itemLabelObj.AddComponent<TextMeshProUGUI>();
        itemLabelText.fontSize = 36;
        itemLabelText.color = Color.black;
        itemLabelText.alignment = TextAlignmentOptions.MidlineLeft;
        itemLabelText.enableAutoSizing = false;
        
        itemToggle.graphic = checkmarkImage;
        
        scrollRect.content = contentRect;
        scrollRect.viewport = viewportRect;
        
        dropdown.template = templateRect;
        dropdown.captionText = labelText;
        dropdown.itemText = itemLabelText;
        
        dropdown.ClearOptions();
        dropdown.AddOptions(new System.Collections.Generic.List<string>(options));
        dropdown.value = defaultIndex;
        dropdown.RefreshShownValue();
        
        templateObj.SetActive(false);
        
        return dropdownObj;
    }
}