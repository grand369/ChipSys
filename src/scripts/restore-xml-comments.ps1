$currRoot = "E:\AI_Root\ChipPlatformSys\Admin.Core\src\modules\admin"
$bakRoot  = "E:\AI_Root\chip_platform_sys\Admin.Core\src\modules\admin"

if (-not (Test-Path $currRoot)) { Write-Host "当前目录不存在: $currRoot"; exit 1 }
if (-not (Test-Path $bakRoot))  { Write-Host "备份目录不存在: $bakRoot";  exit 1 }

$updated = 0; $skipped = 0; $missing = 0

function Merge-XmlComments {
  param([string[]]$currLines, [string[]]$bakLines)

  $out = New-Object System.Collections.Generic.List[string]
  $i = 0; $j = 0
  $currLen = $currLines.Count; $bakLen = $bakLines.Count

  while ($i -lt $currLen -and $j -lt $bakLen) {
    $c = $currLines[$i]
    $b = $bakLines[$j]

    $cIsDoc = $c.TrimStart().StartsWith("///")
    $bIsDoc = $b.TrimStart().StartsWith("///")

    if ($cIsDoc -and $bIsDoc) {
      $currBlock = @(); while ($i -lt $currLen -and $currLines[$i].TrimStart().StartsWith("///")) { $currBlock += $currLines[$i]; $i++ }
      $bakBlock  = @(); while ($j -lt $bakLen  -and $bakLines[$j].TrimStart().StartsWith("///"))  { $bakBlock  += $bakLines[$j];  $j++ }
      foreach ($line in $bakBlock) { $out.Add($line) }
    }
    elseif (-not $cIsDoc -and -not $bIsDoc) {
      $out.Add($c); $i++; $j++
    }
    else {
      $out.Add($c); $i++
      while ($j -lt $bakLen -and $bakLines[$j].TrimStart().StartsWith("///")) { $j++ }
    }
  }

  while ($i -lt $currLen) { $out.Add($currLines[$i]); $i++ }
  return ,$out.ToArray()
}

Get-ChildItem -Path $currRoot -Recurse -Filter *.cs | ForEach-Object {
  $currFile = $_.FullName
  $relPath  = $currFile.Substring($currRoot.Length)
  $relPath  = $relPath.TrimStart([char[]]"\\/")
  $bakFile  = Join-Path $bakRoot $relPath

  if (-not (Test-Path $bakFile)) { $missing++ ; return }

  $currLines = Get-Content -LiteralPath $currFile -Encoding UTF8
  $bakLines  = Get-Content -LiteralPath $bakFile  -Encoding UTF8

  $bakDocCnt = ($bakLines | Where-Object { $_.TrimStart().StartsWith("///") }).Count
  if ($bakDocCnt -eq 0) { $skipped++; return }

  $merged = Merge-XmlComments -currLines $currLines -bakLines $bakLines

  if (-not (@($merged) -join "`n").Equals((@($currLines) -join "`n"))) {
    Set-Content -LiteralPath $currFile -Value $merged -Encoding UTF8
    $updated++
  } else {
    $skipped++
  }
}

Write-Host "完成。更新文件: $updated，跳过: $skipped，备份缺失: $missing"


