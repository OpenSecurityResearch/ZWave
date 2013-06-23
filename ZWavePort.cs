using System;
using System.Collections.Generic;
using System.Text;

using System.IO.Ports;
using System.Threading;

namespace ZWave_Tester
{

    class ZWavePort
    {
        private SerialPort sp;
        private Thread receiverThread;
        private Boolean sendACK = true;
        private byte[] MSG_ACKNOWLEDGE = new byte[] { 0x06 };

        public ZWavePort()
        {
            sp = new SerialPort();
            sp.PortName = "COM4";
            sp.BaudRate = 115200;
            sp.Parity = Parity.None;
            sp.DataBits = 8;
            sp.StopBits = StopBits.One;
            sp.Handshake = Handshake.None;
            sp.DtrEnable = true;
            sp.RtsEnable = true;
            sp.NewLine = System.Environment.NewLine;

            receiverThread = new Thread(
                new System.Threading.ThreadStart(ReceiveMessage));
        }

        public void Open()
        {
            if (sp.IsOpen == false)
            {
                sp.Open();
                receiverThread.Start();
            }
        }

        public void SendMessage(byte[] message)
        {
            if (sp.IsOpen == true)
            {
                if (message != MSG_ACKNOWLEDGE)
                {
                    sendACK = false;
                    message[message.Length - 1] = GenerateChecksum(message); // Insert checksum
                }
                System.Console.WriteLine("Message sent: " + ByteArrayToString(message));
                sp.Write(message, 0, message.Length);
            }
        }

        private void SendACKMessage()
        {
            SendMessage(MSG_ACKNOWLEDGE);
        }

        public void Close()
        {
            if (sp.IsOpen == true)
            {
                sp.Close();
            }
        }

        private static byte GenerateChecksum(byte[] data)
        {
            int offset = 1;
            byte ret = data[offset];
            for (int i = offset + 1; i < data.Length - 1; i++)
            {
                // Xor bytes
                ret ^= data[i];
            }
            // Not result
            ret = (byte)(~ret);
            return ret;
        }

        private void ReceiveMessage()
        {
            while (sp.IsOpen == true)
            {
                int bytesToRead = sp.BytesToRead;
                if ((bytesToRead != 0) & (sp.IsOpen == true))
                {
                    byte[] message = new byte[bytesToRead];
                    sp.Read(message, 0, bytesToRead);
                    System.Console.WriteLine(" *** [Possible HomeId BruteForced] Message received: " + ByteArrayToString(message) + "    . Press any key to continue *****");
                    System.Console.ReadLine();

                    if (sendACK) // Does the incoming message require an ACK?
                    {
                        SendACKMessage();
                    }
                    sendACK = true;
                }
            }
        }

        private String ByteArrayToString(byte[] message)
        {
            String ret = String.Empty;
            foreach (byte b in message)
            {
                ret += b.ToString("X2") + " ";
            }
            return ret.Trim();
        }
    }
}

