param($installPath, $toolsPath, $package)

# Get all projects in the solution
$dte = Get-Interface $dte ([EnvDTE80.DTE2])
$solution = $dte.Solution

# Iterate through all projects
foreach ($proj in $solution.Projects) {
    try {
        if ($proj.Object -and $proj.Object.References) {
            $proj.Object.References | Where-Object { $_.Name -eq "SolidWorks.Interop.sldworks" } | ForEach-Object {
                if ($_.EmbedInteropTypes -eq $true) {
                    $_.EmbedInteropTypes = $false
                    Write-Host "Set EmbedInteropTypes=false for SolidWorks.Interop.sldworks in $($proj.Name)"
                }
            }
            $proj.Object.References | Where-Object { $_.Name -eq "SolidWorks.Interop.swconst" } | ForEach-Object {
                if ($_.EmbedInteropTypes -eq $true) {
                    $_.EmbedInteropTypes = $false
                    Write-Host "Set EmbedInteropTypes=false for SolidWorks.Interop.swconst in $($proj.Name)"
                }
            }
            $proj.Object.References | Where-Object { $_.Name -eq "SolidWorks.Interop.swpublished" } | ForEach-Object {
                if ($_.EmbedInteropTypes -eq $true) {
                    $_.EmbedInteropTypes = $false
                    Write-Host "Set EmbedInteropTypes=false for SolidWorks.Interop.swpublished in $($proj.Name)"
                }
            }
        }
    }
    catch {
        # Skip projects that don't support References (like solution folders)
    }
}
