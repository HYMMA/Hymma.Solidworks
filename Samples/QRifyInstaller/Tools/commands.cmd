Start "" "C:\users\WDAGUtilityAccount\Downloads\Setup"
Start "" %localappddata%
start "" "C:\Program files"

msiexec /i C:\users\WDAGUtilityAccount\Downloads\Setup\qrifyInstaller.msi /l*v C:\users\WDAGUtilityAccount\Downloads\Setup\logs.txt

Start appwiz.cpl