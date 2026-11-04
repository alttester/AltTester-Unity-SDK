$targetCSharp = "./Assets"
$targetJava = "./Bindings~/java"
$targetPyhon = "./Bindings~/python"

$copyrightCSharpAndJava = "/*
    Copyright(C) 2026 Altom Consulting
*/" + [System.Environment]::NewLine

$copyrightPython = '"""
    Copyright(C) 2026 Altom Consulting
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