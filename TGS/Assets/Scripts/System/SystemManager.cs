using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UdonCommons;
using SystemParameter;
using UdonObservable.Commons;

public class SystemManager : UdonBehaviourSingleton<SystemManager> {

    [SerializeField]
    private List<GameObject> BattlePlayingGamePrefab;

    [SerializeField]
    private List<GameObject> TeamPlayingGamePrefab;

    [SerializeField]
    private List<GameObject> StageManagerPrefab;

    [SerializeField]
    private List<GameObject> BattleResultScenePrefabs;

    [SerializeField]
    private List<GameObject> TeamResultScenePrefabs;

    [SerializeField]
    private SystemPresenter Presenter;

    [SerializeField]
    private SceneController SceneCanvas;

    private GameObject PlayingGameObject;

    private GameObject ResultGameObject;

    private GameObject StageManagerObject;

    private int _playerNum = 0;

    public int PlayerNum
        {
            get
            {
                return _playerNum;
            }
        }

    private GameEnum.gameType _gameType = GameEnum.gameType.team;

    public GameEnum.gameType GameType
    {
        get{
            return _gameType;
        }
    }

    private int _stageNum = 1;

    public int StageNum
    {
        get
        {
            return _stageNum;
        }
    }

    public void SetPlayerNum(int num)
    {
        _playerNum = num;
    }

    public void SetGameType(GameEnum.gameType type)
    {
        _gameType = type;
    }

    public void SetStageNum(int num)
    {
        _stageNum = num;
    }

    private bool _createdGame;
    private bool _finishedGame;
    private bool _isGame;

    public bool IsGame
    {
        get
        {
            return _isGame;
        }
    }

    private bool _isPause;

    public bool IsPause
    {
        get
        {
            return _isPause;
        }
    }

    public bool CreateGame
    {
        get
        {
            return _createdGame;
        }
    }

    protected override void Start()
    {
        AudioManager.Instance.PlayBGM(GameEnum.BGM.title);
    }

    public void GameStart()
    {
        if (_createdGame == false)
        {
            StartCoroutine(OnGameStartCoroutine());
        }
    }

    public void GameEnd()
    {
        StartCoroutine(OnGameEndCoroutine());
    }

    public void BackTitle()
    {
        if (_finishedGame == false)
        {
            StartCoroutine(OnBackTitleCoroutine());
        }
    }

    private IEnumerator OnGameStartCoroutine()
    {
        _createdGame = true;
        _isGame = false;
        _finishedGame = false;
        CharacterManager.Instance.InitCharacterList();
        SceneCanvas.GameSceneTranslate(() => { CreatePlayingObjects();  DestroyResultObjects();});

        yield return new WaitUntil(() => SceneCanvas.isPlayingGame);

        SceneCanvas.BattleStart();
        Presenter.Init();
        AudioManager.Instance.PlaySystemSE(GameEnum.SE.start);
        _isPause = false;
        _isGame = true;

        yield break;
    }

    private IEnumerator OnGameEndCoroutine()
    {
        _isGame = false;
        _createdGame = false;
        AudioManager.Instance.PlaySystemSE(GameEnum.SE.end);
        SceneCanvas.BattleEnd();

        yield return new WaitUntil(() => SceneCanvas.isEndingGame);
        yield return new WaitWhile(() => SceneCanvas.isEndingGame);

        SceneCanvas.ResultSceneTranslate(() => { DestroyPlayingObjects(); CreateResultPlayingObject(); });
        AudioManager.Instance.ResetPlayerSource();

        yield break;
    }

    private IEnumerator OnBackTitleCoroutine()
    {
        _finishedGame = true;
        SceneCanvas.TitleSceneTranslate(() => DestroyResultObjects());
        yield break;
    }

    private void CreatePlayingObjects()
    {
        var playObjectList = _gameType == GameEnum.gameType.team ? TeamPlayingGamePrefab : BattlePlayingGamePrefab;
        var playObjectIndex = _playerNum == 0 ? _playerNum : GameValue.MAX_PLAYER_NUM - _playerNum;
        PlayingGameObject = Instantiate(playObjectList.ElementAt(playObjectIndex), transform);
        StageManagerObject = Instantiate(StageManagerPrefab.ElementAt(_stageNum - 1), transform);
    }

    private void DestroyPlayingObjects()
    {
        Destroy(PlayingGameObject);
        Destroy(StageManagerObject);
    }

    private void CreateResultPlayingObject()
    {
        var resultObjectList = _gameType == GameEnum.gameType.team ? TeamResultScenePrefabs : BattleResultScenePrefabs;
        ResultGameObject = Instantiate(resultObjectList.ElementAt(GameValue.MAX_PLAYER_NUM - PlayerNum), transform);
    }

    private void DestroyResultObjects()
    {
        Destroy(ResultGameObject);
    }

    public class SystemModel
    {
        public SystemModel(int time)
        {
            _timer = ReactiveTimer.ReactiveTimerForSeconds(time);
        }

        private ReactiveProperty<int> _timer;
        public IReadOnlyReactiveProperty<int> Timer
        {
            get
            {
                if (_timer == null)
                {
                    InstantLog.StringLogError("timer is null");
                    _timer = new ReactiveProperty<int>(GameValue.BATTLE_TIME);
                }

                return _timer;
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }

    public SystemModel Model
    {
        get
        {
            if(_model == null)
            {
                _model = new SystemModel(0);
            }

            return _model;
        }
    }

    private SystemModel _model;

    public SystemModel InitModel()
    {
       return _model = new SystemModel(GameValue.BATTLE_TIME);
    }

    public int Time
    {
        get
        {
            if(_model == null)
            {
                _model = new SystemModel(0);
                return 0;
            }
            else
            {
                return _model.Timer.Value;
            }
        }
    }

    public void SetPause(bool pause)
    {
        _isPause = pause;
    }

}
