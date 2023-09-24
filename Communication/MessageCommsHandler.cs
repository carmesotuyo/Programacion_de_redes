using System;
using System.Net.Sockets;

namespace Communication
{
	public class MessageCommsHandler
	{
        private readonly ConversionHandler _conversionHandler;
        private readonly SocketHelper _socketHelper;

        public MessageCommsHandler(Socket socket)
        {
            _conversionHandler = new ConversionHandler();
            _socketHelper = new SocketHelper(socket);
        }

        public void SendMessage(string message)
        {
            // ---> Enviar el largo del mensaje
            _socketHelper.Send(_conversionHandler.ConvertIntToBytes(message.Length));
            // ---> Enviar el mensaje
            _socketHelper.Send(_conversionHandler.ConvertStringToBytes(message));
        }

        public string ReceiveMessage()
        {
            // ---> Recibir el largo del mensaje
            int msgLength = _conversionHandler.ConvertBytesToInt(_socketHelper.Receive(Protocol.FixedDataSize));
            // ---> Recibir el mensaje
            string message = _conversionHandler.ConvertBytesToString(_socketHelper.Receive(msgLength));
            return message;
        }

        public float ReceiveNumber()
        {
            // ---> Recibir el largo del mensaje
            int msgLength = _conversionHandler.ConvertBytesToInt(_socketHelper.Receive(Protocol.FixedDataSize));
            // ---> Recibir el valor
            float message = _conversionHandler.ConvertBytesToFloat(_socketHelper.Receive(msgLength));
            return message;

        }
    }
}

