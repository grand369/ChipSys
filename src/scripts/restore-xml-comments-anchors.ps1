$currRoot = "E:\AI_Root\ChipPlatformSys\Admin.Core\src\modules\admin"
$bakRoot  = "E:\AI_Root\chip_platform_sys\Admin.Core\src\modules\admin"

if (-not (Test-Path $currRoot)) { Write-Host "当前目录不存在: $currRoot"; exit 1 }
if (-not (Test-Path $bakRoot))  { Write-Host "备份目录不存在: $bakRoot";  exit 1 }

$updated = 0; $skipped = 0; $missing = 0

function Get-DocBlocksWithAnchors {
  param([string[]]$lines)

  $results = @()
  $i = 0; $n = $lines.Count
  while ($i -lt $n) {
    if ($lines[$i].TrimStart().StartsWith('///')) {
      $start = $i
      while ($i -lt $n -and $lines[$i].TrimStart().StartsWith('///')) { $i++ }
      $end = $i - 1
      # 找到注释后的第一个非空且非注释的代码行，作为锚点
      while ($i -lt $n -and ($lines[$i].Trim().Length -eq 0 -or $lines[$i].TrimStart().StartsWith('//'))) { $i++ }
      if ($i -lt $n) {
        $anchor = $lines[$i].Trim()
        $block  = $lines[$start..$end]
        $results += [pscustomobject]@{ Anchor = $anchor; Start = $start; End = $end; Block = $block }
      }
    } else {
      $i++
    }
  }
  return ,$results
}

Get-ChildItem -Path $currRoot -Recurse -Filter *.cs | ForEach-Object {
  $currFile = $_.FullName
  $relPath  = $currFile.Substring($currRoot.Length)
  $relPath  = $relPath.TrimStart([char[]]"\\/")
  $bakFile  = Join-Path $bakRoot $relPath

  if (-not (Test-Path $bakFile)) { $missing++; return }

  $currLines = Get-Content -LiteralPath $currFile -Encoding UTF8
  $bakLines  = Get-Content -LiteralPath $bakFile  -Encoding UTF8

  $currBlocks = Get-DocBlocksWithAnchors -lines $currLines
  if ($currBlocks.Count -eq 0) { $skipped++; return }
  $bakBlocks  = Get-DocBlocksWithAnchors -lines $bakLines

  # 建立备份锚点到块的映射
  $bakMap = @{}
  foreach ($b in $bakBlocks) { if (-not $bakMap.ContainsKey($b.Anchor)) { $bakMap[$b.Anchor] = $b.Block } }

  $modified = $false
  # 因为替换会改变索引，先收集替换操作
  $replacements = @()
  foreach ($c in $currBlocks) {
    if ($bakMap.ContainsKey($c.Anchor)) {
      $replacements += [pscustomobject]@{ Start = $c.Start; End = $c.End; NewBlock = $bakMap[$c.Anchor] }
    }
  }

  if ($replacements.Count -gt 0) {
    # 从后往前替换，避免索引偏移
    $replacements = $replacements | Sort-Object Start -Descending
    $work = [System.Collections.Generic.List[string]]::new()
    $work.AddRange($currLines)
    foreach ($r in $replacements) {
      $work.RemoveRange($r.Start, $r.End - $r.Start + 1)
      $work.InsertRange($r.Start, $r.NewBlock)
      $modified = $true
    }
    if ($modified) {
      Set-Content -LiteralPath $currFile -Value $work -Encoding UTF8
      $updated++
    } else { $skipped++ }
  } else {
    $skipped++
  }
}

Write-Host "完成(锚点方式)。更新文件: $updated，跳过: $skipped，备份缺失: $missing"


