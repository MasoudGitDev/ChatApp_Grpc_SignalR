using Shared.Server.Dtos.User;

namespace Client.ChatApp.Services;  
internal class UserSelectionObserver {

    public event Action? OnChangeSelection;
    public UserBasicInfoDto? Item { get; private set; }
    public void OnSelectedItem(UserBasicInfoDto item) {
        Item = item;
        NotifyOnChangeItem();
    }

    private void NotifyOnChangeItem() => OnChangeSelection?.Invoke();
}
