using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatClient.ServiceChat;

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IServiceChatCallback
    {
        bool isConected = false;
        ServiceChatClient client;
        int ID;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }//метод вызывается при запуске

        void ConnectUser()
        {

            if (!isConected)
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                ID = client.Connect(tbUserName.Text);
                tbUserName.IsEnabled = false;
                bConDis.Content = "disconect";
                isConected = true;
            }

        }

        void DisconnectUser()
        {

            if (isConected)
            {
                client.Disconnect(ID);
                client = null;
                bConDis.Content = "conect";
                isConected = false;
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
        }//при закрытии 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (isConected)
            {
                ConnectUser();
            }
            else
            {
                DisconnectUser();
            }

        }//событие нажатия подключения

        private void tdMessage_keyDown(object sender, KeyEventArgs e)
        {

            if(e.Key == Key.Enter)
            {

                if(client != null)
                {
                    client.SendMsg(tdMessage.Text, ID);
                    tdMessage.Text = string.Empty;
                }

            }

        }//событие отправки сообщений

        public void MsgCallBack(string msg)
        {
            lbChat.Items.Add(msg);
            lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count - 1]);
        }//принятие сообщений и скрол чата

    }
}
