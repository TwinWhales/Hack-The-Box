import struct
import re

# File path to read data from
data_file_path = "data"

def extract_array(name, content):
    # Adjust regex to handle whitespace and multiline
    match = re.search(f"{name}\s*=\s*\{{(.*?)\}};", content, re.DOTALL)
    if match:
        # Convert split strings to int, filter out empty strings
        return [int(x.strip()) for x in match.group(1).split(',') if x.strip()]
    return []

try:
    with open(data_file_path, "r") as f:
        content = f.read()
except FileNotFoundError:
    print(f"Error: File '{data_file_path}' not found.")
    exit()

# Extract arrays
int_arr = extract_array("int_arr", content)
int_arr_r = extract_array("int_arr_r", content)

print(f"Loaded int_arr with {len(int_arr)} elements")
print(f"Loaded int_arr_r with {len(int_arr_r)} elements")

# Validate array lengths
if len(int_arr) != len(int_arr_r) or len(int_arr) == 0:
    print(f"Error: Arrays are empty or length mismatch. (int_arr: {len(int_arr)}, int_arr_r: {len(int_arr_r)})")
    exit()

output_bytes = bytearray()

# Decryption logic (Replicating C# logic)
# C# Code: int_arr[i] = (int_arr[i] * 345300 + int_arr_r[i]);
for i in range(len(int_arr)):
    v1 = int_arr[i]
    v2 = int_arr_r[i]
    
    # Perform calculation
    # C# ulong is 64-bit, so we mask with 0xFFFFFFFFFFFFFFFF to simulate overflow behavior
    calculated_val = (v1 * 345300 + v2) & 0xFFFFFFFFFFFFFFFF
    
    # Convert to bytes (Little Endian, Unsigned Long Long = <Q)
    # C# System.Buffer.BlockCopy is byte-copy, effectively Little Endian on Windows
    output_bytes += struct.pack('<Q', calculated_val)

# Save to file
filename = "extracted_payload.dll"
with open(filename, "wb") as f:
    f.write(output_bytes)

print(f"[+] Decryption complete! '{filename}' has been created.")