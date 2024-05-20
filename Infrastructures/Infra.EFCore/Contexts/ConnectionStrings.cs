using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.EFCore.Contexts;
internal class ConnectionStrings {
    public const string GRPCChatDb = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GRPCChatDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
}
