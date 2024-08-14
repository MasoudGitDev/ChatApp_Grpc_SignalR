using Shared.Server.Dtos.Chat;

namespace Client.ChatApp.Services;
internal class UserSelectionObserver {

    public event Action? OnChangeSelection;
    public ChatItemDto? Item { get; private set; }
    public bool IsGoingToHome { get; private set; } = false;
    public bool WasUserAtHomePage { get; set; } = false;
    public void SelectedItem(ChatItemDto? item , bool isGoingToHome = true , bool wasUserAtHomePage = true) {
        Item = item ?? new();
        IsGoingToHome = isGoingToHome;
        WasUserAtHomePage = wasUserAtHomePage;
        NotifyOnChangeItem();
    }

    private void NotifyOnChangeItem() => OnChangeSelection?.Invoke();
}
