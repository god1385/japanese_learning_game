using UnityEngine;

public class BookInitialize : MonoBehaviour
{
    [SerializeField] private BookView bookView;
    [SerializeField] private LessonsData lessonData;
    [SerializeField] private AlphabetData alphabetData;

    private BookModel _bookModel;
    private BookPresenter _bookPresenter;

    private void Awake()
    {
        _bookModel = new BookModel(lessonData, alphabetData);
        _bookPresenter = new BookPresenter(bookView, _bookModel);
    }

    public void Initialize()
    {
        _bookPresenter.ShowBook();
    }
}
