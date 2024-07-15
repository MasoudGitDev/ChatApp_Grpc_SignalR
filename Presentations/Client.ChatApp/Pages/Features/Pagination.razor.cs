using Microsoft.AspNetCore.Components;

namespace Client.ChatApp.Pages.Features;

public class PaginationViewHandler : ComponentBase {

    [Parameter]
    public uint PageSize { get; set; } = 10;

 
    [Parameter]
    public uint TotalItems { get; set; } = 1;

    [Parameter]
    public EventCallback<(uint CurrentPage, uint PageSize)> PageCallBack { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }


    protected uint CurrentPage = 1;
    protected uint TotalPage = 1;

    protected async Task SetPageSize(uint pageSize) {
        PageSize = pageSize;
        await PageCallBack.InvokeAsync((CurrentPage, PageSize));
    }



    protected async Task Decrease() {
        if(CurrentPage <= 1) {
            CurrentPage = 1;
        }
        else {
            CurrentPage -= 1;
        }
        await PageCallBack.InvokeAsync((CurrentPage, PageSize));
    }

    protected async Task Increase() {
        if(CurrentPage >= TotalPage) {
            CurrentPage = TotalPage;
        }
        else {
            CurrentPage += 1;

        }
        await PageCallBack.InvokeAsync((CurrentPage, PageSize));
    }

    protected async Task SetPage(uint number) {
        CurrentPage = number;
        await PageCallBack.InvokeAsync((CurrentPage, PageSize));
    }

    protected override void OnInitialized() {
        TotalPage = (uint)Math.Ceiling((double)TotalItems / PageSize);
    }
}
