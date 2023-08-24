public class MachineButton : BuildButton
{
    public override void LoadButton()
    {
        base.LoadButton();

        button.onClick.AddListener(() => GameManager.instance.placementManager.StartPlacement(machineIndex));
    }
}
