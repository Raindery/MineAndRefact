using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MineAndRefact.Core
{
    public sealed class Tutorial : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private GameplayEventListener _gameEventListener;
        [Header("First Step")]
        [SerializeField] private BaseSource _source;
        [Header("Second Step")]
        [SerializeField] private BaseSpot _spot;

        private Queue<BaseTutorialStep> _tutorialSteps;


        private void Awake()
        {
            if (_gameEventListener == null)
                throw new System.ArgumentNullException(nameof(_gameEventListener));

            _tutorialSteps = new Queue<BaseTutorialStep>();
        }

        private void Start()
        {
            _tutorialSteps.Enqueue(new FirstTutorialStep(_gameEventListener, _source));
            _tutorialSteps.Enqueue(new SecondTutorialStep(_gameEventListener, _spot));

            StartCoroutine(TutorialCoroutine());
        }


        private IEnumerator TutorialCoroutine()
        {
            while(_tutorialSteps.TryDequeue(out BaseTutorialStep tutorialStep))
            {
                yield return StartCoroutine(tutorialStep.StartStep());
            }

            yield break;
        }


        private abstract class BaseTutorialStep
        {
            protected GameplayEventListener _gameplayEventListener;

            public BaseTutorialStep(GameplayEventListener gameplayEventListener)
            {
                _gameplayEventListener = gameplayEventListener;
            }
            

            public abstract IEnumerator StartStep();
        }

        private sealed class FirstTutorialStep : BaseTutorialStep
        {
            private readonly ISource _source;
            private readonly TutorialObject _tutorialObject;

            private bool _isSourceMined;

            public FirstTutorialStep(GameplayEventListener gameplayEventListener, ISource source) : base(gameplayEventListener)
            {
                if (source.CachedTransform.TryGetComponent<TutorialObject>(out TutorialObject tutorialObject))
                    _tutorialObject = tutorialObject;

                _source = source;
                _isSourceMined = false;
            }


            private void OnSourceMined(ISource source)
            {
                if (source != _source)
                    return;

                _isSourceMined = true;
            }


            public override IEnumerator StartStep()
            {
                _gameplayEventListener.SourceMined.AddListener(OnSourceMined);

                if (_tutorialObject != null)
                    _tutorialObject.ShowPointer();

                yield return new WaitUntil(() => _isSourceMined);

                if (_tutorialObject != null)
                    _tutorialObject.HidePointer();

                _gameplayEventListener.SourceMined.RemoveListener(OnSourceMined);
                yield break;
            }
        }

        private sealed class SecondTutorialStep : BaseTutorialStep
        {
            private readonly ISpot _spot;
            private readonly TutorialObject _tutorialObject;

            private bool _isSpotLoaded;

            public SecondTutorialStep(GameplayEventListener gameplayEventListener, ISpot spot) : base(gameplayEventListener)
            {
                if (spot.CachedTransform.TryGetComponent<TutorialObject>(out TutorialObject tutorialObject))
                    _tutorialObject = tutorialObject;

                _spot = spot;
                _isSpotLoaded = false;
            }


            private void OnSpotLoaded(ISpot spot)
            {
                if (spot != _spot)
                    return;

                _isSpotLoaded = true;
            }


            public override IEnumerator StartStep()
            {
                _gameplayEventListener.SpotLoaded.AddListener(OnSpotLoaded);

                if(_tutorialObject != null)
                    _tutorialObject.ShowPointer();

                yield return new WaitUntil(() => _isSpotLoaded);

                _gameplayEventListener.SpotLoaded.RemoveListener(OnSpotLoaded);

                if(_tutorialObject != null)
                    _tutorialObject.HidePointer();
            }
        }
    }
}