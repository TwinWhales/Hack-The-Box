import os
import urllib.parse
import base64

def extract_hidden_data():
    base_path = r"C:\Users\user\Desktop\Pentest\Forensic\Easy\Fishy HTTP\http_list"
    full_extracted_string = ""

    print("[-] Starting extraction...")

    for i in range(8):
        filename = f"submit_feedback({i})"
        filepath = os.path.join(base_path, filename)
        
        if not os.path.exists(filepath):
            print(f"[!] File not found: {filepath}")
            continue

        try:
            with open(filepath, 'r', encoding='utf-8') as f:
                content = f.read().strip()
                
            feedback_value = ""
            
            # Detect format
            if "feedback=" in content:
                # Format: URL encoded query string (e.g., Name=...&feedback=...)
                parsed = urllib.parse.parse_qs(content)
                if 'feedback' in parsed:
                    feedback_value = parsed['feedback'][0]
            elif "feedback: " in content:
                 # Format: Name: Value pairs
                 for line in content.split('\n'):
                     if line.startswith("feedback: "):
                         feedback_value = line.split("feedback: ", 1)[1].strip()
                         break
            
            if feedback_value:
                # Extract first letter of each word
                # Words are separated by spaces. 
                # Note: In URL encoded strings, spaces are '+' or '%20', but parse_qs handles that.
                words = feedback_value.split()
                hidden_chunk = "".join([word[0] for word in words if word])
                full_extracted_string += hidden_chunk
                print(f"[+] File {i}: Extracted chunk '{hidden_chunk}'")
            else:
                print(f"[!] File {i}: No feedback found")

        except Exception as e:
            print(f"[!] File {i}: Error reading file - {e}")

    print(f"\n[-] Full Extracted String: {full_extracted_string}")

    # Write to file for verification
    with open('result.txt', 'w', encoding='utf-8') as f:
        f.write(f"Full Extracted String: {full_extracted_string}\n")
    
    # Attempt Base64 Decode
    try:
        # Pad if necessary
        missing_padding = len(full_extracted_string) % 4
        if missing_padding:
            full_extracted_string += '=' * (4 - missing_padding)
            
        decoded_bytes = base64.b64decode(full_extracted_string)
        print("\n[-] Base64 Decoded (Preview):")
        print(decoded_bytes[:200]) # Print first 200 bytes to check
        
        # Determine if it's binary or text
        try:
            decoded_text = decoded_bytes.decode('utf-8')
            print("\n[-] Decoded Text:")
            print(decoded_text)
            
            with open('result.txt', 'a', encoding='utf-8') as f:
                f.write(f"Decoded Text:\n{decoded_text}\n")
                
        except UnicodeDecodeError:
            print("\n[!] Decoded content is binary.")
            with open('result.txt', 'a', encoding='utf-8') as f:
                f.write("Decoded content is binary.\n")

    except Exception as e:
        print(f"\n[!] Failed to decode Base64: {e}")

if __name__ == "__main__":
    extract_hidden_data()
