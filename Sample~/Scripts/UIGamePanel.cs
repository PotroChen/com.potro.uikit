using UnityEngine;
using UnityEngine.UI;
using GameFramework.UIKit;

public class UIGamePanel : UIPanel
{
    protected override void Init()
    {
        base.Init();

        Transform uiMainPanel = transform.Find("Button");
        uiMainPanel.GetComponent<Button>().onClick.AddListener(() => { UIPanel.OpenPanel<UIMainPanel>(); ClosePanel(); });
    }
}
