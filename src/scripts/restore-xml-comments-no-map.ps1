$currRoot = "E:\AI_Root\ChipPlatformSys\Admin.Core\src\modules\admin"
$bakRoot  = "E:\AI_Root\chip_platform_sys\Admin.Core\src\modules\admin"

if (-not (Test-Path $currRoot)) { Write-Host "当前目录不存在: $currRoot"; exit 1 }
if (-not (Test-Path $bakRoot))  { Write-Host "备份目录不存在: $bakRoot";  exit 1 }

$updated = 0; $skipped = 0; $missing = 0

function Get-DocRanges {
  param([string[]]$lines)
  $ranges = @()
  $i = 0; $n = $lines.Count
  while ($i -lt $n) {
    if ($lines[$i].TrimStart().StartsWith('///')) {
      $start = $i
      while ($i -lt $n -and $lines[$i].TrimStart().StartsWith('///')) { $i++ }
      $end = $i - 1
      $ranges += [pscustomobject]@{ Start = $start; End = $end }
    } else { $i++ }
  }
  return ,$ranges
}

Get-ChildItem -Path $currRoot -Recurse -Filter *.cs | ForEach-Object {
  $currFile = $_.FullName
  $relPath  = $currFile.Substring($currRoot.Length)
  $relPath  = $relPath.TrimStart([char[]]"\\/")

  # 构造候选备份路径（兼容 ChipSys.Admin.Core 与 ZhonTai.Admin.Core 差异）
  $candidates = @()
  $candidates += (Join-Path $bakRoot $relPath)
  if ($relPath -match 'ChipSys\.Admin\.Core') {
    $candidates += (Join-Path $bakRoot ($relPath -replace 'ChipSys\.Admin\.Core','ZhonTai.Admin.Core'))
  }
  if ($relPath -match 'ZhonTai\.Admin\.Core') {
    $candidates += (Join-Path $bakRoot ($relPath -replace 'ZhonTai\.Admin\.Core','ChipSys.Admin.Core'))
  }

  $bakFile = $null
  foreach ($cand in $candidates) {
    if (Test-Path $cand) { $bakFile = $cand; break }
  }

  if (-not $bakFile) { $missing++; return }

  $currLines = Get-Content -LiteralPath $currFile -Encoding UTF8
  $bakLines  = Get-Content -LiteralPath $bakFile  -Encoding UTF8

  $currRanges = Get-DocRanges -lines $currLines
  $bakRanges  = Get-DocRanges -lines $bakLines

  if ($currRanges.Count -eq 0 -or $bakRanges.Count -eq 0) { $skipped++; return }

  $count = [Math]::Min($currRanges.Count, $bakRanges.Count)
  $work = [System.Collections.Generic.List[string]]::new(); $work.AddRange($currLines)

  for ($k = $count - 1; $k -ge 0; $k--) {
    $cr = $currRanges[$k]; $br = $bakRanges[$k]
    $newBlock = $bakLines[$br.Start..$br.End]
    $work.RemoveRange($cr.Start, $cr.End - $cr.Start + 1)
    $work.InsertRange($cr.Start, $newBlock)
  }

  Set-Content -LiteralPath $currFile -Value $work -Encoding UTF8
  $updated++
}

Write-Host "完成(仅备份XML)。更新: $updated，跳过: $skipped，备份缺失: $missing"


