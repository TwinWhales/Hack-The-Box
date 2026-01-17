
import base64
import glob
import os
from Crypto.Cipher import DES
from Crypto.Util.Padding import unpad

# Credentials from AgentServer.cs
KEY = b"AKaPdSgV"
IV = b"QeThWmYq"

def decrypt_content(encrypted_b64):
    try:
        # Decode Base64
        ciphertext = base64.b64decode(encrypted_b64)
        
        # Setup DES Cipher in CBC mode
        cipher = DES.new(KEY, DES.MODE_CBC, IV)
        
        # Decrypt and Unpad
        padded_plaintext = cipher.decrypt(ciphertext)
        plaintext = unpad(padded_plaintext, DES.block_size)
        
        return plaintext.decode('utf-8', errors='replace')
    except Exception as e:
        return f"[Decryption Error: {e}]"

def main():
    target_dir = "mnt/sessions"
    enc_files = glob.glob(os.path.join(target_dir, "*.log.enc"))
    
    if not enc_files:
        print(f"No .log.enc files found in {target_dir}")
        return

    for enc_file in enc_files:
        print(f"\nProcessing: {enc_file}")
        dec_content = []
        
        try:
            with open(enc_file, 'r', encoding='utf-8') as f:
                lines = f.readlines()
            
            for line in lines:
                line = line.strip()
                if not line:
                    continue
                
                decrypted_line = decrypt_content(line)
                print(f"  Decrypted: {decrypted_line}")
                dec_content.append(decrypted_line)
            
            # Save decrypted file
            dec_file = enc_file.replace(".log.enc", ".log.dec")
            with open(dec_file, 'w', encoding='utf-8') as f:
                f.write("\n".join(dec_content))
            print(f"  > Saved decrypted log to: {dec_file}")
            
        except Exception as e:
            print(f"  Error reading file: {e}")

if __name__ == "__main__":
    main()
