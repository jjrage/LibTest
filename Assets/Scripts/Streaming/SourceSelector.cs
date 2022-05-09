using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Klak.Ndi;
using static UnityEngine.UI.Dropdown;
using System.Collections;

public class SourceSelector : MonoBehaviour
{
    [SerializeField] Dropdown _dropdown = null;
    [SerializeField] Button _refreshSourceButton = null;
    NdiReceiver _receiver;
    List<string> _sourceNames;

    void Start()
    {
        _receiver = GetComponent<NdiReceiver>();
        _dropdown.onValueChanged.AddListener(OnChangeValue);
        _refreshSourceButton.onClick.AddListener(RefreshSoruces);
        StartCoroutine(InitSources());
    }

    private IEnumerator InitSources()
    {
        yield return new WaitForSeconds(1);
        RefreshSoruces();
    }

    private void RefreshSoruces()
    {
        _dropdown.ClearOptions();
        // NDI source name retrieval
        _sourceNames = NdiFinder.sourceNames.ToList();

        // Currect selection
        var index = _sourceNames.IndexOf(_receiver.ndiName);

        // Append the current name to the list if it's not found.
        if (index < 0)
        {
            index = _sourceNames.Count;
            _sourceNames.Add(_receiver.ndiName);
        }

        List<OptionData> sourceOptions = new List<OptionData>();

        foreach (var source in _sourceNames)
        {
            if (!string.IsNullOrEmpty(source))
            {
                // Menu option update
                OptionData optionData = new OptionData(source);
                sourceOptions.Add(optionData);
            }
        }

        _dropdown.AddOptions(sourceOptions);
    }

    public void OnChangeValue(int value)
    {      
        _receiver.ndiName = _sourceNames[value];
    }
}