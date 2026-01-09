
from Crypto.Cipher import AES
import hashlib
import os

def decrypt_file(filename_in, filename_out, key, iv):
    """Decrypts a file using AES in CFB mode."""
    try:
        with open(filename_in, "rb") as f_in:
            encrypted_data = f_in.read()

        cipher = AES.new(key.encode("utf-8"), AES.MODE_CFB, iv)
        decrypted_data = cipher.decrypt(encrypted_data)

        with open(filename_out, "wb") as f_out:
            f_out.write(decrypted_data)
            
        return decrypted_data
            
    except FileNotFoundError:
        print(f"Error: Input file not found at {filename_in}")
        return None
    except Exception as e:
        print(f"An error occurred: {e}")
        return None

def get_md5(data):
    """Calculates the MD5 hash of the given data."""
    md5_hash = hashlib.md5(data).hexdigest()
    return md5_hash

def main():
    key = "vN0nb7ZshjAWiCzv"
    iv = b'ffTC776Wt59Qawe1'
    
    encrypted_file = "candy_dungeon.pdf.boo"
    decrypted_file = "candy_dungeon.pdf"

    # Decrypt the file
    decrypted_content = decrypt_file(encrypted_file, decrypted_file, key, iv)

    if decrypted_content:
        # Calculate MD5 sum
        md5_sum = get_md5(decrypted_content)
        print(f"File decrypted successfully to {decrypted_file}")
        print(f"MD5: {md5_sum}")

if __name__ == "__main__":
    main()
