using System.Reflection;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("UNTests.Apps.Chats")]
namespace Apps.Chats;
public class AppsChatsAssembly {

    public static Assembly Assembly => typeof(AppsChatsAssembly).Assembly;
}
