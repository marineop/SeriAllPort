using CommonWpf.Communication.PhysicalInterfaces;
using CommonWpf.Communication.Protocol.EventTypes;
using CommonWpf.Communication.Protocol.PacketFields;
using CommonWpf.Communication.Protocol.PacketModes;

namespace UnitTest.PacketModes.LengthFields
{
    [TestClass]
    public sealed class TwoLength
    {
        public static void Initialize(PacketModeLengthField _packetMode, SimulatedSerial _simulatedSerial)
        {

            _packetMode.IdleTimeoutMs = 0;
            _packetMode.Fields.Clear();

            _packetMode.Fields.Add(new LengthField("Length1", 1, 1, 2, 0));
            _packetMode.Fields.Add(new LengthField("Length2", 1, 2, 2, 0));
            _packetMode.Fields.Add(PacketField.CreateVariableLength("Data1"));

            _packetMode.Validate();

            _packetMode.Serial = _simulatedSerial;
        }

        [TestMethod]
        public void NormalPacket()
        {
            PacketModeLengthField packetMode = new PacketModeLengthField();
            SimulatedSerial simulatedSerial = new SimulatedSerial();
            Initialize(packetMode, simulatedSerial);

            byte[] length1 = [0x03];
            byte[] length2 = [0x02];
            byte[] data = [0xCC, 0xDD];

            List<byte> bytes = [];
            bytes.AddRange(length1);
            bytes.AddRange(length2);
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
                CollectionAssert.AreEqual(length1, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(length2, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(data, packetReceived.PacketFields[index++].ActualData);
            }

            dequeueSuccess = packetMode.EventQueue.TryDequeue(out eventType);
            Assert.IsFalse(dequeueSuccess);
        }

        [TestMethod]
        public void TooShort()
        {
            PacketModeLengthField packetMode = new PacketModeLengthField();
            SimulatedSerial simulatedSerial = new SimulatedSerial();
            Initialize(packetMode, simulatedSerial);

            byte[] length1 = [0x03];
            byte[] length2 = [0x02];
            byte[] data = [0xCC];

            List<byte> bytes = [];
            bytes.AddRange(length1);
            bytes.AddRange(length2);
            bytes.AddRange(data);

            simulatedSerial.SimulateReceive([.. bytes]);
            packetMode.BytesReceived();
            packetMode.ParsePackets(DateTime.Now, true);

            bool dequeueSuccess;

            dequeueSuccess = packetMode.EventQueue.TryDequeue(out PacketEventType? eventType);
            Assert.IsTrue(dequeueSuccess);
            Assert.IsTrue(eventType is NonPacketBytesReceived);
            if (eventType is NonPacketBytesReceived nonPacketBytesReceived)
            {
                CollectionAssert.AreEqual(bytes, nonPacketBytesReceived.Bytes);
            }

            dequeueSuccess = packetMode.EventQueue.TryDequeue(out eventType);
            Assert.IsFalse(dequeueSuccess);
        }

        [TestMethod]
        public void NormalPacketAndAdditionalByte()
        {
            PacketModeLengthField packetMode = new PacketModeLengthField();
            SimulatedSerial simulatedSerial = new SimulatedSerial();
            Initialize(packetMode, simulatedSerial);

            byte[] length1 = [0x03];
            byte[] length2 = [0x02];
            byte[] data = [0xCC, 0xDD];
            byte[] additionalByte = [0xFF];

            List<byte> bytes = [];
            bytes.AddRange(length1);
            bytes.AddRange(length2);
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
                int index = 0;
                CollectionAssert.AreEqual(length1, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(length2, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(data, packetReceived.PacketFields[index++].ActualData);
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
