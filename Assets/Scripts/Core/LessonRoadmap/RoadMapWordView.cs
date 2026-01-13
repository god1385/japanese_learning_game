using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoadMapWordView : MonoBehaviour
{
    [SerializeField] private RoadMapSlotView slotPrefab;
    [SerializeField] private Transform slotsRoot;
    [SerializeField] private Image resultImage;
    [SerializeField] private TextMeshProUGUI indexText;
    [SerializeField] private LayoutElement equalSignText;
    [SerializeField] private LayoutElement reulstImageLayout;
    [SerializeField] private float minSizeForElements = 25f;
    [SerializeField] private float maxSizeForElements = 50f;

    private readonly List<RoadMapSlotView> _slots = new();
    private WordData _levelData;
    private int _filledCount = 0;
    private bool _isVisible = false;

    public bool IsCompleted => _filledCount == _slots.Count;

    public void Initialize(WordData data, int index)
    {
        _levelData = data;
        resultImage.sprite = _levelData.placeholderSprite ?? _levelData.resultImage;
        resultImage.gameObject.SetActive(false);
        indexText.text = (index + 1).ToString();


        for (int i = 0; i < _levelData.symbols.Count; i++)
        {
            var slot = Instantiate(slotPrefab, slotsRoot);
            slot.transform.SetSiblingIndex(i);
            slot.Initialize(_levelData.symbols[i]);
            _slots.Add(slot);
        }

        AdjustSlotSizes(slotsRoot.GetComponent<HorizontalLayoutGroup>());

        _isVisible = !_levelData.hideUntilTutorialStep;
        gameObject.SetActive(_isVisible);
    }
    public void RevealWord()
    {
        if (_isVisible) return;

        _isVisible = true;
        gameObject.SetActive(true);
    }


    public void AdjustSlotSizes(HorizontalLayoutGroup layoutGroup)
    {
        int symbolCount = _slots.Count;
        if (symbolCount == 0) return;

        float cellWidth = _slots.Count > 10f ? minSizeForElements : maxSizeForElements;

        foreach (var slot in _slots)
        {
            var le = slot.GetComponent<LayoutElement>();
            le.preferredWidth = cellWidth;
            le.preferredHeight = cellWidth;
        }

        equalSignText.preferredHeight = cellWidth;
        equalSignText.preferredWidth = cellWidth;
        reulstImageLayout.preferredHeight = cellWidth + 10;
        reulstImageLayout.preferredWidth = cellWidth + 10;
    }

    public async Task TryAddSymbol(SymbolData symbol)
    {
        if (IsCompleted || !_levelData.symbols.Contains(symbol))
            return;

        bool filledAny = false;

        var tasks = new List<Task<bool>>();

        foreach (var slot in _slots)
            tasks.Add(slot.PlayFill(symbol));

        var results = await Task.WhenAll(tasks);

        foreach (var filled in results)
        {
            if (filled)
            {
                _filledCount++;
                filledAny = true;
            }
        }

        _filledCount = Mathf.Min(_filledCount, _slots.Count);

        if (IsCompleted && filledAny)
            await ShowResult();
    }

    private async Task ShowResult()
    {
        resultImage.sprite = _levelData.resultImage;
        resultImage.gameObject.SetActive(true);
        await Task.Delay(TimeSpan.FromSeconds(1));

    }
}
