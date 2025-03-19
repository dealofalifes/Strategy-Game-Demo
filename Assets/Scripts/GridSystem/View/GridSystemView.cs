using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSystemView : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _GridLayout;

    [SerializeField] private Transform _GridElementsContent;

    public void CreateGridMap(GridElementView[,] _gridElements, Vector2Int _cellSize)
    {
        int x_Length = _gridElements.GetLength(0);
        int y_Length = _gridElements.GetLength(1);
        _GridLayout.constraintCount = x_Length;
        _GridLayout.cellSize = _cellSize;

        int length = _GridElementsContent.childCount;
        for (int i = 0; i < length - 1; i++) //Length - 1, Because the last element is Container for Buildings.
            _GridElementsContent.GetChild(i).gameObject.SetActive(false);

        GameObject gridElementPrefab = Resources.Load<GameObject>(Constants.GridElementResourcePath);

        int _elementsCountFromPool = _GridElementsContent.childCount;
        int _totalElementsCount = x_Length * y_Length;

        int _neededElements = _totalElementsCount - _elementsCountFromPool;

        for (int i = 0; i < _neededElements; i++)
        {
            GameObject newElement = Instantiate(gridElementPrefab, _GridElementsContent);
            newElement.SetActive(false);
        }

        int index = 0;
        for (int y = 0; y < y_Length; y++)
        {
            for (int x = 0; x < x_Length; x++)
            {
                GameObject currentElement = _GridElementsContent.GetChild(index).gameObject;

                if (!currentElement.TryGetComponent(out GridElementView currentElementView))
                    currentElementView = currentElement.AddComponent<GridElementView>();

                _gridElements[x, y] = currentElementView;

                GridElementModel currentModel = new(new Vector2Int(x, y));
                currentElementView.SetData(currentModel);

                currentElement.SetActive(true);
                index++;
            }
        }
    }

    public void SetGridElementViewState(GridElementView _elementView, GridElementState _elementState)
    {
        _elementView.SetNewState(_elementState);
    }

    public void ResetBuildingStates()
    {
        int length = _GridElementsContent.childCount;
        for (int i = 0; i < length - 1; i++) //Length - 1, Because the last element is Container for Buildings.
        {
            GridElementView _currentElementView = _GridElementsContent.GetChild(i).GetComponent<GridElementView>();
            _currentElementView.EndState(GridElementState.Buildable);
            _currentElementView.EndState(GridElementState.NotBuildable);
            _currentElementView.EndState(GridElementState.BarrackDoor);
        }
    }
}
