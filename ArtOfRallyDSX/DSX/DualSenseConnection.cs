using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace ArtOfRallyDSX.DSX
{
    public class DualSenseConnection
    {
        private readonly UdpClient _client;
        private readonly IPEndPoint _endPoint;

        private const string PortFilePath = @"C:\Temp\DualSenseX\DualSenseX_PortNumber.txt";
        public const int ControllerIndex = 0;

        public DualSenseConnection()
        {
            _client = new UdpClient();
            if (!File.Exists(PortFilePath))
            {
                Main.Logger.Critical("Failed to connect. DualSenseX is required for this mod to work.");
            }

            var portNumber = File.ReadAllText(PortFilePath);
            _endPoint = new IPEndPoint(Triggers.localhost, Convert.ToInt32(portNumber));
        }

        public void Send(Instruction[] instructions)
        {
            var data = Encoding.ASCII.GetBytes(Triggers.PacketToJson(new Packet { instructions = instructions }));
            _client.Send(data, data.Length, _endPoint);
        }

        public void ffb(float value)
        {
            Send(new[]
                {
                    new Instruction
                    {
                        type = InstructionType.TriggerUpdate,
                        parameters = new object[] { ControllerIndex, Trigger.Right, TriggerMode.Resistance, 0, value }
                    },
                    new Instruction
                    {
                        type = InstructionType.TriggerUpdate,
                        parameters = new object[] { ControllerIndex, Trigger.Left, TriggerMode.Resistance, 0, value }
                    }
                }
            );
        }
    }
}