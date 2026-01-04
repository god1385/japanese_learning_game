using UnityEngine;

public class BookInitialize : MonoBehaviour
{
    [SerializeField] private BookView bookView;
    [SerializeField] private LessonsData lessonData;

    private BookModel _bookModel;
    private BookPresenter _bookPresenter;

    private void Awake()
    {
        _bookModel = new BookModel(lessonData);
        _bookPresenter = new BookPresenter(bookView, _bookModel);
    }

    public void Initialize()
    {
        _bookPresenter.ShowBook();
    }
}
