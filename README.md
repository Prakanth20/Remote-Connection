# Remote Connection

> A lightweight Windows client that periodically fetches commands from a remote endpoint, executes them non-interactively, stores the output locally, and synchronizes the result back to the server.

---

## 📌 Overview

**RemoteCommand** is a Windows-based automation client designed for **remote diagnostics, system administration tasks, and controlled command execution**.

The application:

* Periodically polls a remote `commands.txt`
* Executes commands **non-interactively**
* Maintains execution context (working directory)
* Writes output to a managed local directory
* Uploads the updated output file back to a server
* Runs continuously in the background (no console window)

This project is intended for:

* Educational purposes
* Internship / academic projects
* Controlled enterprise automation
* Secure remote diagnostics

---

## ⚠️ Disclaimer

> This project is intended **only for legitimate, authorized use**.
> It must be deployed **with explicit consent** on systems you own or manage.
> The author assumes **no responsibility** for misuse.

---

## ✨ Features

* ✅ Background execution (no CMD window)
* ✅ Periodic polling (configurable interval)
* ✅ Remote command retrieval (`commands.txt`)
* ✅ Persistent working directory (`cd` support)
* ✅ Local output storage (AppData)
* ✅ File-based output synchronization
* ✅ HTTPS support (Cloudflare Tunnel compatible)
* ✅ Single-file standalone EXE
* ✅ No external dependencies at runtime

---

## 🧱 Architecture

```
RemoteCommandClient.exe
        |
        |-- Fetch commands.txt (HTTP GET)
        |
        |-- Execute commands sequentially
        |      - Maintains working directory
        |      - Non-interactive execution
        |
        |-- Write output to local output.txt
        |
        |-- Upload output.txt (HTTP POST)
        |
        |-- Sleep → repeat
```

---

## 📂 Local File Storage

Output files are stored in:

```
C:\Users\<username>\AppData\Local\RemoteCommandClient\output.txt
```

This avoids:

* Permission issues
* Writing beside the executable
* Suspicious behavior

---

## 🌐 Server Requirements

The server **must support**:

* `GET /commands.txt`
* `POST /output` (file upload)

Static servers **will not work**.

### Example (Flask)

```python
from flask import Flask, request

app = Flask(__name__)

@app.route("/commands.txt", methods=["GET"])
def commands():
    return open("commands.txt").read(), 200

@app.route("/output", methods=["POST"])
def output():
    file = request.files["file"]
    file.save("output.txt")
    return "OK", 200

app.run(host="127.0.0.1", port=5000)
```

---

## ☁️ Cloudflare Tunnel Support

The client works seamlessly with **Cloudflare Tunnel** (`trycloudflare.com`).

Example configuration:

```json
{
  "CommandUrl": "https://<tunnel>.trycloudflare.com/commands.txt",
  "UploadUrl": "https://<tunnel>.trycloudflare.com/output",
  "IntervalSeconds": 30
}
```

---

## 🧪 Supported Commands

Commands must be **non-interactive**.

### ✅ Supported

```
whoami
dir
ipconfig
systeminfo
powershell -Command Get-Date
```

### ❌ Not supported

```
powershell
cmd
interactive shells
```

> PowerShell commands must use `-Command`.

---

## 🔁 Working Directory Support

The client internally tracks the working directory.

Example `commands.txt`:

```
cd ..
dir
cd Windows
dir
```

Each command runs relative to the updated directory context.

---

## 🏗️ Build Instructions

### Requirements

* .NET 6 SDK or newer
* Windows (x64)

### Build single-file EXE

```bash
dotnet publish RemoteCommandClient.csproj ^
  -c Release ^
  -r win-x64 ^
  --self-contained true ^
  -p:PublishSingleFile=true
```

Output:

```
bin/Release/net6.0/win-x64/publish/RemoteCommandClient.exe
```

---

## 🖥️ Execution Mode

The application is built as a **windowless background app**:

```xml
<OutputType>WinExe</OutputType>
```

* No console window
* Runs continuously
* Closing CMD does not stop execution

---

## 🛡️ Security Notes

* No persistence mechanisms (startup, registry, services)
* No privilege escalation
* No obfuscation
* No exploit techniques
* Commands are executed as the current user
* All network communication uses HTTPS

A small number of antivirus engines may flag the binary heuristically due to:

* Command execution
* Periodic polling
* Unsigned binary

This is a **known false-positive pattern** for administrative tools.

---

## 🎓 Suggested Use Cases

* Academic projects
* Internship demonstrations
* Remote diagnostics tools
* Controlled automation clients
* Secure command runners
* System monitoring agents

---

## 📄 License

This project is provided **for educational and authorized use only**.
You are responsible for ensuring compliance with applicable laws and policies.

---

## 🙌 Acknowledgements

* .NET Runtime
* Cloudflare Tunnel
* Windows Process APIs

