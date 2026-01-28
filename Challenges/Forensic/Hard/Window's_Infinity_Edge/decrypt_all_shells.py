import os
import base64
import re
from cryptography.hazmat.primitives.ciphers import Cipher, algorithms, modes
from cryptography.hazmat.backends import default_backend

# Configuration
input_dir = "HTTP_Data"
output_dir = "Shells"
hex_key = "4d65bdbad183f00203b1e80cf96fba549663dabeab12fab153a921b346975cdd"
key = bytes.fromhex(hex_key)

def decrypt_data(encrypted_data):
    try:
        # AES-256-CBC
        iv = encrypted_data[:16]
        ciphertext = encrypted_data[16:]
        
        cipher = Cipher(algorithms.AES(key), modes.CBC(iv), backend=default_backend())
        decryptor = cipher.decryptor()
        decrypted_data = decryptor.update(ciphertext) + decryptor.finalize()
        return decrypted_data
    except Exception as e:
        return f"Decryption Error: {str(e)}".encode()

def process_file(filename):
    input_path = os.path.join(input_dir, filename)
    output_path = os.path.join(output_dir, filename + "_result")
    
    try:
        with open(input_path, "r", encoding="utf-8", errors="ignore") as f:
            content = f.read()
            
        payload_b64 = ""
        
        # Check for multipart/form-data
        if "Content-Disposition: form-data" in content:
            # Simple parsing: look for lines that are base64-like and not boundaries/headers
            lines = content.splitlines()
            valid_lines = []
            is_header = False
            for line in lines:
                line = line.strip()
                if not line: continue
                if line.startswith("--"): continue # Boundary
                if "Content-" in line: continue # Header
                
                # Assume this is payload part
                valid_lines.append(line)
            
            payload_b64 = "".join(valid_lines)
        else:
            # Assume raw base64 or other content
            payload_b64 = content.strip()
            
        if not payload_b64:
            print(f"Skipping {filename}: No payload found")
            return

        try:
            encrypted_data = base64.b64decode(payload_b64)
            decrypted_data = decrypt_data(encrypted_data)
            
            with open(output_path, "wb") as f:
                f.write(decrypted_data)
                
            print(f"Processed {filename} -> {output_path}")
            
        except Exception as e:
            print(f"Error decoding/decrypting {filename}: {e}")
            # Write raw content or error to result if decryption fails? 
            # User asked to decrypt, if it fails maybe just log it.
            # We'll try to write what we can or leave it.

    except Exception as e:
        print(f"Error reading {filename}: {e}")

# Main loop
if not os.path.exists(output_dir):
    os.makedirs(output_dir)

# Process shell.aspx, shell(0).aspx ... shell(63).aspx
files = os.listdir(input_dir)
# Filter strictly for shell*.aspx to match user request
shell_files = [f for f in files if f.startswith("shell") and f.endswith(".aspx")]

# Sort to process somewhat orderly (shell.aspx, shell(1), shell(2)...)
# Just sorting by name is enough for now
shell_files.sort()

print(f"Found {len(shell_files)} files to process.")

for filename in shell_files:
    process_file(filename)
