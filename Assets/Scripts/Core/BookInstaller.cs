using UnityEngine;
using Zenject;

public class BookInstaller : MonoInstaller
{
    [SerializeField] private BookView bookView;
    [SerializeField] private AudioSourceHandler audioSourceHandler;
    [SerializeField] private LessonsData lessonsData;
    [SerializeField] private AlphabetData alphabetData;

    public override void InstallBindings()
    {
        Container.Bind<BookView>()
            .FromInstance(bookView)
            .AsSingle();

        Container.Bind<ISaveService>()
            .To<JsonDataSaveService>()
            .AsSingle();

        Container.Bind<BookProgressTracker>()
            .AsSingle();

        Container.Bind<BookModel>()
            .AsSingle()
            .WithArguments(lessonsData, alphabetData);

        Container.Bind<BookPresenter>()
            .AsSingle();

        Container.Bind<SymbolInteractionsConnector>()
            .AsSingle();

        Container.Bind<AudioSourceHandler>()
            .FromInstance(audioSourceHandler)
            .AsSingle();

        Container.Bind<TutorialInfo>()
            .AsSingle();
    }
}
