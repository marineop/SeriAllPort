using CommonWpf.Communication.PhysicalInterfaces;
using CommonWpf.Communication.Protocol.EventTypes;
using CommonWpf.Communication.Protocol.PacketFields;
using CommonWpf.Communication.Protocol.PacketModes;

namespace UnitTest.PacketModes.LengthFields
{
    [TestClass]
    public sealed class L_V_FD_Data
    {
        public static void Initialize(PacketModeLengthField _packetMode, SimulatedSerial _simulatedSerial)
        {

            _packetMode.IdleTimeoutMs = 0;
            _packetMode.Fields.Clear();

            _packetMode.Fields.Add(new LengthField("Length1", 1, 3, 3, 0));
            _packetMode.Fields.Add(PacketField.CreateVariableLength("Data1"));
            _packetMode.Fields.Add(PacketField.CreateFixedData("Delimiter", [0x2C]));
            _packetMode.Fields.Add(PacketField.CreateVariableLength("Data2"));

            _packetMode.Validate();

            _packetMode.Serial = _simulatedSerial;
        }

        [TestMethod]
        public void NormalPacket()
        {
            PacketModeLengthField packetMode = new PacketModeLengthField();
            SimulatedSerial simulatedSerial = new SimulatedSerial();
            Initialize(packetMode, simulatedSerial);

            byte[] length = [0x03];
            byte[] data1 = [0x00, 0x01];
            byte[] delimiter = [0x2C];
            byte[] data2 = [0xCC, 0xDD, 0xEE];

            List<byte> bytes = [];
            bytes.AddRange(length);
            bytes.AddRange(data1);
            bytes.AddRange(delimiter);
            bytes.AddRange(data2);

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
                CollectionAssert.AreEqual(length, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(data1, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(delimiter, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(data2, packetReceived.PacketFields[index++].ActualData);
            }

            dequeueSuccess = packetMode.EventQueue.TryDequeue(out eventType);
            Assert.IsFalse(dequeueSuccess);
        }
    }
}
