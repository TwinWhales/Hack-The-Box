import base64
import re

def parse_and_decode(file_path):
    with open(file_path, 'r') as f:
        content = f.read()

    vars = {}
    
    # Extract variable assignments
    # Supports VAR="VALUE" and VAR='VALUE'
    assignments = re.findall(r'(\w+)=[\'"](.*?)[\'"]', content, re.DOTALL)
    for k, v in assignments:
        vars[k] = v

    # Payload 1 parts
    keys1 = ["ABvnz", "QOPjH", "gQIxX"]
    payload1_b64 = "".join([vars.get(k, "") for k in keys1])
    try:
        if payload1_b64:
            payload1 = base64.b64decode(payload1_b64).decode('utf-8')
            print("=== Payload 1 ===")
            print(payload1)
        else:
            print("Payload 1 keys not found.")
    except Exception as e:
        print(f"Error decoding payload 1: {e}")

    # Payload 2 parts
    keys2 = ["LQebW", "gVR7i", "bkzHk", "q97up", "GYJan", "HJj6A", "fD9Kc", "hpAgs", "FqOPN", "CpJLT", "PIx1p"]
    payload2_b64 = "".join([vars.get(k, "") for k in keys2])
    try:
        if payload2_b64:
            payload2 = base64.b64decode(payload2_b64).decode('utf-8')
            print("\n=== Payload 2 ===")
            print(payload2)
        else:
            print("Payload 2 keys not found.")
    except Exception as e:
        print(f"Error decoding payload 2: {e}")

if __name__ == '__main__':
    parse_and_decode('decoded_layer1.sh')
