using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionView : MonoBehaviour
{
    [SerializeField] private Transform _ProductionElementsContent;

    //Actions
    public event Action<List<ProductionElementView>> OnElementCreated;
    public event Action OnCancelled;

    public void CreateProductions(List<ProductionModel> _productions)
    {
        List<ProductionElementView> elements = new();

        foreach (Transform _element in _ProductionElementsContent)
            _element.gameObject.SetActive(false);

        GameObject productionElementPrefab = Resources.Load<GameObject>(Constants.ProductionElementResourcePath);

        int _elementsCountFromPool = _ProductionElementsContent.childCount;
        int _totalElementsCount = _productions.Count;

        int _neededElements = _totalElementsCount - _elementsCountFromPool;

        for (int i = 0; i < _neededElements; i++)
        {
            GameObject newElement = Instantiate(productionElementPrefab, _ProductionElementsContent);
            newElement.SetActive(false);
        }

        for (int i = 0; i < _totalElementsCount; i++)
        {
            GameObject currentElement = _ProductionElementsContent.GetChild(i).gameObject;

            if (!currentElement.TryGetComponent(out ProductionElementView currentElementView))
                currentElementView = currentElement.AddComponent<ProductionElementView>();

            currentElementView.SetData(_productions[i]);
            currentElement.SetActive(true);

            elements.Add(currentElementView);
        }

        OnElementCreated?.Invoke(elements);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            OnCancelled.Invoke();
    }

    public ProductionElementView GetProductionElementViewByProductionID(int _productionID)
    {
        foreach (Transform item in _ProductionElementsContent)
        {
            ProductionElementView currentElement = item.GetComponent<ProductionElementView>();
            if (currentElement.GetProductionID() == _productionID)
            {
                return currentElement;
            }
        }

        return null;
    }

    public ProductionElementView GetProductionElementViewByIndex(int _index)
    {
        return _ProductionElementsContent.GetChild(_index).GetComponent<ProductionElementView>();
    }

    public ProductionModel GetProductionModelByID(int _productionID)
    {
        foreach (Transform item in _ProductionElementsContent)
        {
            ProductionElementView currentElement = item.GetComponent<ProductionElementView>();
            if (currentElement.GetProductionID() == _productionID)
            {
                return currentElement.GetData();
            }
        }

        return null;
    }
}
