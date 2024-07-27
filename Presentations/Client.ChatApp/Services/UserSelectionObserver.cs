using Shared.Server.Dtos.Chat;

namespace Client.ChatApp.Services;
internal class UserSelectionObserver {

    public event Action? OnChangeSelection;
    public ChatItemDto? Item { get; private set; }
    public void OnSelectedItem(ChatItemDto item) {
        Item = item;
        NotifyOnChangeItem();
    }

    private void NotifyOnChangeItem() => OnChangeSelection?.Invoke();
}
