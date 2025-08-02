using CommonWpf.Communication.PhysicalInterfaces;
using CommonWpf.Communication.Protocol.EventTypes;
using CommonWpf.Communication.Protocol.PacketFields;
using CommonWpf.Communication.Protocol.PacketModes;

namespace UnitTest.PacketModes.LengthFields
{
    [TestClass]
    public sealed class Protocol1
    {
        public static void Initialize(PacketModeLengthField _packetMode, SimulatedSerial _simulatedSerial)
        {

            _packetMode.IdleTimeoutMs = 0;
            _packetMode.Fields.Clear();

            _packetMode.Fields.Add(new LengthField("Length", 1, 1, 1, 0));
            _packetMode.Fields.Add(PacketField.CreateVariableLength("Data"));

            _packetMode.Validate();

            _packetMode.Serial = _simulatedSerial;
        }

        [TestMethod]
        public void NormalPacket()
        {
            PacketModeLengthField packetMode = new PacketModeLengthField();
            SimulatedSerial simulatedSerial = new SimulatedSerial();
            Initialize(packetMode, simulatedSerial);

            byte[] length = [0x01];
            byte[] data = [0xAB];

            List<byte> bytes = [];
            bytes.AddRange(length);
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
                CollectionAssert.AreEqual(length, packetReceived.PacketFields[0].ActualData);
                CollectionAssert.AreEqual(data, packetReceived.PacketFields[1].ActualData);
            }

            dequeueSuccess = packetMode.EventQueue.TryDequeue(out eventType);
            Assert.IsFalse(dequeueSuccess);
        }

        [TestMethod]
        public void NormalPacketAdditionalByte()
        {
            PacketModeLengthField packetMode = new PacketModeLengthField();
            SimulatedSerial simulatedSerial = new SimulatedSerial();
            Initialize(packetMode, simulatedSerial);

            byte[] length = [0x01];
            byte[] data = [0xAB];
            byte[] additionalByte = [0xCC];

            List<byte> bytes = [];
            bytes.AddRange(length);
            bytes.AddRange(data);
            bytes.AddRange(additionalByte);

            simulatedSerial.SimulateReceive([.. bytes]);
            packetMode.BytesReceived();
            packetMode.ParsePackets(DateTime.Now, true);

            bool dequeueSuccess;

            dequeueSuccess = packetMode.EventQueue.TryDequeue(out PacketEventType? eventType);
            Assert.IsTrue(dequeueSuccess);
            Assert.IsTrue(eventType is PacketReceived);
            if (eventType is PacketReceived packetReceived)
            {
                CollectionAssert.AreEqual(length, packetReceived.PacketFields[0].ActualData);
                CollectionAssert.AreEqual(data, packetReceived.PacketFields[1].ActualData);
            }

            dequeueSuccess = packetMode.EventQueue.TryDequeue(out eventType);
            Assert.IsTrue(dequeueSuccess);
            Assert.IsTrue(eventType is NonPacketBytesReceived);
            if (eventType is NonPacketBytesReceived nonPacketBytesReceived)
            {
                CollectionAssert.AreEqual(additionalByte, nonPacketBytesReceived.Bytes);
            }

            dequeueSuccess = packetMode.EventQueue.TryDequeue(out eventType);
            Assert.IsFalse(dequeueSuccess);
        }
    }
}
