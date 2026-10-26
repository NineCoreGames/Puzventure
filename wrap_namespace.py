#!/usr/bin/env python3
"""
Wrap C# files in a namespace (for Unity scripts).

Usage:
  python3 wrap_namespace.py Assets/Gokturk/_Project/Scripts
  python3 wrap_namespace.py Assets/Gokturk/_Project/Scripts/Player/PlayerJump.cs Other.cs
  python3 wrap_namespace.py --ns Puzventure.Gokturk --dry-run Assets/Gokturk

- Accepts files and/or folders (folders are searched recursively for .cs files).
- Leaves `using` lines (and leading comments) at the top, wraps everything else.
- Skips files that already declare a namespace.
- Keeps the file's encoding (BOM) and line endings.
"""

import argparse
import os
import re
import sys

DEFAULT_NS = "Puzventure.Gokturk"
NAMESPACE_RE = re.compile(r"^\s*namespace\s+[\w.]+", re.MULTILINE)


def collect_files(paths):
    files = []
    for p in paths:
        if os.path.isdir(p):
            for root, _, names in os.walk(p):
                for n in sorted(names):
                    if n.endswith(".cs"):
                        files.append(os.path.join(root, n))
        elif p.endswith(".cs") and os.path.isfile(p):
            files.append(p)
        else:
            print(f"  skip (not a .cs file or folder): {p}")
    return files


def is_header_line(line):
    s = line.strip()
    return (
        s == ""
        or s.startswith("using ")
        or s.startswith("//")
        or s.startswith("/*")
        or s.startswith("*")
        or s.startswith("*/")
    )


def wrap(text, ns, newline):
    lines = text.split(newline)

    # Header = leading using/comment/blank lines
    i = 0
    while i < len(lines) and is_header_line(lines[i]):
        i += 1
    header = lines[:i]
    body = lines[i:]

    # Trim blank lines at the edges
    while header and header[-1].strip() == "":
        header.pop()
    while body and body[-1].strip() == "":
        body.pop()

    indented = [("    " + l) if l.strip() else "" for l in body]

    out = []
    if header:
        out += header + [""]
    out += [f"namespace {ns}", "{"] + indented + ["}", ""]
    return newline.join(out)


def process(path, ns, dry_run):
    with open(path, "rb") as f:
        raw = f.read()

    has_bom = raw.startswith(b"\xef\xbb\xbf")
    # Try UTF-8 first, then Windows Turkish (cp1254), then latin-1 (never fails)
    encoding = None
    for enc in ("utf-8-sig", "cp1254", "latin-1"):
        try:
            text = raw.decode(enc)
            encoding = enc
            break
        except UnicodeDecodeError:
            continue
    if encoding != "utf-8-sig":
        print(f"  note: {path} is not UTF-8, read as {encoding} (kept as {encoding})")
    newline = "\r\n" if "\r\n" in text else "\n"

    if NAMESPACE_RE.search(text):
        print(f"  already has namespace, skipped: {path}")
        return False

    new_text = wrap(text, ns, newline)

    if dry_run:
        print(f"  would wrap: {path}")
        return True

    data = new_text.encode("utf-8" if encoding == "utf-8-sig" else encoding)
    if has_bom:
        data = b"\xef\xbb\xbf" + data
    with open(path, "wb") as f:
        f.write(data)
    print(f"  wrapped: {path}")
    return True


def main():
    ap = argparse.ArgumentParser(description="Wrap C# files in a namespace.")
    ap.add_argument("paths", nargs="+", help=".cs files and/or folders")
    ap.add_argument("--ns", default=DEFAULT_NS, help=f"namespace (default: {DEFAULT_NS})")
    ap.add_argument("--dry-run", action="store_true", help="show what would change, write nothing")
    args = ap.parse_args()

    files = collect_files(args.paths)
    if not files:
        print("No .cs files found.")
        sys.exit(1)

    changed = sum(process(f, args.ns, args.dry_run) for f in files)
    verb = "would change" if args.dry_run else "changed"
    print(f"\nDone: {changed} of {len(files)} file(s) {verb}. Namespace: {args.ns}")


if __name__ == "__main__":
    main()