$targetCSharp = "./Assets"
$targetJava = "./Bindings~/java"
$targetPyhon = "./Bindings~/python"

$copyrightCSharpAndJava = "/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/" + [System.Environment]::NewLine

$copyrightPython = '"""
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
"""' + [System.Environment]::NewLine

function Write-Header ($file, $header) {
    $content = Get-Content $file
    $hasCopyright = $true
    $headers = $header.Split([System.Environment]::NewLine, [System.StringSplitOptions]::RemoveEmptyEntries)
    $contents = $content.Split([System.Environment]::NewLine, [System.StringSplitOptions]::RemoveEmptyEntries)[0..$headers.Count]
    for($i=0; $i -lt $headers.Count; $i++)
    {
        if(-not($contents[$i] -like $headers[$i]))
        {
            $hasCopyright = $false
        }
    }
    if(-not $hasCopyright)
    {
        $filename = Split-Path -Leaf $file
        $fileheader = $header -f $filename
        Set-Content $file $fileheader
        Add-Content $file $content
    }
}

Get-ChildItem $targetCSharp -Recurse | ? { $_.Extension -like ".cs" } | % {
    Write-Header $_.PSPath.Split(":",3)[2] $copyrightCSharpAndJava
}

Get-ChildItem $targetJava -Recurse | ? { $_.Extension -like ".java" } | % {
    Write-Header $_.PSPath.Split(":",3)[2] $copyrightCSharpAndJava
}

Get-ChildItem $targetPyhon -Recurse | ? { $_.Extension -like ".py" } | % {
    Write-Header $_.PSPath.Split(":",3)[2] $copyrightPython
}