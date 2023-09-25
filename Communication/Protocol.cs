using System.Text;

namespace Communication
{
    public static class Protocol
    {
        public static readonly int FixedDataSize = 4; // data length

        public const int FixedFileSize = 8;
        public const int MaxPacketSize = 32768; //32KB tamaño maximo de los paquetes que vamos a enviar
        public const string NoImagePath = "SIN_IMAGEN";

        public static long CalculateFileParts(long fileSize)
        {
            var fileParts = fileSize / MaxPacketSize;
            return fileParts * MaxPacketSize == fileSize ? fileParts : fileParts + 1; // si sobra hace un paquete mas
        }
    }
}
