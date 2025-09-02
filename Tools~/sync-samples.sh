#!/bin/sh
echo "Syncing Samples -> Samples~ ..."

if command -v rsync >/dev/null 2>&1; then
    rsync -a --delete "Samples/" "Samples~/"
elif command -v robocopy >/dev/null 2>&1; then
    robocopy Samples Samples~ /MIR >nul
else
    echo "Error: need rsync (Linux/macOS) or robocopy (Windows)."
    exit 1
fi

# Stage changes so they're ready to commit
git add Samples~/