param(
    [string]$Branch = "fix/wpf-winforms-interop",
    [string]$BaseBranch = "master",
    [switch]$DryRun
)

$ErrorActionPreference = 'Stop'

Write-Host "Creating branch $Branch" -ForegroundColor Cyan
git checkout -b $Branch
if ($LASTEXITCODE -ne 0) { throw "git checkout failed" }

Write-Host "Validating patch..." -ForegroundColor Cyan
git apply --check "SwPopupWindow.patch"
if ($LASTEXITCODE -ne 0) { throw "Patch validation failed" }

if ($DryRun) {
  Write-Host "Dry run complete; patch validated. Exiting." -ForegroundColor Yellow
  exit 0
}

Write-Host "Applying patch..." -ForegroundColor Cyan
git apply "SwPopupWindow.patch"
if ($LASTEXITCODE -ne 0) { throw "Patch apply failed" }

Write-Host "Committing..." -ForegroundColor Cyan
git add "Addins/UI/PopUps/SwPopupWindow.cs"
git commit -F "COMMIT_MESSAGE.txt"
if ($LASTEXITCODE -ne 0) { throw "git commit failed" }

Write-Host "Pushing..." -ForegroundColor Cyan
git push -u origin $Branch
if ($LASTEXITCODE -ne 0) { throw "git push failed" }

Write-Host "Creating PR..." -ForegroundColor Cyan
# Requires GitHub CLI (gh) to be installed and authenticated
gh pr create --base $BaseBranch --head $Branch --title "Harden WinForms/WPF interop in SwPopupWindow" --body-file "PR.md"
if ($LASTEXITCODE -ne 0) { throw "Failed to create PR (gh CLI)" }

Write-Host "Done." -ForegroundColor Green
