
import hashlib
import sys
from Crypto.Cipher import AES
from pathlib import Path

# Configuration
ENCRYPTED_FILE = Path("http_files/9tVI0")
OUTPUT_FILE = Path("decrypted_payload.bin")
PASSWORD = "z64&Rx27Z$B%73up"

def decrypt():
    try:
        # Read encrypted data
        if not ENCRYPTED_FILE.exists():
            print(f"Error: {ENCRYPTED_FILE} not found.")
            return

        data = ENCRYPTED_FILE.read_bytes()
        
        # Derive Key
        key = hashlib.sha256(PASSWORD.encode("utf-8")).digest()
        
        # Extract IV (First 16 bytes)
        iv = data[:16]
        ciphertext = data[16:]
        
        # Decrypt
        cipher = AES.new(key, AES.MODE_CBC, iv)
        plaintext = cipher.decrypt(ciphertext)
        
        # Remove PKCS7 padding
        pad_len = plaintext[-1]
        plaintext = plaintext[:-pad_len]
        
        # Save output
        OUTPUT_FILE.write_bytes(plaintext)
        print(f"Decryption successful. Saved to {OUTPUT_FILE}")
        
        # Print magic bytes for identification
        print(f"First 16 bytes of decrypted payload: {plaintext[:16].hex()}")

    except Exception as e:
        print(f"Error decrypting: {e}")

if __name__ == "__main__":
    decrypt()
