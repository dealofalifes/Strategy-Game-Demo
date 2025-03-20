using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationView : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup _InformationDetailsCanvasGroup;
    [SerializeField] private Text _InformationItemNameText;
    [SerializeField] private Image _InformationItemIconImage;

    [SerializeField] private GameObject _InformationProductionHolder;
    [SerializeField] private Text _InformationProductionText;
    [SerializeField] private Image _InformationProductionIcon;
    [SerializeField] private Button _InformationProductionIconButton;

    [Header("DEBUG")]
    [SerializeField] private BuildingElementView _CurrentSelectedBarrackView;
    private Coroutine FadeInformationCoroutine;

    
    public event Action OnProduceButtonClicked;

    private void Awake()
    {
        _InformationProductionIconButton.onClick.AddListener(OnClickedProduceButton);
    }

    public Button GetProduceButton()
    {
        return _InformationProductionIconButton;
    }

    public void ShowInformation(ProductionModel _productionModel)
    {
        if (FadeInformationCoroutine != null)
            StopCoroutine(FadeInformationCoroutine);

        FadeInformationCoroutine = StartCoroutine(FadeInformationPanel(_productionModel, true));
    }

    public void ShowInformation(UnitModel _unitModel)
    {
        if (FadeInformationCoroutine != null)
            StopCoroutine(FadeInformationCoroutine);

        FadeInformationCoroutine = StartCoroutine(FadeInformationPanel(_unitModel, true));
    }

    public void ShowInformation(BuildingElementView _productionElementView, ProductionModel _productionModel)
    {
        if (FadeInformationCoroutine != null)
            StopCoroutine(FadeInformationCoroutine);

        _CurrentSelectedBarrackView = _productionElementView;
        
        if(_productionModel != null)
            FadeInformationCoroutine = StartCoroutine(FadeInformationPanel(_productionModel, true));
    }

    public void HideInformation()
    {
        if (FadeInformationCoroutine != null)
            StopCoroutine(FadeInformationCoroutine);

        _CurrentSelectedBarrackView = null;
        FadeInformationCoroutine = StartCoroutine(FadeInformationPanel(new ProductionModel(), false));
    }

    public void OnClickedProduceButton()
    {
        OnProduceButtonClicked.Invoke();
    }

    public BuildingElementView GetCurrentSelectedBarrack()
    {
        return _CurrentSelectedBarrackView;
    } 

    private IEnumerator FadeInformationPanel(ProductionModel _productionModel, bool _show)
    {
        if (_show)
        {
            _InformationItemNameText.text = _productionModel._Name;
            _InformationItemIconImage.sprite = Resources.Load<Sprite>(Constants.ProductionResourcePath + _productionModel._ProductionID);

            bool canProduct = false;
            if (_productionModel is BarrackModel barrackModel)
            {
                canProduct = true;
                _InformationProductionText.text = "Production";
                _InformationProductionIcon.sprite = canProduct ? Resources.Load<Sprite>(Constants.UnitResourcePath + barrackModel._SoldierID) : null;
            }

            _InformationProductionHolder.SetActive(canProduct);

            _InformationProductionIconButton.interactable = true;
        }

        float duration = 0.3f; // Fade length in seconds
        float startAlpha = _show ? 0f : 1f;
        float targetAlpha = _show ? 1f : 0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            _InformationDetailsCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _InformationDetailsCanvasGroup.alpha = targetAlpha;
    }

    private IEnumerator FadeInformationPanel(UnitModel _unitModel, bool _show)
    {
        if (_show)
        {
            _InformationItemNameText.text = _unitModel.UnitName;
            _InformationItemIconImage.sprite = Resources.Load<Sprite>(Constants.UnitResourcePath + _unitModel.UnitID);
            _InformationProductionHolder.SetActive(false);
        }

        float duration = 0.3f; // Fade length in seconds
        float startAlpha = _show ? 0f : 1f;
        float targetAlpha = _show ? 1f : 0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            _InformationDetailsCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _InformationDetailsCanvasGroup.alpha = targetAlpha;
    }

    private void OnDestroy()
    {
        _InformationProductionIconButton.onClick.RemoveAllListeners();
    }
}
