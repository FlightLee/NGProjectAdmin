using Microsoft.AspNetCore.SignalR;
using NGProjectAdmin.Common.Global;
using System.Threading.Tasks;

namespace NGProjectAdmin.WebApi.AppCode.FrameworkClass
{
    public class ChatHub : Hub
    {
        /// <summary>
        /// 给连接的所有人发送消息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendMsg(string userName, string message)
        {
            //方法需要在前端实现
            return Clients.All.SendAsync(NGAdminGlobalContext.SignalRConfig.Method, userName, message);
        }
    }
}
