
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
    with open('data.txt', 'r', encoding='utf-8') as f:
        content = f.read()

    # Regex to extract payload between kh and kf
    # PHP code: preg_match("/$kh(.+)$kf/", ...
    # We use non-greedy .+? just in case, though PHP pattern was .+ greedy by default?
    # Actually PHP .+ is greedy, but usually it stops at the last occurrence of kf if multiple exist, 
    # but here we likely have only one. Let's use .+? to be safe if multiple appear, or better, .+? to find the first complete sandwich.
    pattern = re.escape(kh) + r'(.+)' + re.escape(kf)
    match = re.search(pattern, content)
    
    if not match:
        print("[-] Pattern not found in data.txt")
        exit()


    payload = match.group(1)
    print(f"[+] Found encoded payload (len={len(payload)}): {payload}")
    
    # Fix padding if necessary
    missing_padding = len(payload) % 4
    if missing_padding:
        payload += '=' * (4 - missing_padding)
        print(f"[*] Added {4 - missing_padding} padding characters.")

    # 1. Base64 Decode
    try:
        decoded_b64 = base64.b64decode(payload)
    except Exception as e:
        print(f"[-] Base64 decode failed: {e}")
        exit()

    # 2. XOR Decrypt
    decrypted_xor = xor_decrypt(decoded_b64, k)

    # 3. GZ Uncompress
    try:
        # zlib.decompress handles zlib header (RFC 1950) which gzcompress uses
        decompressed = zlib.decompress(decrypted_xor)
        print("\n[+] Decryption Successful! Content:")
        print("-" * 40)
        print(decompressed.decode('utf-8', errors='replace'))
        print("-" * 40)
    except Exception as e:
        print(f"[-] GZ extraction failed: {e}")
        print("[*] Raw XOR decrypted data dump:")
        print(decrypted_xor)

except Exception as e:
    print(f"[-] Error: {e}")
