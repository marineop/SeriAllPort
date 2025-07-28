using CommonWpf.Communication.PhysicalInterfaces;
using CommonWpf.Communication.Protocol.EventTypes;
using CommonWpf.Communication.Protocol.PacketFields;
using CommonWpf.Communication.Protocol.PacketModes;

namespace UnitTest.PacketModes.LengthFields
{
    [TestClass]
    public sealed class L_V_FD_V_FD_V
    {
        public static void Initialize(PacketModeLengthField _packetMode, SimulatedSerial _simulatedSerial)
        {
            
            _packetMode.IdleTimeoutMs = 0;
            _packetMode.Fields.Clear();

            _packetMode.Fields.Add(new LengthField("Length1", 1, 1, 5, 0));
            _packetMode.Fields.Add(PacketField.CreateVariableLength("Data1"));
            _packetMode.Fields.Add(PacketField.CreateFixedData("Delimiter1", [0x2C]));
            _packetMode.Fields.Add(PacketField.CreateVariableLength("Data2"));
            _packetMode.Fields.Add(PacketField.CreateFixedData("Delimiter2", [0x2C]));
            _packetMode.Fields.Add(PacketField.CreateVariableLength("Data3"));

            _packetMode.Validate();

            _packetMode.Serial = _simulatedSerial;
        }

        [TestMethod]
        public void NormalPacket()
        {
            PacketModeLengthField packetMode = new PacketModeLengthField();
            SimulatedSerial simulatedSerial = new SimulatedSerial();
            Initialize(packetMode, simulatedSerial);

            byte[] length = [0x08];
            byte[] data1 = [0x00, 0x01];
            byte[] delimiter1 = [0x2C];
            byte[] data2 = [0xAA, 0xBB];
            byte[] delimiter2 = [0x2C];
            byte[] data3 = [0xCC, 0xDD];

            List<byte> bytes = [];
            bytes.AddRange(length);
            bytes.AddRange(data1);
            bytes.AddRange(delimiter1);
            bytes.AddRange(data2);
            bytes.AddRange(delimiter2);
            bytes.AddRange(data3);

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
                CollectionAssert.AreEqual(delimiter1, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(data2, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(delimiter2, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(data3, packetReceived.PacketFields[index++].ActualData);
            }

            dequeueSuccess = packetMode.EventQueue.TryDequeue(out eventType);
            Assert.IsFalse(dequeueSuccess);
        }
    }
}
