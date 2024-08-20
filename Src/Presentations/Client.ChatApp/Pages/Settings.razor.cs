using Client.ChatApp.Protos.Users;
using Google.Protobuf;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Client.ChatApp.Pages;

public class SettingsViewHandler : ComponentBase {


    //===================== Injects

    [Inject]
    private GrpcChannel GrpcChannel { get; set; } = null!;

    private UserCommandRPCs.UserCommandRPCsClient UserCommands => new(GrpcChannel);

    //=====================

    protected string ImageUrl = String.Empty;

    private IBrowserFile? imageFile = null;

    protected async Task LoadImage(InputFileChangeEventArgs e) {        
        if(e.File is null || e.File.Size <= 0) {
            return;
        }
        if(e.File.Size > ( 1024 * 1024 * 2 )) {
            return;
        }
        imageFile = e.File;
        byte[] buffer = new byte[imageFile.Size];
        await imageFile.OpenReadStream(1024*1024*2).ReadAsync(buffer);
        ImageUrl = $"data:{imageFile.ContentType};base64,{Convert.ToBase64String(buffer)}";
    }

    protected async Task SaveImageAsync() {
        if(imageFile is null) { 
            return;
        }
        string newFileName = Path.ChangeExtension(Path.GetRandomFileName() , Path.GetExtension(imageFile.Name));
        byte[] buffer = new byte[imageFile.Size];
        var result = await UserCommands.UpdateImageUrlAsync(new Protos.File(){
            Name = newFileName,
            Data = ByteString.CopyFrom(buffer)
        });
        if(result.IsSuccessful) {
            Console.WriteLine("File saved");
        }
    }

}
