# Writes the Altom copyright header at the top of the C#, Java and Python sources.
#
# - The driver (Assets/AltTester/Runtime/AltDriver) and the bindings (Bindings~) aren't GPL: they always get
#   the copyright only, never the GPL notice.
# - Third-party code (any 3rdParty folder) keeps its own license and gets no header.
# - Any other file keeps the kind of header it has (GPL notice or copyright only), only the year is updated.
#   A file without a header gets the GPL notice.

$year = 2026

$root = $PSScriptRoot
$targetCSharp = Join-Path $root "Assets"
$targetJava = Join-Path $root "Bindings~/java"
$targetPython = Join-Path $root "Bindings~/python"
$driverFolder = [System.IO.Path]::GetFullPath((Join-Path $root "Assets/AltTester/Runtime/AltDriver")) +
    [System.IO.Path]::DirectorySeparatorChar

$copyright = "    Copyright(C) $year Altom Consulting"
$gplNotice = @(
    ""
    "    This program is free software: you can redistribute it and/or modify"
    "    it under the terms of the GNU General Public License as published by"
    "    the Free Software Foundation, either version 3 of the License, or"
    "    (at your option) any later version."
    ""
    "    This program is distributed in the hope that it will be useful,"
    "    but WITHOUT ANY WARRANTY; without even the implied warranty of"
    "    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the"
    "    GNU General Public License for more details."
    ""
    "    You should have received a copy of the GNU General Public License"
    "    along with this program. If not, see <https://www.gnu.org/licenses/>."
)

$cStyle = @{ Open = "/*"; Close = "*/"; Pattern = '\A\s*/\*[\s\S]*?\*/[ \t]*(\r?\n)?' }
$pythonStyle = @{ Open = '"""'; Close = '"""'; Pattern = '\A\s*"""[\s\S]*?"""[ \t]*(\r?\n)?' }

$copyrightPattern = '^\s*Copyright\(C\) \d{4} Altom Consulting\s*$'
$gplNoticeLines = $gplNotice | ForEach-Object { $_.Trim() } | Where-Object { $_ }

# True when the comment holds only the Altom copyright and, optionally, the GPL notice
function Test-Header([string]$comment, $style) {
    $inner = $comment.Trim()
    $inner = $inner.Substring($style.Open.Length, $inner.Length - $style.Open.Length - $style.Close.Length)
    $hasCopyright = $false
    foreach ($line in ($inner -split "\r?\n")) {
        if ($line -match $copyrightPattern) {
            $hasCopyright = $true
        } elseif ($line.Trim() -and $gplNoticeLines -notcontains $line.Trim()) {
            return $false
        }
    }
    return $hasCopyright
}

function Get-Header($style, [bool]$gpl, [string]$eol) {
    $lines = @($style.Open, $copyright)
    if ($gpl) { $lines += $gplNotice }
    $lines += $style.Close
    return $lines -join $eol
}

# $license: "gpl", "nongpl", or "keep" to keep the kind of header the file has
function Write-Header([string]$path, $style, [string]$license) {
    $bytes = [System.IO.File]::ReadAllBytes($path)
    $hasBom = $bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF
    $content = [System.IO.File]::ReadAllText($path, [System.Text.Encoding]::UTF8)
    $eol = "`n"
    if ($content.Contains("`r`n")) { $eol = "`r`n" }

    # Strip the Altom header(s) the file starts with, some files have more than one
    $rest = $content
    $found = $false
    $hadGpl = $false
    while ($true) {
        $match = [regex]::Match($rest, $style.Pattern)
        if (-not $match.Success) { break }
        if (-not (Test-Header $match.Value $style)) {
            # Other text in the comment would be lost, or a second header added above it
            if ($match.Value.Contains("Altom Consulting")) {
                Write-Warning "Skipped $path, its Altom header holds other text, edit it by hand"
                return
            }
            break
        }
        if ($match.Value.Contains("GNU General Public License")) { $hadGpl = $true }
        $rest = $rest.Substring($match.Length)
        $found = $true
    }

    $gpl = $license -eq "gpl"
    if ($license -eq "keep") { $gpl = $hadGpl -or -not $found }

    $header = Get-Header $style $gpl $eol
    if ($found) {
        $newContent = $header + $eol + $rest
    } else {
        $newContent = $header + $eol + $eol + $content
    }

    if ($newContent -cne $content) {
        [System.IO.File]::WriteAllText($path, $newContent, (New-Object System.Text.UTF8Encoding($hasBom)))
        Write-Output "Updated $path"
    }
}

function Get-Sources([string]$folder, [string]$extension) {
    Get-ChildItem $folder -Recurse -File |
        Where-Object { $_.Extension -eq $extension -and $_.FullName -notmatch '[\\/]3rdParty[\\/]' }
}

Get-Sources $targetCSharp ".cs" | ForEach-Object {
    $license = "keep"
    if ($_.FullName.StartsWith($driverFolder, [System.StringComparison]::OrdinalIgnoreCase)) { $license = "nongpl" }
    Write-Header $_.FullName $cStyle $license
}

Get-Sources $targetJava ".java" | ForEach-Object {
    Write-Header $_.FullName $cStyle "nongpl"
}

Get-Sources $targetPython ".py" | ForEach-Object {
    Write-Header $_.FullName $pythonStyle "nongpl"
}
