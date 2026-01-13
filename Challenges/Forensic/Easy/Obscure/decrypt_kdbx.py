
import re
import base64
import zlib

k = "80e32263"
kh = "6f8af44abea0"
kf = "351039f4a7b5"

def xor_decrypt(data, key):
    key_bytes = key.encode('utf-8')
    key_len = len(key_bytes)
    output = bytearray()
    for i, byte in enumerate(data):
        output.append(byte ^ key_bytes[i % key_len])
    return output

try:
    with open('response.txt', 'r', encoding='utf-8') as f:
        content = f.read()

    # Extract payload between kh and kf
    pattern = re.escape(kh) + r'(.+)' + re.escape(kf)
    match = re.search(pattern, content)
    
    if not match:
        print("[-] Pattern not found in response.txt")
        exit()

    payload = match.group(1)
    print(f"[+] Found encoded response payload (len={len(payload)})")

    # 1. Outer Layer: Base64 Decode
    try:
        decrypted_layer1 = base64.b64decode(payload)
    except Exception as e:
        # Try fixing padding if needed
        missing_padding = len(payload) % 4
        if missing_padding:
            payload += '=' * (4 - missing_padding)
            decrypted_layer1 = base64.b64decode(payload)
        else:
            print(f"[-] Base64 decode failed: {e}")
            exit()

    # 2. Outer Layer: XOR Decrypt
    decrypted_layer2 = xor_decrypt(decrypted_layer1, k)

    # 3. Outer Layer: GZ Decompress
    try:
        decompressed_output = zlib.decompress(decrypted_layer2)
    except Exception as e:
        print(f"[-] GZ extraction failed: {e}")
        exit()
    
    # The decompressed output is the result of the system command: system('base64 -w 0 pwdb.kdbx 2>&1')
    # So this data is the Base64 encoded KDBX file.
    kdbx_b64 = decompressed_output.decode('utf-8', errors='ignore')
    
    print(f"[+] Recovered KDBX Base64 Data (len={len(kdbx_b64)})")
    
    # 4. Inner Layer: Base64 Decode to get binary KDBX
    try:
        kdbx_data = base64.b64decode(kdbx_b64)
    except Exception as e:
         print(f"[-] Inner Base64 decode failed: {e}")
         exit()

    # Save to file
    with open('pwdb.kdbx', 'wb') as f:
        f.write(kdbx_data)
        
    print(f"[+] Successfully saved 'pwdb.kdbx' ({len(kdbx_data)} bytes)")

except Exception as e:
    print(f"[-] Error: {e}")
