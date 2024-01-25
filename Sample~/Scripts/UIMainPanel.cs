using UnityEngine;
using UnityEngine.UI;
using GameFramework.UIKit;

public class UIMainPanel : UIPanel
{
    protected override void Init()
    {
        base.Init();

        Transform uiGamePanel = transform.Find("Button");
        uiGamePanel.GetComponent<Button>().onClick.AddListener(() => { UIPanel.OpenPanel<UIGamePanel>(); ClosePanel(); });
    }

}
