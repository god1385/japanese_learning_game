public class BookPresenter
{
    private BookView _bookView;
    private BookModel _bookModel;
    public BookPresenter(BookView view, BookModel model)
    {
        _bookView = view;
        _bookModel = model;
    }

    public void ShowBook()
    {
        _bookView?.ShowBook();
        CreateCards();
    }

    public void HideBook()
    {
        _bookView?.HideBook();
    }

    public void CreateCards()
    {
        foreach (var element in _bookModel.AllElements)
        {
            var card = _bookView.CreateElementOnUi(element);

            if (!_bookModel.isUnlocked(element))
                card.SetInLockedState();
            else
            {
                card.CardButton.onClick.AddListener(OnCardClicked);
            }
        }
    }

    private void OnCardClicked()
    {

    }
}
