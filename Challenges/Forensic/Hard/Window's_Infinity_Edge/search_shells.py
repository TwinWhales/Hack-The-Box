import os

output_dir = "Shells"
target_string = "HTB"

if not os.path.exists(output_dir):
    print(f"Directory {output_dir} does not exist.")
    exit()

files = os.listdir(output_dir)
files.sort()

print(f"Scanning {len(files)} files in {output_dir}...")

for filename in files:
    filepath = os.path.join(output_dir, filename)
    if os.path.getsize(filepath) == 0:
        continue
        
    try:
        with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
            content = f.read()
            if target_string.lower() in content.lower():
                print(f"\n[-] MATCH FOUND in {filename}:")
                # Print 200 characters of context around the match
                idx = content.lower().find(target_string.lower())
                start = max(0, idx - 100)
                end = min(len(content), idx + 200)
                print(f"    ...{content[start:end]}...")
    except Exception as e:
        print(f"Error reading {filename}: {e}")
