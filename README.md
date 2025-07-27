# SeriAllPort

SeriAllPort is a simple UI-based application for inspecting and analyzing data transmissions between a PC and serial devices (e.g., UART, RS-232/422/485).

---

## Platform

- **Operating Systems:** Windows 10, Windows 11  
- **.NET:** .NET 8.0 (LTS)  
- **UI Framework:** WPF

![Application Screenshot](Image/App.png)

---

# Features _(Work in Progress)_

## Send Raw Data

- Send raw bytes represented in hexadecimal format.  
  _Example:_ `AA BB 11 22`
- Send plain text encoded in UTF-8.  
  _Example:_ `text`

## Create custom Command Button  
- Each button is linked to a predefined command, enabling fast and intuitive interaction with your devices.  
  ![Application Screenshot](Image/CommandEditor.png)
---

## Protocol System
![Application Screenshot](Image/ProtocolEditor.png)

When data is received, it may be displayed across multiple lines if the application does not understand the device’s protocol. For example, a stream like `00 AA 02 DD DD CC CC 00 AA 02 DD DD CC CC` might appear as:

```
Received: 00 AA 02 DD DD CC CC 00 AA
Received: 02 DD DD CC CC
```

The **Protocol System** allows you to define how SeriAllPort parses the incoming data stream into **packets**, making the data easier to interpret.

```
Received: 00 AA 02 DD DD CC CC
Received: 00 AA 02 DD DD CC CC
```

### Packet Modes

**1. Length Field**

Use a length field to specify the length of the packet. For example:
```
[Length of Data][Data]
```

**2. End of Packet Symbol**

Specify a termination character or sequence (e.g., CR, LF, CRLF) to mark the end of a packet. For example, if the **End of Packet Symbol** is `0D 0A`:

```
Data stream: 31 32 0D 0A 33 34 0D 0A

Received: 31 32 0D 0A // first packet
Received: 33 34 0D 0A // second packet
```

**3. Timeout**

Use a timeout to separate packets when the connected device does not use any kind of explicit packet boundaries. If there is no incoming data for the specified time, SeriAllPort treats the received bytes as one packet.

_Example:_ Idle Timeout: 50 ms  
```
Data stream: 31 32 0D 0A [idle for 100 ms] 33 34 0D 0A

Received: 31 32 0D 0A // first packet
// idle for 100 ms
Received: 33 34 0D 0A // second packet
```
---


---
### Field Types

- **Preamble**  
  Optional. If defined, SeriAllPort scans for the preamble as the start of a packet. Bytes between the end of the last packet and the next valid preamble are marked as errors (`R.Error`).  
  - Must be **Fixed Data** field.
  - Must be the first field.

- **Fields**  
  User-defined fields within the packet.

- **End of Packet**  
  Defines a termination symbol for packets.  
  - Required if **Packet Mode** is **End of Packet Symbol**.
  - Must be the last field.
  - Cannot be used in **Timeout** mode.
  
---

### Field Length Modes

- **Fixed Length**  
  The length is defined and SeriAllPort extracts data accordingly.

- **Fixed Data**  
  Both length and content must match the specified value.  
  _Example_: **Preamble** and **End of Packet** are **Fixed Data** fields.

- **Variable Length**  
  The length is determined dynamically by other fields in the packet.
---

## Profile System

SeriAllPort allows you to save your configuration as a **Profile**, making it easy to switch between different device setups.

A profile can include:

- Used Protocol
- User defined command buttons
- COM Port settings
- Preferred data display format
- Default send mode: raw bytes or plain text

---
