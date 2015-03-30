using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Text;
using SMEVService.Attributes;
using SMEVService.smev;

namespace SMEVService
{
    /// <summary>
    /// Реализация сервиса
    /// </summary>
    [SignHeaderOutputBehavior]
    public class TestService : ITestService
    {
        public Smev GetCurrentDate()
        {
            try
            {
                // ServiceReference1.MessageDataType sfd = new ServiceReference1();
                var result = new Smev();
                result.Header = new Header { NodeId = 10, TimeStamp = DateTime.Now, MessageId = Guid.NewGuid(), MessageClass = MessageClassType.RESPONSE };

                result.Message = new Message();
                result.Message.Sender = new Sender("FOIV00100", "ФОИВ-001");
                result.Message.Recipient = new Recipient("STES01013", "Тестовый СИР");
                result.Message.Originator = new Originator("FOIV00101", "ФОИВ-001");
                result.Message.ExchangeType = "3";
                //Блок обертка данных
                result.MessageData = new MessageData();
                result.MessageData.AppData = new AppData();
                result.MessageData.AppData.MyTime = DateTime.Now;
                //result.MessageData.AppDocument = new AppDocument();
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return null;
        }


        public Smev GetFile()
        {
            try
            {
                // ServiceReference1.MessageDataType sfd = new ServiceReference1();
                var result = new Smev();
                result.Header = new Header { NodeId = 10, TimeStamp = DateTime.Now, MessageId = Guid.NewGuid(), MessageClass = MessageClassType.RESPONSE };

                result.Message = new Message();
                result.Message.Sender = new Sender("FOIV00100", "ФОИВ-001");
                result.Message.Recipient = new Recipient("STES01013", "Тестовый СИР");
                result.Message.Originator = new Originator("FOIV00101", "ФОИВ-001");
                result.Message.ExchangeType = "3";
                //Блок обертка данных
                result.MessageData = new MessageData();
                //result.MessageData.AppData = new AppData();
                //result.MessageData.AppData.MyTime = DateTime.Now;
                result.MessageData.AppDocument = new AppDocumentType();
                //Тут читаем файл
                string file = "Hellow world";
                byte[] binary = Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(file)));
                result.MessageData.AppDocument.BinaryData = binary;

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return null;
        }

        public Smev GetError()
        {
            Fault fault = new Fault();
            fault.Description = "У вас все плохо!";
            throw new FaultException<Fault>(fault);
        }
    }
}
