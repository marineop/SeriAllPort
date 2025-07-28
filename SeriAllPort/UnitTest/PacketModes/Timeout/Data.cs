using CommonWpf.Communication.PhysicalInterfaces;
using CommonWpf.Communication.Protocol.EventTypes;
using CommonWpf.Communication.Protocol.PacketFields;
using CommonWpf.Communication.Protocol.PacketModes;

namespace UnitTest.PacketModes.Timeout
{
    [TestClass]
    public sealed class Data
    {
        public static void Initialize(PacketModeTimeout _packetMode, SimulatedSerial _simulatedSerial)
        {
            
            _packetMode.IdleTimeoutMs = 0;
            _packetMode.Fields.Clear();

            _packetMode.Fields.Add(PacketField.CreateVariableLength("Data"));

            _packetMode.Validate();

            _packetMode.Serial = _simulatedSerial;
        }

        [TestMethod]
        public void NormalPacket()
        {
            PacketModeTimeout packetMode = new PacketModeTimeout();
            SimulatedSerial simulatedSerial = new SimulatedSerial();
            Initialize(packetMode, simulatedSerial);

            byte[] data = [0xAA, 0xBB];

            List<byte> bytes = [];
            bytes.AddRange(data);

            simulatedSerial.SimulateReceive([.. bytes]);
            packetMode.BytesReceived();
            packetMode.ParsePackets(DateTime.Now, true);

            bool dequeueSuccess;

            dequeueSuccess = packetMode.EventQueue.TryDequeue(out PacketEventType? eventType);
            Assert.IsTrue(dequeueSuccess);
            Assert.IsTrue(eventType is PacketReceived);
            if (eventType is PacketReceived packetReceived)
            {
                int index = 0;

                CollectionAssert.AreEqual(data, packetReceived.PacketFields[index++].ActualData);
            }

            dequeueSuccess = packetMode.EventQueue.TryDequeue(out eventType);
            Assert.IsFalse(dequeueSuccess);
        }
    }
}
