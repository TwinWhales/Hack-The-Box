import re
import base64

def analyze():
    with open('http_files/VgLy8V0Zxo', 'r') as f:
        content = f.read()
    
    # Extract the string assigned to s
    match = re.search(r's=" \'([^\']+)\'', content)
    if not match:
        print("Could not find variable s")
        return

    payload_reversed = match.group(1)
    
    # Reverse the string
    payload_b64 = payload_reversed[::-1]
    
    try:
        decoded = base64.b64decode(payload_b64).decode('utf-8')
        with open('decoded_layer1.sh', 'w') as out:
            out.write(decoded)
        print("Successfully wrote first layer to decoded_layer1.sh")
    except Exception as e:
        print(f"Error decoding: {e}")

if __name__ == "__main__":
    analyze()
