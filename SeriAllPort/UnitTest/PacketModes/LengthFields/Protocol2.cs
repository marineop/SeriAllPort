using CommonWpf.Communication.PhysicalInterfaces;
using CommonWpf.Communication.Protocol.EventTypes;
using CommonWpf.Communication.Protocol.PacketFields;
using CommonWpf.Communication.Protocol.PacketModes;

namespace UnitTest.PacketModes.LengthFields
{
    [TestClass]
    public sealed class Protocol2
    {
        public static void Initialize(PacketModeLengthField _packetMode, SimulatedSerial _simulatedSerial)
        {

            _packetMode.IdleTimeoutMs = 0;
            _packetMode.Fields.Clear();

            _packetMode.Fields.Add(new Preamble("Preamble", [0x00]));
            _packetMode.Fields.Add(new LengthField("Length", 1, 2, 2, 0));
            _packetMode.Fields.Add(PacketField.CreateVariableLength("Data"));
            _packetMode.Fields.Add(PacketField.CreateFixedLength("CRC", 2));

            _packetMode.Validate();

            _packetMode.Serial = _simulatedSerial;
        }

        [TestMethod]
        public void NormalPacket()
        {
            PacketModeLengthField packetMode = new PacketModeLengthField();
            SimulatedSerial simulatedSerial = new SimulatedSerial();
            Initialize(packetMode, simulatedSerial);

            byte[] preamble = [0x00];
            byte[] length = [0x02];
            byte[] data = [0xAA, 0xBB];
            byte[] crc = [0xCC, 0xCC];

            List<byte> bytes = [];
            bytes.AddRange(preamble);
            bytes.AddRange(length);
            bytes.AddRange(data);
            bytes.AddRange(crc);

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
                CollectionAssert.AreEqual(preamble, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(length, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(data, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(crc, packetReceived.PacketFields[index++].ActualData);
            }

            dequeueSuccess = packetMode.EventQueue.TryDequeue(out eventType);
            Assert.IsFalse(dequeueSuccess);
        }

        [TestMethod]
        public void NormalPacketWithPreErrorByte()
        {
            PacketModeLengthField packetMode = new PacketModeLengthField();
            SimulatedSerial simulatedSerial = new SimulatedSerial();
            Initialize(packetMode, simulatedSerial);

            byte[] preErrorByte = [0x11];
            byte[] preamble = [0x00];
            byte[] length = [0x02];
            byte[] data = [0xAA, 0xBB];
            byte[] crc = [0xCC, 0xCC];

            List<byte> bytes = [];
            bytes.AddRange(preErrorByte);
            bytes.AddRange(preamble);
            bytes.AddRange(length);
            bytes.AddRange(data);
            bytes.AddRange(crc);

            simulatedSerial.SimulateReceive([.. bytes]);
            packetMode.BytesReceived();
            packetMode.ParsePackets(DateTime.Now, true);

            bool dequeueSuccess;

            dequeueSuccess = packetMode.EventQueue.TryDequeue(out PacketEventType? eventType);
            Assert.IsTrue(dequeueSuccess);
            Assert.IsTrue(eventType is NonPacketBytesReceived);
            if (eventType is NonPacketBytesReceived nonPacketBytesReceived)
            {
                CollectionAssert.AreEqual(preErrorByte, nonPacketBytesReceived.Bytes);
            }

            dequeueSuccess = packetMode.EventQueue.TryDequeue(out eventType);
            Assert.IsTrue(dequeueSuccess);
            Assert.IsTrue(eventType is PacketReceived);
            if (eventType is PacketReceived packetReceived)
            {
                int index = 0;
                CollectionAssert.AreEqual(preamble, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(length, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(data, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(crc, packetReceived.PacketFields[index++].ActualData);
            }

            dequeueSuccess = packetMode.EventQueue.TryDequeue(out eventType);
            Assert.IsFalse(dequeueSuccess);
        }
    }
}
