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
    private Coroutine FadeInformationCoroutine;
    private void Awake()
    {
        _InformationProductionIconButton.onClick.AddListener(OnClickedProduceButton);
    }

    public void ShowInformation(ProductionModel _productionModel)
    {
        if (FadeInformationCoroutine != null)
            StopCoroutine(FadeInformationCoroutine);

        FadeInformationCoroutine = StartCoroutine(FadeInformationPanel(_productionModel, true));
    }

    public void HideInformation()
    {
        if (FadeInformationCoroutine != null)
            StopCoroutine(FadeInformationCoroutine);

        FadeInformationCoroutine = StartCoroutine(FadeInformationPanel(null, false));
    }

    public void OnClickedProduceButton()
    {

    }

    private IEnumerator FadeInformationPanel(ProductionModel _productionModel, bool _show)
    {
        if (_show)
        {
            _InformationItemNameText.text = _productionModel._Name;
            _InformationItemIconImage.sprite = Resources.Load<Sprite>(Constants.ProductionResourcePath + _productionModel._ProductionID);

            bool canProduct = _productionModel is BarrackModel;
            Debug.Log($"Can Produce: {canProduct}");
            _InformationProductionHolder.SetActive(canProduct);

            _InformationProductionText.text = "Production";
            _InformationProductionIcon.sprite = canProduct ? Resources.Load<Sprite>("") : null;
            
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

    private void OnDestroy()
    {
        _InformationProductionIconButton.onClick.RemoveAllListeners();
    }
}
