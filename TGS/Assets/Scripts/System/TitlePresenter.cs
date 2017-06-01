using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UdonObservable.InputRx.GamePad;
using SystemParameter;

public class TitlePresenter : MonoBehaviour
{

    [SerializeField]
    private TitleController _model;

    [SerializeField]
    private TitleView _view;

    public void Init()
    {
        _model.Init();
        SetEvents();
        ObserveTitleScene();
    }

    public void Dispose()
    {
        _model.Dispose();
    }

    private void ObserveTitleScene()
    {
        _model.PlayerNum
            .Skip(1)
            .Subscribe(x => _view.OnPlayerNumChanged(x));

        _model.StageIndex
            .Skip(1)
            .Subscribe(x => _view.OnStageIndexChanged(x));

        _model.ArrowPos
            .Skip(1)
            .Subscribe(x => _view.OnArrowPosChanged(x));

        _model.Mode
            .Skip(1)
            .Subscribe(x => _view.OnModeChanged(x));
    }

    private void SetEvents()
    {
        _view.OnPlayerNumChangedListener = OnPlayerNumChanged;
        _view.OnStageIndexChangedListener = OnStageIndexChanged;
        _view.OnArrowPosChangedListener = OnArrowPosChanged;
        _view.OnModeChangedListener = OnModeChanged;
    }

    public void OnPlayerNumChanged()
    {
        AudioManager.Instance.PlaySystemSE(GameEnum.SE.cursor);
    }

    public void OnStageIndexChanged()
    {
        AudioManager.Instance.PlaySystemSE(GameEnum.SE.cursor);
    }

    public void OnArrowPosChanged()
    {
        AudioManager.Instance.PlaySystemSE(GameEnum.SE.cursor);
    }

    public void OnModeChanged()
    {

    }
}
