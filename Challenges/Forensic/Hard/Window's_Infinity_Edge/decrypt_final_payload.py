import base64
from cryptography.hazmat.primitives.ciphers import Cipher, algorithms, modes
from cryptography.hazmat.backends import default_backend

# 1. Read the base64 encoded payload
input_file = "extractedfile"
output_file = "decrypted_payload.bin"

try:
    with open(input_file, "r") as f:
        # Read file, remove potential multipart headers if any remain (though previous steps should have cleaned it)
        content = f.read().strip()
        # Handle if it still has multipart boundaries manually
        if "--" in content:
            lines = content.splitlines()
            # simple filter to get the base64 blob
            base64_lines = [l for l in lines if not l.startswith("--") and "Content-" not in l and l.strip()]
            content = "".join(base64_lines)
            
    encrypted_data = base64.b64decode(content)
except Exception as e:
    print(f"Error reading/decoding input file: {e}")
    exit()

# 2. Key and IV Setup
# Key from upload(2).aspx: "4d65bdbad183f00203b1e80cf96fba549663dabeab12fab153a921b346975cdd"
hex_key = "4d65bdbad183f00203b1e80cf96fba549663dabeab12fab153a921b346975cdd"
key = bytes.fromhex(hex_key)

# Usually in these payloads, IV is the first 16 bytes.
iv = encrypted_data[:16]
ciphertext = encrypted_data[16:]

print(f"Key: {hex_key}")
print(f"IV: {iv.hex()}")

# 3. Decrypt (AES-256-CBC is standard for DInjector/SharPy)
try:
    cipher = Cipher(algorithms.AES(key), modes.CBC(iv), backend=default_backend())
    decryptor = cipher.decryptor()
    decrypted_data = decryptor.update(ciphertext) + decryptor.finalize()
    
    # 4. Save
    with open(output_file, "wb") as f:
        f.write(decrypted_data)
        
    print(f"[+] Decryption successful. Saved to {output_file}")
    
    # Peek at the header to see what it is
    print(f"Header: {decrypted_data[:4].hex()} ({decrypted_data[:2]})")
    
except Exception as e:
    print(f"Decryption failed: {e}")
