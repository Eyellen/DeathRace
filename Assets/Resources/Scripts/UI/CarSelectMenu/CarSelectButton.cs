using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private uint _carIndex;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CarSelectPreviewManager.Instance.ShowCar(_carIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CarSelectPreviewManager.Instance.HideCar(_carIndex);
    }

    public void OnDisable()
    {
        CarSelectPreviewManager.Instance.HideCar(_carIndex);
    }

    public void SelectCar()
    {
        Player.LocalPlayer.CmdSetSelectedCarIndex((int)_carIndex);
        CarSelectPreviewManager.Instance.SelectedCarIndex = _carIndex;
    }
}
