
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

def decrypt_payload(payload):
    try:
        # Pad Base64 if missing
        missing_padding = len(payload) % 4
        if missing_padding:
            payload += '=' * (4 - missing_padding)
            
        decoded_b64 = base64.b64decode(payload)
        decrypted_xor = xor_decrypt(decoded_b64, k)
        decompressed = zlib.decompress(decrypted_xor)
        return decompressed.decode('utf-8', errors='replace')
    except Exception as e:
        return f"Error: {e}"

try:
    with open('responses.txt', 'r') as f:
        content = f.read()

    # Find all matches
    # Pattern: kh + (payload) + kf
    pattern = re.escape(kh) + r'(.+?)' + re.escape(kf)
    matches = re.findall(pattern, content)

    print(f"[*] Found {len(matches)} responses.\n")

    for i, payload in enumerate(matches):
        print(f"--- Response #{i+1} ---")
        result = decrypt_payload(payload)
        print(result)
        print("-" * 30 + "\n")

except Exception as e:
    print(f"[-] Fatal Error: {e}")
