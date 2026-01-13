# Reminiscent Forensic Analysis Log

## 1. Initial Assessment
- **Target File**: `flounder-pc-memdump.elf`
- **Profile Identification**: Analyzed `imageinfo.txt`.
    - Suggested Profile: **Win7SP1x64**

## 2. Spear Phishing Context (Email Analysis)
- **File**: `Resume.eml`
- **Sender**: `Brian Loodworm <bloodworm@madlab.lcl>`
- **Recipient**: `flounder@madlab.lcl`
- **Suspicious Content**:
    - Subject: "Resume"
    - Link/C2: `http://10.10.99.55:8080/resume.zip`
    - Attachment: `resume.zip`
- **Inference**: The victim likely clicked the link or opened the attachment, triggering the malware.

## 3. Network Connection Analysis
- **Tool**: Volatility 3 `windows.netscan`
- **Target IP**: `10.10.99.55` (extracted from email)
- **Findings**:
    - **Process**: `powershell.exe`
    - **PID**: `2752`
    - **Connection**: TCP connection to `10.10.99.55:80`
- **Conclusion**: PID 2752 is the malicious process responsible for the C2 communication.

## 4. Payload Analysis
- **Tool**: Volatility 3 `windows.cmdline`
- **Target**: PID `2752`
- **Result**: PowerShell process executed with a hidden window (`-w 1`) and a large Base64 encoded payload (`-enc ...`).
- **Payload Decoding**:
    - The Base64 string contains a PowerShell script.
    - Script analysis reveals it connects to `http://10.10.99.55:8080/login/process.php`.
    - **Flag**: The script explicitly defines a flag variable:
      `$flag='HTB{$_j0G_y0uR_M3m0rY_$}'`

## 5. Summary
The attacker sent a phishing email with a malicious link. The user clicked it, executing a PowerShell script (PID 2752) that established a connection to the attacker's C2 server (`10.10.99.55`). The memory dump reveals the encoded command which, when decoded, provides the challenge flag.
