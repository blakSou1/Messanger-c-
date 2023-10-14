using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wcf_chat
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();
        private int nextID = 1;

        public int Connect(string name)
        {
            ServerUser user = new ServerUser()
            {
                ID = nextID,
                Name = name,
                operationContext = OperationContext.Current
            };
            nextID++;
            users.Add(user);
            SendMsg("; " + user.Name + "влетает с 2 ног", 0);
            return user.ID;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.ID == id);

            if(user != null)
            {
                users.Remove(user);
                SendMsg("; " + user.Name + "ливнул", 0);
            }

        }

        public void SendMsg(string msg, int id)
        {

            foreach(var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString();

                var user = users.FirstOrDefault(i => i.ID == id);

                if (user != null)
                {
                    answer += ":" + user.Name + " ";
                }

                answer += msg;
                item.operationContext.GetCallbackChannel<IServerChatCallBack>().MsgCallBack(answer);
            }

        }

    }
}
